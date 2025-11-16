// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 96b27a66fad544c58dd1a6ead61f6dfc5a1e1a6537a9f4c311f8a00fd9cd844a
// IndexVersion: 2
// --- END CODE INDEX META ---
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
