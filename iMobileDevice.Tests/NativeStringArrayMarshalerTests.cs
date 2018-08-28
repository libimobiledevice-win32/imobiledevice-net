using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace iMobileDevice.Tests
{
    public class NativeStringArrayMarshalerTests
    {
        [Fact (Skip = "This test should run in its own context")]
        public void TestRoundTrip()
        {
            NativeLibraries.Load();

            // Create a string array worth +/- 10 MB of memory
            var values = new List<string>();
            values.Add(new string('0', 1024 * 1024));
            values.Add(new string('1', 1024 * 1024));
            values.Add(new string('2', 1024 * 1024));
            values.Add(new string('3', 1024 * 1024));
            values.Add(new string('4', 1024 * 1024));
            values.Add(new string('5', 1024 * 1024));
            values.Add(new string('6', 1024 * 1024));
            values.Add(new string('7', 1024 * 1024));
            values.Add(new string('8', 1024 * 1024));
            values.Add(new string('9', 1024 * 1024));

            // And, as the icing on the cake, an UTF-8 string.
            values.Add("Bart’s iPhone");

            var readonlyValues = new ReadOnlyCollection<string>(values);

            GC.Collect();
            var p = Process.GetCurrentProcess();
            var initialMemory = p.PrivateMemorySize64;

            for (int i = 0; i < 75; i++)
            {
                NativeStringArrayMarshaler marshaler = new NativeStringArrayMarshaler();
                var pointer = marshaler.MarshalManagedToNative(readonlyValues);
                marshaler = null;

                GC.Collect();

                marshaler = new NativeStringArrayMarshaler();
                var roundTrip = (ReadOnlyCollection<string>)marshaler.MarshalNativeToManaged(pointer);
                Assert.Equal(readonlyValues, roundTrip);
                marshaler.CleanUpNativeData(pointer);
            }

            GC.Collect();
            p.Refresh();

            var currentMemory = p.PrivateMemorySize64;
            var delta = currentMemory - initialMemory;

            // If more than 10 MB was leaked, set off the alarm bells
            // You can verify this works by commenting out NativeStringMarshaler.CleanUpNativeData
            Assert.True(delta <= 10 * 1024 * 1024 * 8 /* 10 MB */, "Memory was leaked");
        }
    }
}
