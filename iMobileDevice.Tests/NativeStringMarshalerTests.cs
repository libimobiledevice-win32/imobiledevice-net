using iMobileDevice.Plist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMobileDevice.Tests
{
    [TestClass]
    public class NativeStringMarshalerTests
    {
        [TestMethod]
        public void NativeToManagedTest()
        {
            NativeLibraries.Load();

            // An easy way to invoke the marshaler is through the use of
            // plist_new_string, which creates a new object with a string value,
            // and plist_get_string_val, which gets that object's value.
            var plist = new PlistApi();

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
                Assert.AreEqual(input, output);
            }

            handle.Dispose();
            GC.Collect();
            p.Refresh();

            var currentMemory = p.PrivateMemorySize64;
            var delta = currentMemory - initialMemory;

            // If more than 10 MB was leaked, set off the alarm bells
            // You can verify this works by commenting out NativeStringMarshaler.CleanUpNativeData
            if (delta > 10 * 1024 * 1024 * 8 /* 10 MB */)
            {
                Assert.Fail("Memory was leaked");
            }
        }
    }
}
