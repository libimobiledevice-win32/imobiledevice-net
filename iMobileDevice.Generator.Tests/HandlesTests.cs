using System.IO;
using System.Linq;
using Xunit;

namespace iMobileDevice.Generator.Tests
{
    /// <summary>
    /// Tests the <see cref="Handles"/> class.
    /// </summary>
    public class HandlesTests
    {
        [Fact]
        public void CreateSafeHandleTest()
        {
            var moduleGenerator = new ModuleGenerator();
            moduleGenerator.InputFile = "test.h";
            var handle = Handles.CreateSafeHandle("TestHandle", moduleGenerator).First();

            using (Stream stream = new MemoryStream())
            {
                moduleGenerator.WriteType(stream, handle, string.Empty);

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    var actual = reader.ReadToEnd();
                    var expected = File.ReadAllText("TestHandle.cs.txt");

                    Assert.Equal(expected, actual);
                }
            }
        }
    }
}
