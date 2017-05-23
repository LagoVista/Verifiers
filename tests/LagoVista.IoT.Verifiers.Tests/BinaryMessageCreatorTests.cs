using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;

namespace LagoVista.IoT.Verifiers.Tests
{
    [TestClass]
    public class BinaryMessageCreatorTests
    {
        [TestMethod]
        public void BinaryMessage_TwoCharacterBinary_CreaateFromString()
        {
            var verifier = new Verifier();
            verifier.Input = "3A 4C 54 6A 32 FF 47";

            var bytes = verifier.GetBinaryPayload();

            Assert.AreEqual(7, bytes.Length);
            Assert.AreEqual(0x3A, bytes[0]);
            Assert.AreEqual(0x4C, bytes[1]);
            Assert.AreEqual(0x54, bytes[2]);
            Assert.AreEqual(0x6A, bytes[3]);
            Assert.AreEqual(0x32, bytes[4]);
            Assert.AreEqual(0xFF, bytes[5]);
            Assert.AreEqual(0x47, bytes[6]);
        }

        [TestMethod]
        public void BinaryMessage_FourCharacterBinary_0x00_CreaateFromString()
        {
            var verifier = new Verifier();
            verifier.Input = "0x3A 0x4C 0x54 0x6A 0x32 0xFF 0x47";

            var bytes = verifier.GetBinaryPayload();

            Assert.AreEqual(7, bytes.Length);
            Assert.AreEqual(0x3A, bytes[0]);
            Assert.AreEqual(0x4C, bytes[1]);
            Assert.AreEqual(0x54, bytes[2]);
            Assert.AreEqual(0x6A, bytes[3]);
            Assert.AreEqual(0x32, bytes[4]);
            Assert.AreEqual(0xFF, bytes[5]);
            Assert.AreEqual(0x47, bytes[6]);
        }

        [TestMethod]
        public void BinaryMessage_FramingCharacters_CreaateFromString()
        {
            var verifier = new Verifier();
            verifier.Input = "[SOH] [STX] 0x54 0x6A [ETX] 0xFF [EOT]";

            var bytes = verifier.GetBinaryPayload();

            Assert.AreEqual(7, bytes.Length);
            Assert.AreEqual(0x01, bytes[0]);
            Assert.AreEqual(0x02, bytes[1]);
            Assert.AreEqual(0x54, bytes[2]);
            Assert.AreEqual(0x6A, bytes[3]);
            Assert.AreEqual(0x03, bytes[4]);
            Assert.AreEqual(0xFF, bytes[5]);
            Assert.AreEqual(0x04, bytes[6]);
        }

    }
}
