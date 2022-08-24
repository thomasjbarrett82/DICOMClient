using FellowOakDicom;
using DICOMClient.Common.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DICOMClient.WPF.CFind {
    public class CFindViewModel : CFindBase, INotifyPropertyChanged {
        public CFindViewModel() {
            SubmitCommand = new SubmitCFindCommand(this);
        }

        #region BaseProperties

        public new string TargetName {
            get => _targetName;
            set {
                _targetName = value;
                NotifyPropertyChanged(nameof(TargetName));
            }
        }

        public new List<ApplicationEntity> TargetList {
            get => _targetList;
            set {
                _targetList = value;
                NotifyPropertyChanged(nameof(TargetName));
            }
        }

        public new List<string> TargetNameList {
            get => _targetNameList;
            set {
                _targetNameList = value;
                NotifyPropertyChanged(nameof(TargetNameList));
            }
        }

        public new string ConsoleOutput {
            get => _consoleOutput;
            set {
                _consoleOutput = value;
                NotifyPropertyChanged(nameof(ConsoleOutput));
            }
        }

        #endregion BaseProperties

        #region CFindProperties

        public new string QueryLevel {
            get => _queryLevel;
            set {
                _queryLevel = value;
            }
        }

        public new List<string> QueryLevelList => _queryLevelList;

        public ObservableCollection<DicomTagValuePair> ParametersOC { get; set; } = new ObservableCollection<DicomTagValuePair>();

        protected List<string> _dicomTagNameList = typeof(DicomTag)
            .GetFields()
            .OrderBy(f => f.Name)
            .Select(f => f.Name)
            .ToList();
        public List<string> DicomTagNameList => _dicomTagNameList;

        protected string _dicomTagName = "";
        public string DicomTagName {
            get => _dicomTagName;
            set {
                _dicomTagName = value;
            }
        }

        protected string _dicomTagValue = "";
        public string DicomTagValue {
            get => _dicomTagValue;
            set {
                _dicomTagValue = value;
            }
        }

        #endregion

        #region ICommand

        private SubmitCFindCommand submitCmd;
        public SubmitCFindCommand SubmitCommand {
            get {
                return submitCmd;
            }
            set {
                submitCmd = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion ICommand
    }
}
