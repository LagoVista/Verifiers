using LagoVista.IoT.Runtime.Core.Module;
using System;
using LagoVista.Core;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using System.Threading.Tasks;
using System.Diagnostics;
using LagoVista.IoT.Verifiers.Models;
using LagoVista.IoT.Verifiers.Utils;

namespace LagoVista.IoT.Verifiers.Runtime
{
    public class MessageParserVerifierRuntime : IMessageParserVerifierRuntime
    {
        IParserManager _parserManager;

        public MessageParserVerifierRuntime(IParserManager parserManager)
        {
            _parserManager = parserManager;
        }


        public Task<VerificationResult> VerifyAsync(VerificationRequest<DeviceMessageDefinition> request)
        {
            var sw = new Stopwatch();

            var verifier = request.Verifier as Verifier;

            var result = new VerificationResult(request.Configuration.Id);
            result.ComponentId = request.Configuration.Id;
            var start = DateTime.Now;
            result.DateStamp = start.ToJSONString();
            result.Success = true;

            var logger = new VerifierLogger();

            var parser = _parserManager.GetMessageParser(request.Configuration, logger);

            sw.Start();

            for (var idx = 0; idx < request.Iterations; ++idx)
            {
                var pem = new IoT.Runtime.Core.Models.PEM.PipelineExectionMessage();
                pem.PayloadType = verifier.InputType.Id == Verifier.InputType_Text ? IoT.Runtime.Core.Models.PEM.MessagePayloadTypes.Text : IoT.Runtime.Core.Models.PEM.MessagePayloadTypes.Binary;
                if (pem.PayloadType == IoT.Runtime.Core.Models.PEM.MessagePayloadTypes.Binary)
                {
                    pem.BinaryPayload = verifier.GetBinaryPayload();
                }
                else
                {
                    pem.TextPayload = verifier.Input;
                }

                var parseResult = parser.Parse(pem, request.Configuration);
                if (parseResult.Successful)
                {
                    foreach (var item in verifier.ExpectedOutputs)
                    {
                        if (pem.Envelope.Values.ContainsKey(item.Key))
                        {
                            if(pem.Envelope.Values[item.Key].Value == item.Value)
                            {
                                result.ErrorMessage = $"Expected [{verifier.ExpectedOutput}], Received [{item.Value}]";
                            }
                            else
                            {
                                result.ErrorMessage = $"Expected [{verifier.ExpectedOutput}], Received [-empty-]";
                            }
                        }
                        else
                        {
                            result.Success = false;
                        }
                    }
                }
                else
                {
                    result.Success = false;
                }

                result.Iterations++;
            }

            sw.Stop();

            result.ExecutionTimeMS = sw.Elapsed.TotalMilliseconds;

            return Task.FromResult(result);
        }
    }
}
