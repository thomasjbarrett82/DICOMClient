using DICOMClient.Common.Constant;
using DICOMClient.Common.Core;
using DICOMClient.Common.Dto;
using DICOMClient.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DICOMClient.WPF.CStore {
    public class SubmitCStoreCommand : ICommand {
        public CStoreViewModel VM;
        private IDimseScuService _scuService;

        public SubmitCStoreCommand(CStoreViewModel vm) {
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
                Log.Warning($"CStore parameters not valid.");
                return;
            }
            ExecuteCStoreRequest();
        }

        #region PrivateMethods

        private bool SubmitIsNotValid() {
            return (
                string.IsNullOrWhiteSpace(VM.TargetName) ||
                VM.FileFolderRadio == null ||
                (VM.FileFolderRadio == FileFolder.File && string.IsNullOrWhiteSpace(VM.FileName)) ||
                (VM.FileFolderRadio == FileFolder.Folder && string.IsNullOrWhiteSpace(VM.FolderName))
            );
        }

        private void ExecuteCStoreRequest() {
            var ae = VM.TargetList.FirstOrDefault(t => t.Name == VM.TargetName);
            if (ae == null) {
                Log.Warning($"Invalid target name {VM.TargetName}");
                return;
            }
            if (VM.FileFolderRadio == null) {
                Log.Warning("Must choose file or folder operation.");
                return;
            }
            
            var items = new List<string>();
            var source = "";
            switch (VM.FileFolderRadio) { 
                case FileFolder.File:
                    if (string.IsNullOrWhiteSpace(VM.FileName) || !File.Exists(VM.FileName)) {
                        Log.Error("Invalid file");
                        return;
                    }
                    source = VM.FileName;
                    items.Add(source);
                    break;
                case FileFolder.Folder:
                    if (string.IsNullOrWhiteSpace(VM.FolderName) || !Directory.Exists(VM.FolderName)) {
                        Log.Error("Invalid folder");
                        return;
                    }
                    source = VM.FolderName;
                    items.AddRange(GetDicomFiles());
                    break;
            }

            var messageBoxResult = MessageBox.Show($"Confirm C-Store request:\n\nSource: {source}\n\nTarget: {VM.TargetName}", "C-Store Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes) {
                return;
            }

            var request = new CStoreRequest {
                CalledAETitle = ae.HostTitle,
                CallingAETitle = ae.CallingTitle,
                HostAddress = ae.HostAddress,
                HostPort = ae.HostPort,
                UseTls = ae.UseTls,
                Items = items
            };
            _scuService.OnCStoreRequest(request);
        }

        private IEnumerable<string> GetDicomFiles() {
            // TODO order items for Aria requirements - CT (by folder), RTStruct, RTPlan, RTDose, then anything else
            // TODO get the list of items, get the folder and DICOM type for each file, then do OrderBy
            var includeSubfolders = VM.IncludeSubfolders;
            var subfolderOption = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.EnumerateFiles(VM.FolderName, "*.*", subfolderOption);
        }

        #endregion
    }
}
