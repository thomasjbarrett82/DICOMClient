using DICOMClient.Common.Model;
using System.Collections.Generic;

namespace DICOMClient.Common.Core {
    public interface IApplicationEntityService {
        List<ApplicationEntity> GetAETitles(string config);
    }
}
