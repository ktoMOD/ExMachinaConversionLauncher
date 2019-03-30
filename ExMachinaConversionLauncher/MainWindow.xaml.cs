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
using ExMachinaConversionLauncher.Models;
using ExMachinaConversionLauncher.Services;

namespace ExMachinaConversionLauncher
{
    public partial class MainWindow : Window
    {
        private readonly LauncherConfigReader _launcherConfigReader;
        private readonly ErrorHandler _errorHandler;

        public MainWindow()
        {
            _errorHandler = new ErrorHandler();
            try
            {
                InitializeComponent();

                _launcherConfigReader = new LauncherConfigReader(Directory.GetCurrentDirectory() + @"\LauncherConfig\Launcher.config", _errorHandler);
                _launcherConfigReader.GetDataFromFile();
                _launcherConfigReader.Games.ForEach(x => ListOfMods.Items.Add(x.Name));
                ListOfMods.SelectedItem = _launcherConfigReader.LastLaunchGame;
                string[] launchMods = { "Первое ядро CPU", "Второе ядро CPU", "Все ядра CPU" };
                ListOfLaunchMode.ItemsSource = launchMods;
                ListOfLaunchMode.SelectedItem = _launcherConfigReader.LastLaunchMode;

                LabelVersion.Content = "Version: " + _launcherConfigReader.Version;
                var selectedGame = _launcherConfigReader.Games.FirstOrDefault(x => x.Name == _launcherConfigReader.LastLaunchGame);
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
                var selectedGameName = ListOfMods.SelectedValue != null ? ListOfMods.SelectedValue.ToString() : _launcherConfigReader.LastLaunchGame;
                var selectedGame = _launcherConfigReader.Games.FirstOrDefault(x => x.Name == selectedGameName);

                if (selectedGame == null) return;

                ModDescription.Text = selectedGame.Description;
                var logo = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\LauncherConfig" + selectedGame.PicturePath));
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
                var source = PresentationSource.FromVisual(this);
                double scaleX = 1;
                if (source?.CompositionTarget != null)
                {
                    scaleX = source.CompositionTarget.TransformToDevice.M11;
                }

                var writeConfig = new WriteConfig(_errorHandler);
                writeConfig.UpdateUiSchema2Hd(scaleX, _launcherConfigReader.FontScaleParamsForHd);
                writeConfig.UpdateLauncherConfig(new Dictionary<string, string>() { { "lastLaunchGame", (string)ListOfMods.SelectedItem }, { "lastLaunchMode", (string)ListOfLaunchMode.SelectedItem } });

                var selectedGameName = ListOfMods.SelectedValue != null ? ListOfMods.SelectedValue.ToString() : _launcherConfigReader.LastLaunchGame;
                var selectedGame = _launcherConfigReader.Games.FirstOrDefault(x => x.Name == selectedGameName);

                var hdMode = _launcherConfigReader.LastLaunchHdMode;
                writeConfig.WriteConfigBySelectionGame(selectedGame, hdMode);

                var fullScreenParams = new List<GameConfigParameterModel>
                    {new GameConfigParameterModel("r_fullScreen", _launcherConfigReader.FullScreen)};
                
                writeConfig.UpdateGameConfigWithParameters(fullScreenParams);

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
                        WorkingDirectory = Directory.GetCurrentDirectory(),
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
