using LagoVista.IoT.Verifiers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Tests.Misc
{
    [TestClass]
    public class ParseBinaryPayloadTests
    {
        [TestMethod]
        public void TestParsingOfMessage()
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
    }
}
