using DICOMClient.Core;
using Serilog;
using Serilog.Formatting.Display;
using Serilog.Sinks.WPF;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace DICOMClient.WPF.CEcho {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public CEchoViewModel CEcho = new CEchoViewModel();

        public MainWindow() {
            var logTemplate = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteToSimpleAndRichTextBox(logTemplate)
                .CreateLogger();

            InitializeComponent();

            var service = new ApplicationEntityService(); // TODO dependency injection
            CEcho.TargetList = service.GetAETitles(File.ReadAllText("aetitles.cecho.config.json"));
            CEcho.TargetNameList = CEcho.TargetList
                .Select(t => t.Name)
                .ToList();
            DataContext = CEcho;

            Closed += new EventHandler(MainWindow_Closed);
        }

        private void MainWindow_Closed(object? sender, EventArgs? e) {
            Log.Information("Closing application window");
            Log.CloseAndFlush();
        }
    }
}
