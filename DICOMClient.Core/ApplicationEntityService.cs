using DICOMClient.Common.Core;
using DICOMClient.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DICOMClient.Core {
    public class ApplicationEntityService : IApplicationEntityService {
        public List<ApplicationEntity> GetAETitles(string configJson) {
            if (string.IsNullOrWhiteSpace(configJson)) {
                throw new ArgumentNullException(nameof(configJson));
            }
            var result = JsonSerializer.Deserialize<List<ApplicationEntity>>(configJson);
            if (result == null) {
                throw new ArgumentNullException(nameof(configJson));
            }
            var list = result.OrderBy(x => x.Name).ToList();
            return list.Count == list.Select(x => x.Name).Distinct().Count() // check for duplicate names
                ? list
                : throw new DicomClientOperationException("Application Entities can not contain duplicate names");
        }
    }
}
