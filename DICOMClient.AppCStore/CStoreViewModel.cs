using DICOMClient.Common.Constant;
using DICOMClient.Common.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DICOMClient.WPF.CStore {
    public class CStoreViewModel : CStoreBase, INotifyPropertyChanged {
        public CStoreViewModel() {
            SubmitCommand = new SubmitCStoreCommand(this);
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

        #region CStoreProperties

        public new FileFolder? FileFolderRadio {
            get => _fileFolderRadio;
            set {
                _fileFolderRadio = value;
                NotifyPropertyChanged(nameof(FileFolderRadio));
            }
        }

        public new string FileName {
            get => _fileName;
            set {
                _fileName = value;
                NotifyPropertyChanged(nameof(FileName));
            }
        }

        public new string FolderName {
            get => _folderName;
            set {
                _folderName = value;
                NotifyPropertyChanged(nameof(FolderName));
            }
        }

        public new bool IncludeSubfolders {
            get => _includeSubfolders;
            set {
                _includeSubfolders = value;
                NotifyPropertyChanged(nameof(IncludeSubfolders));
            }
        }

        #endregion CStoreProperties

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region ICommand

        private SubmitCStoreCommand submitCmd;
        public SubmitCStoreCommand SubmitCommand {
            get {
                return submitCmd;
            }
            set {
                submitCmd = value;
            }
        }

        #endregion ICommand
    }
}
