namespace iMobileDevice.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Afc;

    [TestClass]
    public class HandleTests
    {
        // Because handles are generate, we only test one class of handles.

        [TestMethod]
        public void ZeroTest()
        {
            var handle = AfcClientHandle.Zero;
            Assert.IsTrue(handle.IsInvalid);
            Assert.IsFalse(handle.IsClosed);
        }

        [TestMethod]
        public void EqualsTest()
        {
            AfcClientHandle handle = AfcClientHandle.DangerousCreate(new IntPtr(42));
            AfcClientHandle handle2 = AfcClientHandle.DangerousCreate(new IntPtr(42));
            AfcClientHandle zero = AfcClientHandle.Zero;

            Assert.IsTrue(handle.Equals(handle));
            Assert.IsTrue(handle.Equals(handle2));
            Assert.IsTrue(handle2.Equals(handle));

            Assert.IsFalse(handle.Equals(zero));
            Assert.IsFalse(zero.Equals(handle));

            Assert.IsFalse(handle.Equals(42));
        }
    }
}
