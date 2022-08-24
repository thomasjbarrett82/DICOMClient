using Microsoft.VisualStudio.TestTools.UnitTesting;
using DICOMClient.Common.Model;
using DICOMClient.Core;
using System;

namespace DicomClient.Test.Core {
    [TestClass]
    public class ApplicationEntityTests {
#pragma warning disable CS8604 // Possible null reference argument.
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AeService_ConfigJsonIsNull() {
            string? json = null;
            var service = new ApplicationEntityService();
            service.GetAETitles(json);
        }
#pragma warning restore CS8604 // Possible null reference argument.

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AeService_ConfigJsonIsEmpty() {
            const string json = "";
            var service = new ApplicationEntityService();
            service.GetAETitles(json);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AeService_ConfigJsonIsWhitespace() {
            const string json = "      ";
            var service = new ApplicationEntityService();
            service.GetAETitles(json);
        }

        [TestMethod]
        [ExpectedException(typeof(DicomClientOperationException))]
        public void AeService_ConfigHasDuplicates() {
            const string json = "[{\"Name\":\"TestName\",\"HostTitle\":\"TestTitle\",\"HostAddress\":\"127.0.0.1\",\"HostPort\":12345,\"UseTls\":false,\"CallingTitle\":\"CallingTitle\"},{\"Name\":\"TestName\",\"HostTitle\":\"TestTitle2\",\"HostAddress\":\"127.0.0.2\",\"HostPort\":54321,\"UseTls\":true,\"CallingTitle\":\"CallingTitle2\"}]";
            var service = new ApplicationEntityService();
            service.GetAETitles(json);
        }

        [TestMethod]
        public void AeService_HappyPath() {
            const string json = "[{\"Name\":\"TestName\",\"HostTitle\":\"TestTitle\",\"HostAddress\":\"127.0.0.1\",\"HostPort\":12345,\"UseTls\":false,\"CallingTitle\":\"CallingTitle\"},{\"Name\":\"TestName2\",\"HostTitle\":\"TestTitle2\",\"HostAddress\":\"127.0.0.2\",\"HostPort\":54321,\"UseTls\":true,\"CallingTitle\":\"CallingTitle2\"}]";
            var service = new ApplicationEntityService();
            var result = service.GetAETitles(json);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2);
        }
    }
}