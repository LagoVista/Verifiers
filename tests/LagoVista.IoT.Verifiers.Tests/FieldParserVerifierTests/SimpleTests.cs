using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.DeviceAdmin.Interfaces.Managers;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Verifiers.Repos;
using LagoVista.IoT.Verifiers.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Tests.FieldParserVerifierTests
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

            foreach (var err in resultSet.ErrorMessages)
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

        private IParserManager GetParserManager(string result)
        {
            var fakeParser = new Moq.Mock<IMessageFieldParser>();
            fakeParser.Setup(prs => prs.Parse(It.IsAny<PipelineExecutionMessage>())).Returns(new IoT.Runtime.Core.Models.Messaging.MessageFieldParserResult()
            {
                Result = result,
                Success = true
               
            });

            var mockParserMgr = new Moq.Mock<IParserManager>();
            mockParserMgr.Setup(prs => prs.GetFieldMessageParser(It.IsAny<DeviceMessageDefinitionField>(), It.IsAny<IInstanceLogger>())).Returns(fakeParser.Object);
            return mockParserMgr.Object;
        }

        [TestMethod]
        public async Task Verfier_Field_Simple_Valid()
        {
            var parserMgr = GetParserManager("one");

            var config = new MessageAttributeParser();

            var verifier = new Verifier();
            verifier.VerifierType = EntityHeader<VerifierTypes>.Create(VerifierTypes.MessageFieldParser);
            verifier.ShouldSucceed = true;
            verifier.InputType = EntityHeader<InputTypes>.Create(InputTypes.Text);
            verifier.Input = "abc123";
            verifier.ExpectedOutput = "one";

            var verifiers = new MessageAttributeParserVerifierRuntime(parserMgr, _resultRepo.Object, _deviceAdminManager.Object);
            var result = await verifiers.VerifyAsync(new IoT.Runtime.Core.Models.Verifiers.VerificationRequest<MessageAttributeParser>()
            {
                Verifier = verifier,
                Configuration = config
            }, null, null);

            WriteResults(result);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.IterationsCompleted);
        }


        [TestMethod]
        public async Task Verfier_Field_Simple_Invalid()
        {
            var parserMgr = GetParserManager("two");

            var config = new MessageAttributeParser();

            var verifier = new Verifier();
            verifier.VerifierType = EntityHeader<VerifierTypes>.Create(VerifierTypes.MessageFieldParser);
            verifier.ShouldSucceed = true;
            verifier.InputType = EntityHeader<InputTypes>.Create(InputTypes.Text);
            verifier.Input = "abc123";
            verifier.ExpectedOutput = "one";

            var verifiers = new MessageAttributeParserVerifierRuntime(parserMgr, _resultRepo.Object, _deviceAdminManager.Object);
            var result = await verifiers.VerifyAsync(new IoT.Runtime.Core.Models.Verifiers.VerificationRequest<MessageAttributeParser>()
            {
                Verifier = verifier,
                Configuration = config
            }, null, null);

            WriteResults(result);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.IterationsCompleted);
        }


        [TestMethod]
        public async Task Verfier_Field_XML_Simple_Valid()
        {
            var parserMgr = GetParserManager("two");

            var config = new MessageAttributeParser()
            {
                ContentType = EntityHeader<MessageContentTypes>.Create(MessageContentTypes.XML),
                XPath = "//Event/control"

            };

            var verifier = new Verifier();
            verifier.VerifierType = EntityHeader<VerifierTypes>.Create(VerifierTypes.MessageFieldParser);
            verifier.ShouldSucceed = true;
            verifier.InputType = EntityHeader<InputTypes>.Create(InputTypes.Text);
            verifier.Input = "abc123";
            verifier.ExpectedOutput = "one";

            var verifiers = new MessageAttributeParserVerifierRuntime(parserMgr, _resultRepo.Object, _deviceAdminManager.Object);
            var result = await verifiers.VerifyAsync(new IoT.Runtime.Core.Models.Verifiers.VerificationRequest<MessageAttributeParser>()
            {
                Verifier = verifier,
                Configuration = config
            }, null, null);

            WriteResults(result);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(1, result.IterationsCompleted);
        }



    }
}
