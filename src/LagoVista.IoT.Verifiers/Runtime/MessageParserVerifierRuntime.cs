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
using LagoVista.IoT.Runtime.Core.Models.PEM;
using LagoVista.IoT.DeviceAdmin.Interfaces.Managers;

namespace LagoVista.IoT.Verifiers.Runtime
{
    public class MessageParserVerifierRuntime : IMessageParserVerifierRuntime
    {
        IParserManager _parserManager;
        IVerifierResultRepo _resultRepo;
        IDeviceAdminManager _deviceAdminManager;

        public MessageParserVerifierRuntime(IParserManager parserManager, IVerifierResultRepo resultRepo, IDeviceAdminManager deviceAdminManager)
        {
            _parserManager = parserManager;
            _resultRepo = resultRepo;
            _deviceAdminManager = deviceAdminManager;
        }

        public async Task<VerificationResults> VerifyAsync(VerificationRequest<DeviceMessageDefinition> request, EntityHeader org, EntityHeader user)
        {
            var sw = new Stopwatch();

            var verifier = request.Verifier as Verifier;

            var result = new VerificationResults(new EntityHeader() { Text = request.Configuration.Name, Id = request.Configuration.Id }, VerifierTypes.MessageParser);

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

            if(verifier.ExpectedOutputs == null ||verifier.ExpectedOutputs.Count == 0)
            {
                result.ErrorMessages.Add(VerifierResources.Verifier_MissingInput);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }
          
            if(EntityHeader.IsNullOrEmpty(verifier.InputType))
            {
                result.ErrorMessages.Add(VerifierResources.Verifier_MissingInputType);
                result.Success = false;
                await _resultRepo.AddResultAsync(result);
                return result;
            }

            if(request.Iterations == 0)
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

            foreach(var fld in request.Configuration.Fields)
            {
                if(fld.StorageType.Value == DeviceAdmin.Models.ParameterTypes.State)
                {
                    fld.StateSet.Value = await _deviceAdminManager.GetStateSetAsync(fld.StateSet.Id, org, user);
                }
                else if(fld.StorageType.Value == DeviceAdmin.Models.ParameterTypes.ValueWithUnit)
                {
                    fld.UnitSet.Value = await _deviceAdminManager.GetAttributeUnitSetAsync(fld.UnitSet.Id, org, user);
                }
            }

            var parser = _parserManager.GetMessageParser(request.Configuration, logger);

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

                foreach (var header in verifier.Headers)
                {
                    pem.Envelope.Headers.Add(header.Name, header.Value);
                }

                pem.Envelope.Path = verifier.PathAndQueryString;
                pem.Envelope.Topic = verifier.Topic;

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
                                result.ErrorMessages.Add($"{VerifierResources.Verifier_Expected_NotMatch_Actual}. {VerifierResources.Verifier_Field}: {item.Key},  {VerifierResources.Verifier_Expected}={verificationResult.Expected}, {VerifierResources.Verifier_Actual}={verificationResult.Actual} ");
                                result.Success = false;
                            }
                        }
                        else
                        {
                            result.ErrorMessages.Add($"{VerifierResources.Verifier_ResultDoesNotContainKey}, {item.Key}");
                            result.Success = false;
                        }
                    }
                }
                else
                {
                    result.Success = false;
                }

                result.IterationsCompleted++;

                if (!result.Success)
                {
                    idx = request.Iterations;
                    result.ErrorMessages.Add($"{VerifierResources.Verifier_Aborted}; {VerifierResources.Verifier_IterationsCompleted} ({result.IterationsCompleted})");
                }
            }

            sw.Stop();

            result.ExecutionTimeMS = Math.Round(sw.Elapsed.TotalMilliseconds, 3);

            await _resultRepo.AddResultAsync(result);
            return result;
        }
    }
}
