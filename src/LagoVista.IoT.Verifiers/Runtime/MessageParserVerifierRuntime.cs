using LagoVista.IoT.Runtime.Core.Module;
using System;
using LagoVista.Core;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using System.Threading.Tasks;
using System.Diagnostics;
using LagoVista.IoT.Verifiers.Utils;
using LagoVista.IoT.Verifiers.Repos;
using LagoVista.IoT.Verifiers.Resources;
using LagoVista.Core.Models;

namespace LagoVista.IoT.Verifiers.Runtime
{
    public class MessageParserVerifierRuntime : IMessageParserVerifierRuntime
    {
        IParserManager _parserManager;
        IVerifierResultRepo _resultRepo;

        public MessageParserVerifierRuntime(IParserManager parserManager, IVerifierResultRepo resultRepo)
        {
            _parserManager = parserManager;
            _resultRepo = resultRepo;
        }


        public async Task<VerificationResults> VerifyAsync(VerificationRequest<DeviceMessageDefinition> request, EntityHeader requestedBy)
        {
            var sw = new Stopwatch();

            var verifier = request.Verifier as Verifier;

            var result = new VerificationResults(request.Configuration.Id);

            if (String.IsNullOrEmpty(verifier.Input))
            {
                result.ErrorMessage.Add(VerifierResources.Verifier_MissingInput);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }

            if(verifier.ExpectedOutputs == null ||verifier.ExpectedOutputs.Count == 0)
            {
                result.ErrorMessage.Add(VerifierResources.Verifier_MissingInput);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }
          
            if(EntityHeader.IsNullOrEmpty(verifier.InputType))
            {
                result.ErrorMessage.Add(VerifierResources.Verifier_MissingInputType);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }

            if(request.Iterations == 0)
            {
                result.ErrorMessage.Add(VerifierResources.Verifier_IterationCountZero);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }

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
                    if (pem.BinaryPayload == null || pem.BinaryPayload.Length == 0)
                    {
                        result.ErrorMessage.Add(VerifierResources.Verifier_MissingInput);
                        result.Success = false;
                        await _resultRepo.AddResultAsync(result);
                        return result;
                    }
                }
                else
                {
                    pem.TextPayload = verifier.Input;
                }

                foreach (var header in verifier.Headers)
                {
                    pem.Envelope.Headers.Add(header.Name, header.Value);
                }

                pem.Envelope.Path = verifier.PathAndQueryString;

                var parseResult = parser.Parse(pem, request.Configuration);
                if (parseResult.Successful)
                {
                    foreach (var item in verifier.ExpectedOutputs)
                    {
                        var expectedValue = item.Value;
                        if (pem.Envelope.Values.ContainsKey(item.Key))
                        {
                            var actualValue = pem.Envelope.Values[item.Key].Value;

                            if (expectedValue == actualValue)
                            {
                                result.Results.Add(new VerificationResult()
                                {
                                    Key = item.Key,
                                    Expected = item.Value,
                                    Actual = String.IsNullOrEmpty(actualValue) ? "-empty-" : actualValue,
                                    Success = true,
                                });
                            }
                            else
                            {
                                var verificationResult = new VerificationResult()
                                {
                                    Key = item.Key,
                                    Expected = item.Value,
                                    Actual = String.IsNullOrEmpty(actualValue) ? "-empty-" : actualValue,
                                    Success = false,
                                };

                                result.Results.Add(verificationResult);
                                result.ErrorMessage.Add($"{VerifierResources.Verifier_Expected_NotMatch_Actual}, {VerifierResources.Verifier_Expected} {verificationResult.Expected}, {VerifierResources.Verifier_Actual} {verificationResult.Actual} ");
                                result.Success = false;
                            }
                        }
                        else
                        {
                            result.ErrorMessage.Add($"{VerifierResources.Verifier_ResultDoesNotContainKey}, {item.Key}");
                            result.Success = false;
                        }
                    }
                }
                else
                {
                    result.Success = false;
                }

                result.IterationCompleted++;

                if (!result.Success)
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
