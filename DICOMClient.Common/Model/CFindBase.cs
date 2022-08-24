using FellowOakDicom;
using DICOMClient.Common.Constant;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DICOMClient.Common.Model {
    public class CFindBase : DimseService {
        protected string _queryLevel = "";
        public string QueryLevel {
            get => _queryLevel;
            set {
                _queryLevel = value;
            }
        }

        protected List<string> _queryLevelList = Enum.GetNames(typeof(QueryLevels)).ToList();
        public List<string> QueryLevelList => _queryLevelList;

        protected Dictionary<DicomTag, string> _parameters = new Dictionary<DicomTag, string>();
        public Dictionary<DicomTag, string> Parameters {
            get => _parameters;
            set {
                _parameters = value;
            }
        }
    }
}