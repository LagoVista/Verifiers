using System;
using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Verifiers.Models;
using LagoVista.IoT.Verifiers.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using LagoVista.IoT.Verifiers.Repos;
using System.Collections.Generic;
using LagoVista.IoT.Verifiers.Tests.Helpers;

namespace LagoVista.IoT.Verifiers.Tests.MessageVerfierTests
{
    [TestClass]
    public class BinaryTests
    {
        Mock<IVerifierResultRepo> _resultRepo;

        [TestInitialize]
        public void Init()
        {
            _resultRepo = new Mock<IVerifierResultRepo>();
        }
        
        private IParserManager GetParserManager(params KeyValuePair<string, string>[] kvps)
        {
            var fakeParser = new FakeParser();
            var mockParserMgr = new Moq.Mock<IParserManager>();
            mockParserMgr.Setup(prs => prs.GetMessageParser(It.IsAny<DeviceMessageDefinition>(), It.IsAny<ILogger>())).Returns(fakeParser);
            fakeParser.KVPs = kvps;
            return mockParserMgr.Object;
        }

        [TestMethod]
        public async Task Verfier_BinaryTest_Valid()
        {
            var msgDefinition = new DeviceMessageDefinition();

            var parserMgr = GetParserManager(new KeyValuePair<string, string>("key1", "5"), new KeyValuePair<string, string>("key2", "value1"));

            var verifier = new Verifier();
            verifier.Headers.Add(new Models.Header() { Name = "field1", Value = "value1" });
            verifier.VerifierType = EntityHeader<VerifierTypes>.Create(VerifierTypes.MessageFieldParser);
            verifier.ShouldSucceed = true;
            verifier.InputType = EntityHeader<InputTypes>.Create(InputTypes.Binary);
            verifier.Input = "03 32 05";
            verifier.ExpectedOutputs.Add(new ExpectedValue() { Key = "key1", Value = "5" });
            verifier.ExpectedOutputs.Add(new ExpectedValue() { Key = "key2", Value = "value1" });


            var verifiers = new MessageParserVerifierRuntime(parserMgr, _resultRepo.Object);
            var result = await verifiers.VerifyAsync(new IoT.Runtime.Core.Models.Verifiers.VerificationRequest<DeviceMessageDefinition>()
            {
                Verifier = verifier,
                Configuration = msgDefinition
            }, null);

            Console.WriteLine(result.ErrorMessage);
            Console.WriteLine(result.ExecutionTimeMS);

            Assert.IsTrue(result.Success);
        }


        [TestMethod]
        public async Task Verifier_BinaryTest_Invalid()
        {
            var msgDefinition = new DeviceMessageDefinition();
            
            var verifier = new Verifier();
            verifier.ShouldSucceed = true;
            verifier.VerifierType = EntityHeader<VerifierTypes>.Create(VerifierTypes.MessageFieldParser);
            verifier.InputType = EntityHeader<InputTypes>.Create(InputTypes.Binary);
            verifier.Input = "03 32 05";
            verifier.ExpectedOutputs.Add(new ExpectedValue() { Key = "key1", Value = "8" });
            verifier.ExpectedOutputs.Add(new ExpectedValue() { Key = "key2", Value = "value1" });

            var parserMgr = GetParserManager(new KeyValuePair<string, string>("key1", "5"), new KeyValuePair<string, string>("key2", "value1"));
            var verifiers = new MessageParserVerifierRuntime(parserMgr, _resultRepo.Object);
            var result = await verifiers.VerifyAsync(new IoT.Runtime.Core.Models.Verifiers.VerificationRequest<DeviceMessageDefinition>()
            {
                Verifier = verifier,
                Configuration = msgDefinition
            });

            Console.WriteLine(result.ErrorMessage);
            Console.WriteLine(result.ExecutionTimeMS);

            Assert.IsFalse(result.Success);
        }


    }
}
