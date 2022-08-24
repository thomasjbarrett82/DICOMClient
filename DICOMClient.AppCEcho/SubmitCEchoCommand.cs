using DICOMClient.Common.Core;
using DICOMClient.Common.Dto;
using DICOMClient.Core;
using Serilog;
using System;
using System.Linq;
using System.Windows.Input;

namespace DICOMClient.WPF.CEcho {
    public class SubmitCEchoCommand : ICommand {
        public CEchoViewModel VM;
        private readonly IDimseScuService _scuService;

        public SubmitCEchoCommand(CEchoViewModel vm) {
            VM = vm;
            _scuService = new DimseScuService(); // TODO dependency injection
        }

        public bool CanExecute(object? parameter) {
            return !SubmitIsNotValid();
        }

        public event EventHandler? CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter) {
            if (!CanExecute(parameter)) {
                return;
            }
            if (SubmitIsNotValid()) {
                Log.Warning($"Target name must be selected first");
                return;
            }
            ExecuteCEchoRequest();
        }

        #region PrivateMethods

        private bool SubmitIsNotValid() {
            return string.IsNullOrWhiteSpace(VM.TargetName);
        }

        private void ExecuteCEchoRequest() {
            var ae = VM.TargetList.FirstOrDefault(t => t.Name == VM.TargetName);
            if (ae == null) {
                Log.Warning($"Invalid target name {VM.TargetName}");
                return;
            }
            var request = new CEchoRequest {
                CalledAETitle = ae.HostTitle,
                CallingAETitle = ae.CallingTitle,
                HostAddress = ae.HostAddress,
                HostPort = ae.HostPort,
                UseTls = ae.UseTls
            };
            _scuService.OnCEchoRequest(request);
        }

        #endregion
    }
}
