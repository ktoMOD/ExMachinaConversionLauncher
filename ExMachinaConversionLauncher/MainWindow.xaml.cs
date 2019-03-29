using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ExMachinaConversionLauncher.Services;

namespace ExMachinaConversionLauncher
{
    public partial class MainWindow : Window
    {
        private readonly ConfigReader _configReader;
        private readonly ErrorHandler _errorHandler;

        public MainWindow()
        {
            _errorHandler = new ErrorHandler(this);
            try
            {
                InitializeComponent();

                _configReader = new ConfigReader(Directory.GetCurrentDirectory() + @"\LauncherConfig\Launcher.config", _errorHandler);
                _configReader.GetDataFromFile();
                _configReader.Games.ForEach(x => ListOfMods.Items.Add(x.Name));
                ListOfMods.SelectedItem = _configReader.LastLaunchGame;
                string[] launchMods = { "Первое ядро CPU", "Второе ядро CPU", "Все ядра CPU" };
                ListOfLaunchMode.ItemsSource = launchMods;
                ListOfLaunchMode.SelectedItem = _configReader.LastLaunchMode;

                LabelVersion.Content = "Version: " + _configReader.Version;
                var selectedGame = _configReader.Games.FirstOrDefault(x => x.Name == ListOfMods.SelectedValue);//.ToString());
                if (selectedGame != null)
                {
                    ModDescription.Text = selectedGame.Description;
                    var logo = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\LauncherConfig" + selectedGame.PicturePath));
                    Image.Source = logo;
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "MainWindow");
            }
        }

        private void ListOfMods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedGame = _configReader.Games.FirstOrDefault(x => x.Name == ListOfMods.SelectedValue);//.ToString());
                if (selectedGame != null)
                {
                    ModDescription.Text = selectedGame.Description;
                    var logo = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\LauncherConfig" + selectedGame.PicturePath));
                    Image.Source = logo;
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ListOfMods_SelectionChanged");
            }
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PresentationSource source = PresentationSource.FromVisual(this);
                double scaleX = 1;
                if (source != null)
                {
                    scaleX = source.CompositionTarget.TransformToDevice.M11;
                }

                WriteConfig writeConfig = new WriteConfig(_errorHandler);
                writeConfig.UpdateUiSchema2Hd(scaleX, _configReader.FontScaleParamsForHD);
                writeConfig.UpdateLauncherConfig(new Dictionary<string, string>() { { "lastLaunchGame", (string)ListOfMods.SelectedItem }, { "lastLaunchMode", (string)ListOfLaunchMode.SelectedItem } });
                writeConfig.WriteConfigBySelectionGame(ListOfMods.SelectedValue.ToString(), _configReader.Games.Select(x => x.Name).ToArray(), _configReader.ListBaseConfigDictionary);

                var hdMode = _configReader.LastLaunchHdMode;

                switch (hdMode)
                {
                    case "WithOutHD":
                        writeConfig.WriteConfigBySelectionGame(ListOfMods.SelectedValue.ToString(), _configReader.Games.Select(x => x.Name).ToArray(), _configReader.ListWithOutHdConfigDictionary);
                        break;
                    case "WithHDWithDefaultSight":
                        writeConfig.WriteConfigBySelectionGame(ListOfMods.SelectedValue.ToString(), _configReader.Games.Select(x => x.Name).ToArray(), _configReader.ListWithHdWithDefaultSightConfigDictionary);
                        break;
                    case "WithHDWithSmallSight":
                        writeConfig.WriteConfigBySelectionGame(ListOfMods.SelectedValue.ToString(), _configReader.Games.Select(x => x.Name).ToArray(), _configReader.ListWithHdWithSmallSightConfigDictionary);
                        break;
                    case "WithHDWithHardcoreSight":
                        writeConfig.WriteConfigBySelectionGame(ListOfMods.SelectedValue.ToString(), _configReader.Games.Select(x => x.Name).ToArray(), _configReader.ListWithHdWithHardcoreSightConfigDictionary);
                        break;
                    case "WithHDWithOvalSight":
                        writeConfig.WriteConfigBySelectionGame(ListOfMods.SelectedValue.ToString(), _configReader.Games.Select(x => x.Name).ToArray(), _configReader.ListWithHdWithOvalSightConfigDictionary);
                        break;
                }

                var fullScreenList = new List<Dictionary<string, string>>();
                for (var i = 0; i < _configReader.Games.Count; i++)
                {
                    fullScreenList.Add(new Dictionary<string, string>() {{"r_fullScreen", _configReader.FullScreen}});
                }
                writeConfig.WriteConfigBySelectionGame(ListOfMods.SelectedValue.ToString(), _configReader.Games.Select(x => x.Name).ToArray(), fullScreenList);

                string parametr = string.Empty;
                string console = string.Empty;

                if (ListOfLaunchMode.SelectedIndex == 0)
                {
                    parametr = " /LOW /NODE 0 /AFFINITY 0x1";
                }
                else if(ListOfLaunchMode.SelectedIndex == 1)
                {
                    parametr = " /AFFINITY 0x2";
                }

                if (_configReader.Console)
                {
                    console = "-console";
                }

                string command = String.Format("{0} {1} {2} {3}", "/C start", parametr,
                    "\"Ex Machina\" " + _configReader.ExeName, console);
                Process cmd = new Process();
                cmd.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                cmd.StartInfo.Arguments = command;
                cmd.Start();
                this.Close();

            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "StartGame_Click");
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            try
            {
                base.OnMouseLeftButtonDown(e);
                this.DragMove();
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "OnMouseLeftButtonDown");
            }
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "CloseApp_Click");
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "Hyperlink_RequestNavigate");
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var settingsWindow = new Settings(_configReader, _errorHandler);
                settingsWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "Settings_Click");
            }
        }

    }
}
