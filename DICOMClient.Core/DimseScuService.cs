using FellowOakDicom;
using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using DICOMClient.Common.Core;
using DICOMClient.Common.Dto;
using Serilog;
using System;

namespace DICOMClient.Core {
    public class DimseScuService : IDimseScuService {
        public async void OnCEchoRequest(CEchoRequest request) {
            try {
                var client = DicomClientFactory.Create(
                    request.HostAddress,
                    request.HostPort,
                    request.UseTls,
                    request.CallingAETitle,
                    request.CalledAETitle
                );
                client.NegotiateAsyncOps();
                await client.AddRequestAsync(new DicomCEchoRequest {
                    OnResponseReceived = (req, resp) => {
                        Log.Information($"C-Echo Response from {request.CalledAETitle}: {resp.Status}");
                    }
                });
                await client.SendAsync();
            }
            catch (Exception ex) {
                Log.Error($"{ex.InnerException} {ex.StackTrace}");
            }
        }

        public async void OnCFindRequest(CFindRequest request) {
            try {
                var client = DicomClientFactory.Create(
                    request.HostAddress,
                    request.HostPort,
                    request.UseTls,
                    request.CallingAETitle,
                    request.CalledAETitle
                );
                var level = (DicomQueryRetrieveLevel) Enum.Parse(typeof(DicomQueryRetrieveLevel), request.QueryLevel);
                DicomCFindRequest cfind = new DicomCFindRequest(level);
                cfind.Dataset.AddOrUpdate(new DicomTag(0x8, 0x5), "ISO_IR 100"); // encoding
                foreach (var param in request.Parameters) {
                    cfind.Dataset.AddOrUpdate(param.Key, param.Value);
                }
                cfind.OnResponseReceived = (req, resp) => {
                    Log.Information($"C-Find Response from {request.CalledAETitle}: Status: {resp.Status}, Completed: {resp.Completed}, Completed: {resp.Completed}, Completed: {resp.Completed}");
                    if (resp.Dataset != null) {
                        foreach (var param in request.Parameters) {
                            resp.Dataset.TryGetString(param.Key, out string tag);
                            Log.Information($"{param.Key.DictionaryEntry.Name}: {tag ?? string.Empty}");
                        }
                    }
                    else {
                        Log.Warning("No dataset returned");
                    }
                };
                client.NegotiateAsyncOps();
                await client.AddRequestAsync(cfind);
                await client.SendAsync();
            }
            catch (Exception ex) {
                Log.Error($"{ex.InnerException} {ex.StackTrace}");
            }
        }

        public async void OnCStoreRequest(CStoreRequest request) {
            try {
                var client = DicomClientFactory.Create(
                    request.HostAddress,
                    request.HostPort,
                    request.UseTls,
                    request.CallingAETitle,
                    request.CalledAETitle
                );
                for (int i = 0; i < request.Items.Count; i++) {
                    var current = i + 1;
                    await client.AddRequestAsync(new DicomCStoreRequest(request.Items[i]) {
                        OnResponseReceived = (req, resp) => {
                            Log.Information($"{current}/{request.Items.Count} C-Store Response from {request.CalledAETitle} for {req.SOPInstanceUID}: {resp.Status}");
                        }
                    });
                }
                client.NegotiateAsyncOps();
                await client.SendAsync();
            }
            catch (Exception ex) {
                Log.Error($"{ex.InnerException} {ex.StackTrace}");
            }
        }
    }
}
