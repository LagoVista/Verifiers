using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Verifiers.Models;
using System;
using System.Threading.Tasks;
using LagoVista.Core;
using System.Diagnostics;
using LagoVista.IoT.Verifiers.Utils;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;

namespace LagoVista.IoT.Verifiers.Runtime
{
        public class FieldParserVerifierRuntime : IFieldParserVerifierRuntime
    {
        IParserManager _parserManager;

        public FieldParserVerifierRuntime(IParserManager parserManager)
        {
            _parserManager = parserManager;

        }

        public Task<VerificationResult> VerifyAsync(VerificationRequest<MessageFieldParserConfiguration> request)
        {
            var sw = new Stopwatch();

            var verifier = request.Verifier as Verifier;

            var result = new VerificationResult(request.Configuration.Id);
            result.ComponentId = request.Configuration.Id;
            var start = DateTime.Now;
            result.DateStamp = start.ToJSONString();
            result.Success = true;

            var logger = new VerifierLogger();

            var parser = _parserManager.GetFieldMessageParser(request.Configuration, logger);

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

                var parseResult = parser.Parse(pem);
                if (parseResult.Success)
                {

                    if (parseResult.Result != verifier.ExpectedOutput)
                    {
                        result.Success = false;
                        if (!String.IsNullOrEmpty(parseResult.Result))
                        {
                            result.ErrorMessage = $"Expected [{verifier.ExpectedOutput}], Received [{parseResult.Result}]";
                        }
                        else
                        {
                            result.ErrorMessage = $"Expected [{verifier.ExpectedOutput}], Received [-empty-]";
                        }
                    }
                    else
                    {
                        result.Success = parseResult.Success && result.Success;
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
