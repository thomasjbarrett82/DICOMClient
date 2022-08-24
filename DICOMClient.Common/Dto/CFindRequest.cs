using FellowOakDicom;
using DICOMClient.Common.Constant;
using System.Collections.Generic;

namespace DICOMClient.Common.Dto {
    public class CFindRequest : DimseRequest {
        public string QueryLevel { get; set; } = "";
        public Dictionary<DicomTag, string> Parameters { get; set; } = new Dictionary<DicomTag, string>();
    }
}
