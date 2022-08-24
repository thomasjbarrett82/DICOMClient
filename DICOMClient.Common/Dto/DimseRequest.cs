namespace DICOMClient.Common.Dto {
    public abstract class DimseRequest {
        public string CalledAETitle { get; set; } = "";
        public string CallingAETitle { get; set; } = "";
        public string HostAddress { get; set; } = "";
        public int HostPort { get; set; }
        public bool UseTls { get; set; }
    }
}
