using DICOMClient.Common.Dto;

namespace DICOMClient.Common.Core {
    public interface IDimseScuService {
        void OnCEchoRequest(CEchoRequest request);
        void OnCFindRequest(CFindRequest request);
        void OnCStoreRequest(CStoreRequest request);
    }
}
