namespace DICOMClient.Common.Model {
    public class ApplicationEntity {
        public string Name { get; set; } = "";
        public string HostTitle { get; set; } = "";
        public string HostAddress { get; set; } = "";
        public int HostPort { get; set; }
        public bool UseTls { get; set; }
        public string CallingTitle { get; set; } = "";
    }
}
