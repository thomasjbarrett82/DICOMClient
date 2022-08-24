using System;
using System.Runtime.Serialization;

namespace DICOMClient.Common.Model {
    public class DicomClientOperationException : InvalidOperationException {
        public DicomClientOperationException() : base() { }
        public DicomClientOperationException(string? message) : base(message) { }
        public DicomClientOperationException(string? message, Exception? innerException) : base(message, innerException) { }
        public DicomClientOperationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
