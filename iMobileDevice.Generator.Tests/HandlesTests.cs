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
            var handles = Handles.CreateSafeHandle("TestHandle", moduleGenerator).ToArray();
            Assert.Equal(2, handles.Length);

            using (Stream stream = new MemoryStream())
            {
                moduleGenerator.WriteType(stream, handles[0], string.Empty);

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    var actual = reader.ReadToEnd();
                    var expected = File.ReadAllText("TestHandle.cs.txt");

                    Assert.Equal(expected, actual);
                }
            }

            using (Stream stream = new MemoryStream())
            {
                moduleGenerator.WriteType(stream, handles[1], string.Empty);

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    var actual = reader.ReadToEnd();
                    var expected = File.ReadAllText("TestHandleDelegateMarshaler.cs.txt");

                    Assert.Equal(expected, actual);
                }
            }
        }
    }
}
