using FellowOakDicom;
using DICOMClient.Common.Core;
using DICOMClient.Common.Dto;
using DICOMClient.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DICOMClient.WPF.CFind {
    public class SubmitCFindCommand : ICommand {
        public CFindViewModel VM;
        private IDimseScuService _scuService;

        public SubmitCFindCommand(CFindViewModel vm) {
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
                Log.Warning($"CFind parameters not valid.");
                return;
            }
            ExecuteCFindRequest();
        }

        #region PrivateMethods

        private bool SubmitIsNotValid() {
            return (
                string.IsNullOrWhiteSpace(VM.TargetName) ||
                string.IsNullOrWhiteSpace(VM.QueryLevel) ||
                VM.ParametersOC.Count == 0
            );
        }

        private void ExecuteCFindRequest() {
            var ae = VM.TargetList.FirstOrDefault(t => t.Name == VM.TargetName);
            if (ae == null) {
                Log.Warning($"Invalid target name {VM.TargetName}");
                return;
            }

            var parameters = new Dictionary<DicomTag, string>();
            foreach (var p in VM.ParametersOC) {
                parameters.Add(p.TagKey, p.TagValue);
            }

            var request = new CFindRequest {
                CalledAETitle = ae.HostTitle,
                CallingAETitle = ae.CallingTitle,
                HostAddress = ae.HostAddress,
                HostPort = ae.HostPort,
                UseTls = ae.UseTls,
                QueryLevel = VM.QueryLevel,
                Parameters = parameters
            };
            _scuService.OnCFindRequest(request);
        }

        #endregion PrivateMethods
    }
}
