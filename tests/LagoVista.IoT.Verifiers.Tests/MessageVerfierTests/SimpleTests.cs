using System;
using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Verifiers.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using LagoVista.IoT.Verifiers.Repos;
using System.Collections.Generic;
using LagoVista.IoT.Verifiers.Tests.Helpers;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.DeviceAdmin.Interfaces.Managers;

namespace LagoVista.IoT.Verifiers.Tests.MessageVerfierTests
{
    [TestClass]
    public class SimpleTests
    {
        Mock<IVerifierResultRepo> _resultRepo;
        Mock<IDeviceAdminManager> _deviceAdminManager;

        private void WriteResults(VerificationResults resultSet)
        {
            Console.WriteLine("Succcess    : " + resultSet.Success);
            Console.WriteLine("Iterations  : " + resultSet.IterationsCompleted);
            Console.WriteLine("ExecutionMS : " + resultSet.ExecutionTimeMS);

            foreach (var result in resultSet.Results)
            {
                Console.WriteLine($"Key:      {result.Key}");
                Console.WriteLine($" - Success:  {result.Success}");
                Console.WriteLine($" - Expected: {result.Expected}");
                Console.WriteLine($" - Actual: {result.Actual}");
                Console.WriteLine("  ");
            }

            foreach(var err in resultSet.ErrorMessages)
            {
                Console.WriteLine(err);
            }
        }


        [TestInitialize]
        public void Init()
        {
            _resultRepo = new Mock<IVerifierResultRepo>();
            _deviceAdminManager = new Mock<IDeviceAdminManager>();
        }
        
        private IParserManager GetParserManager(params KeyValuePair<string, string>[] kvps)
        {
            var fakeParser = new FakeMessageParser();
            var mockParserMgr = new Moq.Mock<IParserManager>();
            mockParserMgr.Setup(prs => prs.GetMessageParser(It.IsAny<DeviceMessageDefinition>(), It.IsAny<IInstanceLogger>())).Returns(fakeParser);
            fakeParser.KVPs = kvps;
            return mockParserMgr.Object;
        }

        [TestMethod]
        public async Task Verfier_Message_SimpleTest_Valid()
        {
            var msgDefinition = new DeviceMessageDefinition();

            var parserMgr = GetParserManager(new KeyValuePair<string, string>("key1", "5"), new KeyValuePair<string, string>("key2", "value1"));

            var verifier = new Verifier();
            verifier.Headers.Add(new Header() { Name = "field1", Value = "value1" });
            verifier.VerifierType = EntityHeader<VerifierTypes>.Create(VerifierTypes.MessageFieldParser);
            verifier.ShouldSucceed = true;
            verifier.InputType = EntityHeader<InputTypes>.Create(InputTypes.Binary);
            verifier.Input = "03 32 05";
            verifier.ExpectedOutputs.Add(new ExpectedValue() { Key = "key1", Value = "5" });
            verifier.ExpectedOutputs.Add(new ExpectedValue() { Key = "key2", Value = "value1" });

            var verifiers = new MessageParserVerifierRuntime(parserMgr, _resultRepo.Object, _deviceAdminManager.Object);
            var result = await verifiers.VerifyAsync(new IoT.Runtime.Core.Models.Verifiers.VerificationRequest<DeviceMessageDefinition>()
            {
                Verifier = verifier,
                Configuration = msgDefinition
            }, null, null);

            WriteResults(result);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.IterationsCompleted);
        }

        [TestMethod]
        public async Task Verifier_Message_SimpleTest_Invalid()
        {
            var msgDefinition = new DeviceMessageDefinition();

            var verifier = new Verifier
            {
                ShouldSucceed = true,
                VerifierType = EntityHeader<VerifierTypes>.Create(VerifierTypes.MessageFieldParser),
                InputType = EntityHeader<InputTypes>.Create(InputTypes.Binary),
                Input = "03 32 05"
            };
            verifier.ExpectedOutputs.Add(new ExpectedValue() { Key = "key1", Value = "8" });
            verifier.ExpectedOutputs.Add(new ExpectedValue() { Key = "key2", Value = "value1" });

            var parserMgr = GetParserManager(new KeyValuePair<string, string>("key1", "5"), new KeyValuePair<string, string>("key2", "value1"));
            var verifiers = new MessageParserVerifierRuntime(parserMgr, _resultRepo.Object, _deviceAdminManager.Object);
            var result = await verifiers.VerifyAsync(new IoT.Runtime.Core.Models.Verifiers.VerificationRequest<DeviceMessageDefinition>()
            {
                Verifier = verifier,
                Configuration = msgDefinition
            }, null, null);

            WriteResults(result);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.IterationsCompleted);
        }
    }
}
