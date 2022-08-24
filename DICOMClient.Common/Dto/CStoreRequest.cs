using System.Collections.Generic;

namespace DICOMClient.Common.Dto {
    public class CStoreRequest : DimseRequest {
        public List<string> Items { get; set; } = new List<string>();
    }
}
