using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace BitStream.Tests
{
    [TestClass]
    public class Write
    {
        [TestMethod]
        public void PartialBytes()
        {
            var solution = new byte[] { 219, 182, 109, 219, 182, 109 };

            var memStream = new MemoryStream();
            var bitStream = new BitStream(memStream);

            for (var i = 0; i < 16; ++i)
                bitStream.WriteBits(3, (BitNum)3);

            var actual = memStream.GetBuffer().Take(solution.Length).ToArray();
            for (var i = 0; i < solution.Length; ++i)
                Assert.AreEqual(solution[i], actual[i]);
        }

        [TestMethod]
        public void PartialFullBytes()
        {
            var solution = new byte[] { 51, 51, 51, 51 };

            var memStream = new MemoryStream();
            var bitStream = new BitStream(memStream);

            for (var i = 0; i < 8; ++i)
                bitStream.WriteBits(3, (BitNum)4);

            var actual = memStream.GetBuffer().Take(solution.Length).ToArray();
            for (var i = 0; i < solution.Length; ++i)
                Assert.AreEqual(solution[i], actual[i]);
        }
    }
}