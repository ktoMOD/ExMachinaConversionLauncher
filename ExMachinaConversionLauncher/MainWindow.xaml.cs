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
        private readonly LauncherConfigReader _launcherConfigReader;
        private readonly string _pathToMainDirectory = ((App)Application.Current).PathToMainDirectory;
        private readonly SettingsService _settingsService = ((App)Application.Current).SettingsService;
        private readonly ErrorHandler _errorHandler;

        public MainWindow()
        {
            _errorHandler = new ErrorHandler();
            try
            {
                InitializeComponent();

                _launcherConfigReader = new LauncherConfigReader($@"{_pathToMainDirectory}\LauncherConfig\Launcher.config", _errorHandler);
                _launcherConfigReader.GetDataFromFile();
                _launcherConfigReader.Games.ForEach(x => ListOfMods.Items.Add(x.Name));
                ListOfMods.SelectedItem = _launcherConfigReader.LastLaunchGame;
                string[] launchMods = { "Первое ядро CPU", "Второе ядро CPU", "Все ядра CPU" };
                ListOfLaunchMode.ItemsSource = launchMods;
                ListOfLaunchMode.SelectedItem = _launcherConfigReader.LastLaunchMode;

                LabelVersion.Content = "Version: " + _launcherConfigReader.Version;
                var selectedGame = _launcherConfigReader.Games.FirstOrDefault(x => x.Name == _launcherConfigReader.LastLaunchGame);

                if (selectedGame == null) return;

                ModDescription.Text = selectedGame.Description;
                var logo = new BitmapImage(new Uri($@"{_pathToMainDirectory}\LauncherConfig" + selectedGame.PicturePath));
                Image.Source = logo;
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
                var selectedGameName = ListOfMods.SelectedValue != null ? ListOfMods.SelectedValue.ToString() : _launcherConfigReader.LastLaunchGame;
                var selectedGame = _launcherConfigReader.Games.FirstOrDefault(x => x.Name == selectedGameName);

                if (selectedGame == null) return;

                ModDescription.Text = selectedGame.Description;
                var logo = new BitmapImage(new Uri($@"{_pathToMainDirectory}\LauncherConfig" + selectedGame.PicturePath));
                Image.Source = logo;
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
                _launcherConfigReader.RefreshOtherParamsFromConfig();
                var writeConfig = new WriteConfig(_errorHandler);

                var selectedGameName = ListOfMods.SelectedValue != null ? ListOfMods.SelectedValue.ToString() : _launcherConfigReader.LastLaunchGame;
                var selectedGame = _launcherConfigReader.Games.FirstOrDefault(x => x.Name == selectedGameName);

                //Fill part start
                var uiSchema2HdParams = writeConfig.GetUiSchema2HdParams(this, _launcherConfigReader.FontScaleParamsForHd);
                _settingsService.AddParamsToUiSchema2Hd(uiSchema2HdParams);

                var launcherParams = new Dictionary<string, string>
                {
                    {"lastLaunchGame", (string) ListOfMods.SelectedItem},
                    {"lastLaunchMode", (string) ListOfLaunchMode.SelectedItem}
                };
                _settingsService.AddParamsToLauncherParams(launcherParams);

                var hdMode = _launcherConfigReader.LastLaunchHdMode;
                var mergedParameters = writeConfig.WriteConfigBySelectionGame(selectedGame, hdMode);
                _settingsService.AddParamsToGameParams(mergedParameters);

                var fullScreenParams = new Dictionary<string, string> { { "r_fullScreen", _launcherConfigReader.FullScreen } };
                _settingsService.AddParamsToGameParams(fullScreenParams);
                //Fill part end

                //Save part start
                _settingsService.SaveGameParams();
                _settingsService.SaveLauncherParams();
                _settingsService.SaveUiSchema2HdParams();
                //Save part end

                var parameter = string.Empty;
                var console = string.Empty;

                if (ListOfLaunchMode.SelectedIndex == 0)
                {
                    parameter = " /LOW /NODE 0 /AFFINITY 0x1";
                }
                else if(ListOfLaunchMode.SelectedIndex == 1)
                {
                    parameter = " /AFFINITY 0x2";
                }

                if (_launcherConfigReader.Console)
                {
                    console = "-console";
                }

                var command = $"{"/C start"} {parameter} {"\"Ex Machina\" " + _launcherConfigReader.ExeName} {console}";
                var cmd = new Process
                {
                    StartInfo =
                    {
                        WorkingDirectory = _pathToMainDirectory,
                        FileName = "cmd.exe",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = command
                    }
                };
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
                var settingsWindow = new Settings(_errorHandler);
                settingsWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "Settings_Click");
            }
        }

    }
}
