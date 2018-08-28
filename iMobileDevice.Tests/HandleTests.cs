namespace iMobileDevice.Tests
{
    using System;
    using Afc;
    using Xunit;

    public class HandleTests
    {
        // Because handles are generate, we only test one class of handles.

        [Fact]
        public void ZeroTest()
        {
            var handle = AfcClientHandle.Zero;
            Assert.True(handle.IsInvalid);
            Assert.False(handle.IsClosed);
        }

        [Fact]
        public void EqualsTest()
        {
            AfcClientHandle handle = AfcClientHandle.DangerousCreate(new IntPtr(42), ownsHandle: false);
            AfcClientHandle handle2 = AfcClientHandle.DangerousCreate(new IntPtr(42), ownsHandle: false);
            AfcClientHandle zero = AfcClientHandle.Zero;

            Assert.True(handle.Equals(handle));
            Assert.True(handle.Equals(handle2));
            Assert.True(handle2.Equals(handle));

            Assert.False(handle.Equals(zero));
            Assert.False(zero.Equals(handle));

            Assert.False(handle.Equals(42));
        }
    }
}
