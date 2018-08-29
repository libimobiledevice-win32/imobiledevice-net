﻿using System;
using System.Runtime.InteropServices;

namespace iMobileDevice
{
    internal static class NativeMethods
    {
        /// <summary>
        /// The name of the <c>kernel32</c> library
        /// </summary>
        private const string Kernel32 = "kernel32";

        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="dllToLoad">
        /// <para>
        /// The name of the module. This can be either a library module (a <c>.dll</c> file) or an executable module (an <c>.exe</c> file).
        /// The name specified is the file name of the module and is not related to the name stored in the library module itself,
        /// as specified by the LIBRARY keyword in the module-definition (<c>.def</c>) file.
        /// </para>
        /// <para>
        /// If the string specifies a full path, the function searches only that path for the module.
        /// </para>
        /// <para>
        /// If the string specifies a relative path or a module name without a path, the function uses a standard search strategy
        /// to find the module; for more information, see the Remarks.
        /// </para>
        /// <para>
        /// If the function cannot find the module, the function fails. When specifying a path, be sure to use backslashes (<c>\</c>),
        /// not forward slashes (<c>/</c>). For more information about paths, see Naming a File or Directory.
        /// </para>
        /// <para>
        /// If the string specifies a module name without a path and the file name extension is omitted, the function appends the
        /// default library extension <c>.dll</c> to the module name. To prevent the function from appending <c>.dll</c> to the module name,
        /// include a trailing point character (<c>.</c>) in the module name string.
        /// </para>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the module.
        /// If the function fails, the return value is <see cref="IntPtr.Zero"/>. To get extended error information, call
        /// <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        /// <seealso href="http://msdn.microsoft.com/en-us/library/windows/desktop/ms684175(v=vs.85).aspx"/>
        [DllImport(Kernel32, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport(Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDllDirectory(string lpPathName);

        /// <summary>
        /// The function <see cref="dlopen"/> loads the dynamic library file named by the
        /// null-terminated string filename.
        /// </summary>
        /// <param name="filename">
        /// The path to the file to open.
        /// </param>
        /// <param name="flags">
        /// A value of the <see cref="DlOpenFlags"/> enumeration specifying how the
        /// dynamic library file is openend.
        /// </param>
        /// <returns>
        /// Returns an opaque "handle" for the dynamic library.
        /// </returns>
        /// <remarks>
        /// This method works on Linux only.
        /// </remarks>
        /// <seealso href="http://linux.die.net/man/3/dlopen"/>
        [DllImport("dl")]
        public static extern IntPtr dlopen(string filename, DlOpenFlags flags);

        /// <summary>
        /// The function <see cref="dlerror"/> returns a human readable string describing the most
        /// recent error that occurred from <see cref="dlopen"/>, or <see cref="dlclose"/> since
        /// the last call to <see cref="dlerror"/>. It returns <see cref="IntPtr.Zero"/> if no errors
        /// have occurred since initialization or since it was last called.
        /// </summary>
        /// <returns>
        /// A pointer to a human readable string describing the error, or <see cref="IntPtr.Zero"/>
        /// if no error occurred.
        /// </returns>
        /// <remarks>
        /// This method works on Linux only.
        /// </remarks>
        /// <seealso href="http://linux.die.net/man/3/dlopen"/>
        [DllImport("dl")]
        public static extern IntPtr dlerror();
    }
}
