using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.Runtime.Core.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Tests.Helpers
{

    public class FakeMessageParser : IMessageParser
    {
        public KeyValuePair<string, string>[] KVPs { get; set; }

        public InvokeResult Parse(PipelineExecutionMessage pem, DeviceMessageDefinition definition)
        {
            foreach (var kvp in KVPs)
            {
                pem.Envelope.Values.Add(kvp.Key, new MessageValue() { Value = kvp.Value });
            }
            return InvokeResult.Success;
        }
    }
}
