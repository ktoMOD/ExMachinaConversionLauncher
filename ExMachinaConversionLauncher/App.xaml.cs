using System.IO;
using System.Windows;
using ExMachinaConversionLauncher.Services;

namespace ExMachinaConversionLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string PathToMainDirectory { get; set; } = Directory.GetCurrentDirectory();
        public SettingsService SettingsService { get; set; }

        void App_Startup(object sender, StartupEventArgs e)
        {
            for (var i = 0; i != e.Args.Length; ++i)
            {
                if (!e.Args[i].Contains("-path=")) continue;

                var param = e.Args[i].Substring(6).Trim('"');
                if (!string.IsNullOrEmpty(param))
                {
                    PathToMainDirectory += $@"\{param}";
                }
            }
            SettingsService = new SettingsService(PathToMainDirectory);

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
