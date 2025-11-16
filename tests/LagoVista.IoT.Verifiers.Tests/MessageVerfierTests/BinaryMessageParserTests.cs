// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 2f601999d3f11e312a5cd93cfe58728633885f40933de74c2da3ec76fdd00914
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.DeviceAdmin.Interfaces.Managers;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Verifiers.Repos;
using LagoVista.IoT.Verifiers.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Tests.MessageVerfierTests
{

    [TestClass]
    public class BinaryMessageParserTests
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

        private IParserManager GetParserManager(params KeyValuePair<string, string>[] kvps)
        {
            var fakeParser = new FakeMessageParser();
            var mockParserMgr = new Moq.Mock<IParserManager>();
            mockParserMgr.Setup(prs => prs.GetMessageParser(It.IsAny<DeviceMessageDefinition>(), It.IsAny<IInstanceLogger>())).Returns(fakeParser);
            fakeParser.KVPs = kvps;
            return mockParserMgr.Object;
        }


    }
}
