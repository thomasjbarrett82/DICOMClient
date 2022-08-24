using DICOMClient.Common.Constant;
using DICOMClient.Core;
using Serilog;
using Serilog.Formatting.Display;
using Serilog.Sinks.WPF;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using WinForms = System.Windows.Forms;

namespace DICOMClient.WPF.CStore {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public CStoreViewModel CStore = new CStoreViewModel();

        public MainWindow() {
            var logTemplate = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteToSimpleAndRichTextBox(logTemplate)
                .CreateLogger();

            InitializeComponent();

            var service = new ApplicationEntityService(); // TODO dependency injection
            CStore.TargetList = service.GetAETitles(File.ReadAllText("aetitles.cstore.config.json"));
            CStore.TargetNameList = CStore.TargetList
                .Select(t => t.Name)
                .ToList();
            DataContext = CStore;

            Closed += new EventHandler(MainWindow_Closed);
        }

        private void CStoreButtonForFile_OnClick(object sender, RoutedEventArgs e) {
            var openFileDialog = new WinForms.OpenFileDialog {
                Multiselect = false
            };
            if (openFileDialog.ShowDialog() == WinForms.DialogResult.OK) {
                CStore.FileName = openFileDialog.FileName;
                CStore.FileFolderRadio = FileFolder.File;
            }
        }

        private void CStoreButtonForFolder_OnClick(object sender, RoutedEventArgs e) {
            using var openFolderDialog = new WinForms.FolderBrowserDialog();
            var result = openFolderDialog.ShowDialog();
            if (result == WinForms.DialogResult.OK) {
                CStore.FolderName = openFolderDialog.SelectedPath;
                CStore.FileFolderRadio = FileFolder.Folder;
            }
        }

        private void MainWindow_Closed(object? sender, EventArgs? e) {
            Log.Information("Closing application window");
            Log.CloseAndFlush();
        }
    }
}
