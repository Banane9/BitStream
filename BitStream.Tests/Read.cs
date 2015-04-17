using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace BitStream.Tests
{
    [TestClass]
    public class Read
    {
        [TestMethod]
        public void PartialBytes()
        {
            var memStream = new MemoryStream(new byte[] { 219, 182, 109, 219, 182, 109 });
            var bitStream = new BitStream(memStream);

            for (var i = 0; i < 16; ++i)
            {
                byte value;
                bitStream.ReadBits(out value, (BitNum)3);
                Assert.AreEqual(3, value);
            }
        }

        [TestMethod]
        public void PartialFullBytes()
        {
            var memStream = new MemoryStream(new byte[] { 51, 51, 51, 51 });
            var bitStream = new BitStream(memStream);

            for (var i = 0; i < 8; ++i)
            {
                byte value;
                bitStream.ReadBits(out value, (BitNum)4);
                Assert.AreEqual(3, value);
            }
        }
    }
}