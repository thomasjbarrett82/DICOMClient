using DICOMClient.Common.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DICOMClient.WPF.CEcho {
    public class CEchoViewModel : CEchoBase, INotifyPropertyChanged {
        public CEchoViewModel() {
            SubmitCommand = new SubmitCEchoCommand(this);
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region ICommand

        private SubmitCEchoCommand submitCmd;
        public SubmitCEchoCommand SubmitCommand {
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
