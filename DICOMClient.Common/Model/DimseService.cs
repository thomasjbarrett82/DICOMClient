using System.Collections.Generic;

namespace DICOMClient.Common.Model {
    public abstract class DimseService {
        protected string _targetName = "";
        public string TargetName {
            get => _targetName;
            set {
                _targetName = value;
            }
        }

        protected List<ApplicationEntity> _targetList { get; set; } = new List<ApplicationEntity>();
        public List<ApplicationEntity> TargetList {
            get => _targetList;
            set {
                _targetList = value;
            }
        }

        protected List<string> _targetNameList { get; set; } = new List<string>();
        public List<string> TargetNameList {
            get => _targetNameList;
            set {
                _targetNameList = value;
            }
        }

        protected string _consoleOutput { get; set; } = "";
        public string ConsoleOutput {
            get => _consoleOutput;
            set {
                _consoleOutput = value;
            }
        }
    }
}
