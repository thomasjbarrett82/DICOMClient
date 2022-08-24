using FellowOakDicom;
using MaterialDesignThemes.Wpf;
using DICOMClient.Common.Constant;
using DICOMClient.Core;
using Serilog;
using Serilog.Formatting.Display;
using Serilog.Sinks.WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace DICOMClient.WPF.CFind {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public CFindViewModel CFind = new CFindViewModel();

        public MainWindow() {
            var logTemplate = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteToSimpleAndRichTextBox(logTemplate)
                .CreateLogger();

            InitializeComponent();

            var service = new ApplicationEntityService(); // TODO dependency injection
            CFind.TargetList = service.GetAETitles(File.ReadAllText("aetitles.cfind.config.json"));
            CFind.TargetNameList = CFind.TargetList
                .Select(t => t.Name)
                .ToList();
            DataContext = CFind;

            Closed += new EventHandler(MainWindow_Closed);
        }

        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs) { // TODO move to ViewModel?
            if (eventArgs != null && !bool.Parse(eventArgs.Parameter.ToString())) { // Parameter "False" == dialog cancelled
                return;
            }

            // TODO handle incorrect data in fluent validation instead of here
            // TODO BUG if you enter a value and hit "enter" instead of the accept button, it doesn't read the Value field

            if (string.IsNullOrWhiteSpace(CFind.DicomTagName)) {
                MessageBox.Show($"Missing tag or value, parameter not added.", "Save warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                // TODO re-open dialog instead of returning
                return;
            }

            // Parameter "True" == dialog accepted
            var tag = (DicomTag)typeof(DicomTag)
                .GetField(CFind.DicomTagName)
                .GetValue(this);

            if (CFind.ParametersOC.FirstOrDefault(p => p.TagKey == tag) != null) {
                MessageBox.Show($"{CFind.DicomTagName} already exists in the list.", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
                // TODO re-open dialog instead of returning
                return;
            }

            CFind.ParametersOC.Add(new DicomTagValuePair {
                TagKey = tag,
                TagName = tag.DictionaryEntry.Name,
                TagValue = CFind.DicomTagValue
            });

            CFind.DicomTagName = string.Empty;
            CFind.DicomTagValue = string.Empty;

            AddParamDialog_TagName.Text = string.Empty;
            AddParamDialog_TagValue.Text = string.Empty;
        }

        private void DeleteParameter_OnClick(object sender, EventArgs args) { // TODO move to ViewModel?
            var button = (Button)sender;
            var tag = (DicomTagValuePair)button.DataContext;
            if (CFind.ParametersOC.FirstOrDefault(p => p.TagKey == tag.TagKey) == null) {
                MessageBox.Show($"Unknown error, {tag.TagKey} not found in parameters.", "Unknown Error", MessageBoxButton.OK);
            }

            CFind.ParametersOC.Remove(tag);
        }

        private void MainWindow_Closed(object? sender, EventArgs? e) {
            Log.Information("Closing application window");
            Log.CloseAndFlush();
        }
    }
}
