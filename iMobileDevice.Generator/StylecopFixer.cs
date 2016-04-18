using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using StyleCop.Analyzers.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace iMobileDevice.Generator
{
    public class StylecopFixer
    {
        public static void Run(string projectPath)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress +=
                (sender, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

            // Since Console apps do not have a SynchronizationContext, we're leveraging the built-in support
            // in WPF to pump the messages via the Dispatcher.
            // See the following for additional details:
            //   http://blogs.msdn.com/b/pfxteam/archive/2012/01/21/10259307.aspx
            //   https://github.com/DotNetAnalyzers/StyleCopAnalyzers/pull/1362
            SynchronizationContext previousContext = SynchronizationContext.Current;
            try
            {
                var context = new DispatcherSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(context);

                DispatcherFrame dispatcherFrame = new DispatcherFrame();
                Task mainTask = RunAsync(projectPath, cts.Token);
                mainTask.ContinueWith(task => dispatcherFrame.Continue = false);

                Dispatcher.PushFrame(dispatcherFrame);
                mainTask.GetAwaiter().GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        private static async Task RunAsync(string projectPath, CancellationToken cancellationToken)
        {
            var analyzers = GetAllAnalyzers();

            if (analyzers.Length == 0)
            {
                Console.WriteLine("No analyzers found");
                return;
            }

            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            var project = await workspace.OpenProjectAsync(projectPath, cancellationToken);

            var diagnostics = await GetAnalyzerDiagnosticsAsync(project, analyzers, true, cancellationToken).ConfigureAwait(true);
            var allDiagnostics = diagnostics.SelectMany(i => i.Value).ToImmutableArray();

            await TestCodeFixesAsync(project, allDiagnostics, cancellationToken).ConfigureAwait(true);
        }

        private static ImmutableArray<DiagnosticAnalyzer> GetAllAnalyzers()
        {
            var assemblyNames = new string[] { "StyleCop.Analyzers.dll" };
            var analyzers = ImmutableArray.CreateBuilder<DiagnosticAnalyzer>();

            foreach (var assemblyName in assemblyNames)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(Path.GetFullPath(assemblyName));

                    var diagnosticAnalyzerType = typeof(DiagnosticAnalyzer);

                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.IsSubclassOf(diagnosticAnalyzerType) && !type.IsAbstract)
                        {
                            analyzers.Add((DiagnosticAnalyzer)Activator.CreateInstance(type));
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"An error occurred while loading {ex.FileName}");
                }
            }

            return analyzers.ToImmutable();
        }

        private static async Task TestCodeFixesAsync(Project project, ImmutableArray<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            Console.WriteLine("Calculating fixes");

            List<CodeAction> fixes = new List<CodeAction>();

            var codeFixers = GetAllCodeFixers();

            foreach (var item in diagnostics)
            {
                foreach (var codeFixer in codeFixers.GetValueOrDefault(item.Id, ImmutableList.Create<CodeFixProvider>()))
                {
                    fixes.AddRange(await GetFixesAsync(project, codeFixer, item, cancellationToken).ConfigureAwait(false));
                }
            }

            Console.WriteLine($"Found {fixes.Count} potential code fixes");

            Console.WriteLine("Calculating changes");

            object lockObject = new object();

            foreach (var fix in fixes)
            {
                try
                {
                    var operations = await fix.GetOperationsAsync(cancellationToken);

                    if (operations.Length == 1)
                    {
                        var operation = operations.Single();

                        operation.Apply(project.Solution.Workspace, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    // Report thrown exceptions
                    lock (lockObject)
                    {
                        Console.WriteLine($"The fix '{fix.Title}' threw an exception:");
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        private static ImmutableDictionary<string, ImmutableList<CodeFixProvider>> GetAllCodeFixers()
        {
            var assemblyNames = new string[] { "StyleCop.Analyzers.CodeFixes.dll" };

            Dictionary<string, ImmutableList<CodeFixProvider>> providers = new Dictionary<string, ImmutableList<CodeFixProvider>>();

            foreach (var assemblyName in assemblyNames)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(Path.GetFullPath(assemblyName));

                    var codeFixProviderType = typeof(CodeFixProvider);

                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.IsSubclassOf(codeFixProviderType) && !type.IsAbstract)
                        {
                            var codeFixProvider = (CodeFixProvider)Activator.CreateInstance(type);

                            foreach (var diagnosticId in codeFixProvider.FixableDiagnosticIds)
                            {
                                providers.AddToInnerList(diagnosticId, codeFixProvider);
                            }
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"An error occurred while loading {ex.FileName}");
                }
            }

            return providers.ToImmutableDictionary();
        }

        private static async Task<IEnumerable<CodeAction>> GetFixesAsync(Project project, CodeFixProvider codeFixProvider, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            List<CodeAction> codeActions = new List<CodeAction>();

            await codeFixProvider.RegisterCodeFixesAsync(new CodeFixContext(project.GetDocument(diagnostic.Location.SourceTree), diagnostic, (a, d) => codeActions.Add(a), cancellationToken)).ConfigureAwait(false);

            return codeActions;
        }

        private static async Task<ImmutableDictionary<ProjectId, ImmutableArray<Diagnostic>>> GetAnalyzerDiagnosticsAsync(Project project, ImmutableArray<DiagnosticAnalyzer> analyzers, bool force, CancellationToken cancellationToken)
        {
            List<KeyValuePair<ProjectId, Task<ImmutableArray<Diagnostic>>>> projectDiagnosticTasks = new List<KeyValuePair<ProjectId, Task<ImmutableArray<Diagnostic>>>>();

            // Make sure we analyze the projects in parallel
            if (project.Language != LanguageNames.CSharp)
            {
                throw new ArgumentOutOfRangeException(nameof(project));
            }

            projectDiagnosticTasks.Add(new KeyValuePair<ProjectId, Task<ImmutableArray<Diagnostic>>>(project.Id, GetProjectAnalyzerDiagnosticsAsync(analyzers, project, force, cancellationToken)));

            ImmutableDictionary<ProjectId, ImmutableArray<Diagnostic>>.Builder projectDiagnosticBuilder = ImmutableDictionary.CreateBuilder<ProjectId, ImmutableArray<Diagnostic>>();
            foreach (var task in projectDiagnosticTasks)
            {
                projectDiagnosticBuilder.Add(task.Key, await task.Value.ConfigureAwait(false));
            }

            return projectDiagnosticBuilder.ToImmutable();
        }

        /// <summary>
        /// Returns a list of all analyzer diagnostics inside the specific project. This is an asynchronous operation.
        /// </summary>
        /// <param name="analyzers">The list of analyzers that should be used</param>
        /// <param name="project">The project that should be analyzed</param>
        /// <param name="force"><see langword="true"/> to force the analyzers to be enabled; otherwise,
        /// <see langword="false"/> to use the behavior configured for the specified <paramref name="project"/>.</param>
        /// <param name="cancellationToken">The cancellation token that the task will observe.</param>
        /// <returns>A list of diagnostics inside the project</returns>
        private static async Task<ImmutableArray<Diagnostic>> GetProjectAnalyzerDiagnosticsAsync(ImmutableArray<DiagnosticAnalyzer> analyzers, Project project, bool force, CancellationToken cancellationToken)
        {
            var supportedDiagnosticsSpecificOptions = new Dictionary<string, ReportDiagnostic>();
            if (force)
            {
                foreach (var analyzer in analyzers)
                {
                    foreach (var diagnostic in analyzer.SupportedDiagnostics)
                    {
                        // make sure the analyzers we are testing are enabled
                        supportedDiagnosticsSpecificOptions[diagnostic.Id] = ReportDiagnostic.Default;
                    }
                }
            }

            // Report exceptions during the analysis process as errors
            supportedDiagnosticsSpecificOptions.Add("AD0001", ReportDiagnostic.Error);

            // update the project compilation options
            var modifiedSpecificDiagnosticOptions = supportedDiagnosticsSpecificOptions.ToImmutableDictionary().SetItems(project.CompilationOptions.SpecificDiagnosticOptions);
            var modifiedCompilationOptions = project.CompilationOptions.WithSpecificDiagnosticOptions(modifiedSpecificDiagnosticOptions);
            var processedProject = project.WithCompilationOptions(modifiedCompilationOptions);

            Compilation compilation = await processedProject.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
            CompilationWithAnalyzers compilationWithAnalyzers = compilation.WithAnalyzers(analyzers, cancellationToken: cancellationToken);

            var diagnostics = await FixAllContextHelper.GetAllDiagnosticsAsync(compilation, compilationWithAnalyzers, analyzers, project.Documents, true, cancellationToken).ConfigureAwait(false);
            return diagnostics;
        }
    }
}
