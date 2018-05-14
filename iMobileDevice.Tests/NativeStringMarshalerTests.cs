using iMobileDevice.Plist;
using System;
using System.Diagnostics;
using Xunit;

namespace iMobileDevice.Tests
{
    public class NativeStringMarshalerTests
    {
        [Fact]
        public void NativeToManagedTest()
        {
            NativeLibraries.Load();

            // An easy way to invoke the marshaler is through the use of
            // plist_new_string, which creates a new object with a string value,
            // and plist_get_string_val, which gets that object's value.
            var plist = new PlistApi(new LibiMobileDevice());

            /* Allocate a value worth 10 MB */
            string input = new string('x', 10 * 1024 * 1024);

            var handle = plist.plist_new_string(input);

            GC.Collect();
            var p = Process.GetCurrentProcess();
            var initialMemory = p.PrivateMemorySize64;

            for (int i = 0; i < 75; i++)
            {
                string output;

                // Every call will allocate a new string. If it's not properly cleaned up
                // by our marshaler, we will leak 10 MB in each call, or 750 MB in total.
                plist.plist_get_string_val(handle, out output);
                Assert.Equal(input, output);
            }

            handle.Dispose();
            GC.Collect();
            p.Refresh();

            var currentMemory = p.PrivateMemorySize64;
            var delta = currentMemory - initialMemory;

            // If more than 10 MB was leaked, set off the alarm bells
            // You can verify this works by commenting out NativeStringMarshaler.CleanUpNativeData
            Assert.True(delta < 10 * 1024 * 1024 * 8 /* 10 MB */, "Memory was leaked");
        }
    }
}
