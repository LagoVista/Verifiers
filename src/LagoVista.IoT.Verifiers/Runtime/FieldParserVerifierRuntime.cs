using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Verifiers.Models;
using System;
using System.Threading.Tasks;
using LagoVista.Core;
using System.Diagnostics;
using LagoVista.IoT.Verifiers.Utils;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Verifiers.Resources;
using LagoVista.IoT.Verifiers.Repos;
using LagoVista.Core.Models;

namespace LagoVista.IoT.Verifiers.Runtime
{
    public class FieldParserVerifierRuntime : IFieldParserVerifierRuntime
    {
        IParserManager _parserManager;
        IVerifierResultRepo _resultRepo;

        public FieldParserVerifierRuntime(IParserManager parserManager, IVerifierResultRepo resultRepo)
        {
            _parserManager = parserManager;
            _resultRepo = resultRepo;
        }

        public async Task<VerificationResults> VerifyAsync(VerificationRequest<DeviceMessageDefinitionField> request, EntityHeader requestedBy)
        {
            var sw = new Stopwatch();

            var verifier = request.Verifier as Verifier;

            var result = new VerificationResults(request.Configuration.Id);
            result.ComponentId = request.Configuration.Id;
            var start = DateTime.Now;
            result.DateStamp = start.ToJSONString();
            result.Success = true;
            result.RequestedBy = requestedBy;

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
                        var verificationResult = new VerificationResult()
                        {
                            Key = "n/a",
                            Expected = verifier.ExpectedOutput,
                            Actual = String.IsNullOrEmpty(parseResult.Result) ? "-empty-" : parseResult.Result,
                            Success = false,
                        };

                        result.Results.Add(verificationResult);

                        result.ErrorMessage.Add($"{VerifierResources.Verifier_Expected_NotMatch_Actual}, {VerifierResources.Verifier_Expected} {verificationResult.Expected}, {VerifierResources.Verifier_Actual} {verificationResult.Actual} ") ;

                        result.Success = false;
                    }
                    else
                    {
                        result.Results.Add(new VerificationResult()
                        {
                            Key = "n/a",
                            Expected = verifier.ExpectedOutput,
                            Actual = String.IsNullOrEmpty(parseResult.Result) ? "-empty-" : parseResult.Result,
                            Success = true,
                        });
                    }
                }
                else
                {
                    result.ErrorMessage.Add($"{VerifierResources.Verifier_ParserFailed}, {parseResult.FailureReason} ");
                    result.Success = false;
                }

                result.IterationCompleted++;

                if(!result.Success)
                {
                    idx = request.Iterations;
                    result.ErrorMessage.Add($"{VerifierResources.Verifier_Aborted} {result.IterationCompleted}");
                }
            }

            sw.Stop();

            result.ExecutionTimeMS = sw.Elapsed.TotalMilliseconds;

            await _resultRepo.AddResultAsync(result);

            return result;
        }
    }
}
