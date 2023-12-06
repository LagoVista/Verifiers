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
using LagoVista.IoT.DeviceAdmin.Interfaces.Managers;

namespace LagoVista.IoT.Verifiers.Runtime
{
    public class MessageAttributeParserVerifierRuntime : IMessageAttributeParserVerifierRuntime
    {
        IParserManager _parserManager;
        IVerifierResultRepo _resultRepo;
        IDeviceAdminManager _deviceAdminManager;

        public MessageAttributeParserVerifierRuntime(IParserManager parserManager, IVerifierResultRepo resultRepo, IDeviceAdminManager deviceAdminManager)
        {
            _parserManager = parserManager;
            _resultRepo = resultRepo;
            _deviceAdminManager = deviceAdminManager;
        }

        public async Task<VerificationResults> VerifyAsync(VerificationRequest<MessageAttributeParser> request, EntityHeader org, EntityHeader user)
        {
            var verifier = request.Verifier as Verifier;

            var result = new VerificationResults(new EntityHeader() { Text = request.Configuration.Name, Id = request.Configuration.Id }, VerifierTypes.MessageFieldParser);

            if (String.IsNullOrEmpty(verifier.Input) &&
                String.IsNullOrEmpty(verifier.Topic) &&
                String.IsNullOrEmpty(verifier.PathAndQueryString) &&
                verifier.Headers.Count == 0)
            {
                result.ErrorMessages.Add(VerifierResources.Verifier_MissingInput);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }

            if (EntityHeader.IsNullOrEmpty(verifier.InputType))
            {
                result.ErrorMessages.Add(VerifierResources.Verifier_MissingInputType);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }

            if(String.IsNullOrEmpty(verifier.ExpectedOutput))
            {
                result.ErrorMessages.Add(VerifierResources.Verifier_MissingExpectedOutput);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }

            if (request.Iterations == 0)
            {
                result.ErrorMessages.Add(VerifierResources.Verifier_IterationCountZero);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }
            
            var start = DateTime.Now;
            result.DateStamp = start.ToJSONString();
            result.Success = true;
            result.RequestedBy = user;


            /* TODO: Need to think this through we are using the same parser we are for instances, do we care about logging this? */
            var logger = new VerifierLogger(null, null, null, null);

            var parser = _parserManager.GetFieldMessageParser(request.Configuration, logger);

            var sw = new Stopwatch();
            sw.Start();

            for (var idx = 0; idx < request.Iterations; ++idx)
            {
                var pem = new IoT.Runtime.Core.Models.PEM.PipelineExecutionMessage();
                pem.PayloadType = verifier.InputType.Value == InputTypes.Text ? IoT.Runtime.Core.Models.PEM.MessagePayloadTypes.Text : IoT.Runtime.Core.Models.PEM.MessagePayloadTypes.Binary;
                if (pem.PayloadType == IoT.Runtime.Core.Models.PEM.MessagePayloadTypes.Binary)
                {
                    pem.BinaryPayload = verifier.GetBinaryPayload();
                    if (pem.BinaryPayload == null || pem.BinaryPayload.Length == 0)
                    {
                        result.ErrorMessages.Add(VerifierResources.Verifier_MissingInput);
                        result.Success = false;
                        await _resultRepo.AddResultAsync(result);
                        return result;
                    }
                }
                else
                {
                    pem.TextPayload = verifier.Input;
                }

                pem.Envelope.Topic = verifier.Topic;
                pem.Envelope.Path = verifier.PathAndQueryString;

                foreach(var hdr in verifier.Headers)
                {
                    pem.Envelope.Headers.Add(hdr.Name, hdr.Value);
                }

                var parseResult = parser.Parse(pem);
                if (parseResult.Success)
                {
                    if (parseResult.Result != verifier.ExpectedOutput)
                    {
                        var verificationResult = new VerificationResult()
                        {
                            Field = VerifierResources.NotApplicable,
                            Key = VerifierResources.NA,
                            Expected = verifier.ExpectedOutput,
                            Actual = String.IsNullOrEmpty(parseResult.Result) ? VerifierResources.Empty : parseResult.Result,
                            Success = false,
                        };

                        result.Results.Add(verificationResult);
                        result.ErrorMessages.Add($"{VerifierResources.Verifier_Expected_NotMatch_Actual}. {VerifierResources.Verifier_Expected}={verificationResult.Expected}, {VerifierResources.Verifier_Actual}={verificationResult.Actual} ");

                        result.Success = false;
                    }
                    else
                    {
                        result.Results.Add(new VerificationResult()
                        {
                            Field = VerifierResources.NotApplicable,
                            Key = VerifierResources.NA,
                            Expected = verifier.ExpectedOutput,
                            Actual = String.IsNullOrEmpty(parseResult.Result) ? VerifierResources.Empty : parseResult.Result,
                            Success = true,
                        });
                    }
                }
                else
                {
                    result.ErrorMessages.Add($"{VerifierResources.Verifier_ParserFailed}, {parseResult.FailureReason} ");
                    result.Success = false;
                }

                result.IterationsCompleted++;

                if(!result.Success)
                {
                    idx = request.Iterations;
                    result.ErrorMessages.Add($"{VerifierResources.Verifier_Aborted}; {result.IterationsCompleted}");
                }
            }

            sw.Stop();

            result.ExecutionTimeMS = Math.Round(sw.Elapsed.TotalMilliseconds, 3);

            await _resultRepo.AddResultAsync(result);

            return result;
        }
    }
}
