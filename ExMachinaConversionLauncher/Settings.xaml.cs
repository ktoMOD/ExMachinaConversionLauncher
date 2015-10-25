using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ExMachinaConversionLauncher.Models;

namespace ExMachinaConversionLauncher
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private string LastHdMode { get; set; }
        private ConfigReader ConfigReaderLocal { get; set; }
        readonly GameSettingsService _gameSettings;
        private readonly ErrorHandler _errorHandler;

        public Settings(ConfigReader configReader, ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            try
            {
                _gameSettings = new GameSettingsService(_errorHandler);
                ConfigReaderLocal = configReader;
                LastHdMode = ConfigReaderLocal.ReadHdMode();
                InitializeComponent();
                _gameSettings.GetDataFromFile();
                InitializeSettings();
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "Settings");
            }
        }

        private void InitializeSettings()
        {
            try
            {
                #region sliders

                ViewDistanceTextBox.Text = Convert.ToString(_gameSettings.LsViewDistanceDivider, CultureInfo.InvariantCulture);
                ViewDistanceSlider.Value = _gameSettings.LsViewDistanceDivider;
                ProjectorsFarDistTextBox.Text = Convert.ToString(_gameSettings.ProjectorsFarDist);
                ProjectorsFarDistSlider.Value = _gameSettings.ProjectorsFarDist;
                GrassDrawDistTextBox.Text = Convert.ToString(_gameSettings.GrassDrawDist, CultureInfo.InvariantCulture);
                GrassDrawDistSlider.Value = _gameSettings.GrassDrawDist;
                ShadowBlurCoeffTextBox.Text = Convert.ToString(_gameSettings.ShadowBlurCoeff, CultureInfo.InvariantCulture);
                ShadowBlurCoeffSlider.Value = _gameSettings.ShadowBlurCoeff;
                GammaGammaTextBox.Text = Convert.ToString(_gameSettings.Gamma, CultureInfo.InvariantCulture);
                GammaGammaSlider.Value = _gameSettings.Gamma;
                FovTextBox.Text = Convert.ToString(_gameSettings.Fov, CultureInfo.InvariantCulture);
                FovSlider.Value = _gameSettings.Fov;
                CamSpeedTextBox.Text = Convert.ToString(_gameSettings.CamSpeed);
                CamSpeedSlider.Value = _gameSettings.CamSpeed;
                MusicVolumeTextBox.Text = Convert.ToString(_gameSettings.Volume);
                MusicVolumeSlider.Value = _gameSettings.Volume;
                EffectVolumeTextBox.Text = Convert.ToString(_gameSettings.Volume3D);
                EffectVolumeSlider.Value = _gameSettings.Volume3D;
                SpeakVolumeTextBox.Text = Convert.ToString(_gameSettings.Volume2D);
                SpeakVolumeSlider.Value = _gameSettings.Volume2D;

                #endregion
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - sliders");
            }

            try
            {
                #region screenResolution

                var resolutionsCollection = ConfigReaderLocal.ResolutionsCollection;
                ResolutionComboBox.ItemsSource = resolutionsCollection;
                ResolutionComboBox.SelectedItem = _gameSettings.Width + "×" + _gameSettings.Height;

                #endregion
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - screenResolution");
            }

            try
            {
                #region sight

                var sightCollection = new List<string>()
            {
                "Стандартный",
                "Маленький",
                "Овальный"
            };
                SightComboBox.ItemsSource = sightCollection;
                string sightQualityInConfig;
                switch (LastHdMode)
                {
                    case "WithHDWithSmallSight":
                        sightQualityInConfig = "Маленький";
                        break;
                    case "WithHDWithOvalSight":
                        sightQualityInConfig = "Овальный";
                        break;
                    default:
                        sightQualityInConfig = "Стандартный";
                        break;
                }
                SightComboBox.SelectedItem = sightQualityInConfig;

                #endregion

            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - sight");
            }

            try
            {
                #region waterQuality

                var waterQualityCollection = new List<string>()
            {
                "Низкое",
                "Среднее",
                "Высокое"
            };
                WaterQualityComboBox.ItemsSource = waterQualityCollection;
                string waterQualityInConfig = String.Empty;
                switch (_gameSettings.WaterQuality)
                {
                    case 1:
                        waterQualityInConfig = "Низкое";
                        break;
                    case 2:
                        waterQualityInConfig = "Среднее";
                        break;
                    case 3:
                        waterQualityInConfig = "Высокое";
                        break;
                }
                WaterQualityComboBox.SelectedItem = waterQualityInConfig;

                #endregion
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - waterQuality");
            }

            try
            {
                #region shadowsQuality

                var shadowsQualityCollection = new List<string>()
            {
                "Низкое",
                "Среднее",
                "Высокое"
            };
                ShadowsQualityComboBox.ItemsSource = shadowsQualityCollection;
                string shadowsQualityForSelect = String.Empty;
                string shadowsQualityInConfig = Convert.ToString(_gameSettings.DsShadows) + "_" +
                                                Convert.ToString(_gameSettings.DetShadowTexSz) + "_" +
                                                Convert.ToString(_gameSettings.LgtShadowTexSz);


                switch (shadowsQualityInConfig)
                {
                    case "False_512_256":
                        shadowsQualityForSelect = "Низкое";
                        break;
                    case "True_512_256":
                        shadowsQualityForSelect = "Среднее";
                        break;
                    case "True_1024_512":
                        shadowsQualityForSelect = "Высокое";
                        break;
                }
                ShadowsQualityComboBox.SelectedItem = shadowsQualityForSelect;

                #endregion
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - shadowsQuality");
            }

            try
            {
                #region postEffectBloom

                var postEffectBloomCollection = new List<string>()
            {
                "Низкое",
                "Среднее",
                "Высокое"
            };
                PostEffectBloomComboBox.ItemsSource = postEffectBloomCollection;
                string postEffectBloomInConfig = String.Empty;
                switch (_gameSettings.PostEffectBloom)
                {
                    case 0:
                        postEffectBloomInConfig = "Низкое";
                        break;
                    case 1:
                        postEffectBloomInConfig = "Среднее";
                        break;
                    case 2:
                        postEffectBloomInConfig = "Высокое";
                        break;
                }
                PostEffectBloomComboBox.SelectedItem = postEffectBloomInConfig;

                #endregion
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - postEffectBloom");
            }

            try
            {
                #region smoothing

                var smoothingCollection = new List<string>()
            {
                "Отключено",
                "×2",
                "×4",
                "×8"
            };
                SmoothingComboBox.ItemsSource = smoothingCollection;
                string smoothingInConfig = String.Empty;
                switch (_gameSettings.MultiSamplesNum)
                {
                    case 0:
                        smoothingInConfig = "Отключено";
                        break;
                    case 2:
                        smoothingInConfig = "×2";
                        break;
                    case 4:
                        smoothingInConfig = "×4";
                        break;
                    case 8:
                        smoothingInConfig = "×8";
                        break;
                }
                SmoothingComboBox.SelectedItem = smoothingInConfig;

                #endregion
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - smoothing");
            }

            try
            {
                #region texturesFilter

                var texturesFilterCollection = new List<string>()
            {
                "Билинейная",
                "Трилинейная",
                "Анизотропная"
            };
                TexturesFilterComboBox.ItemsSource = texturesFilterCollection;
                string texturesFilterForSelect = String.Empty;
                string texturesFilterInConfig = Convert.ToString(_gameSettings.TexturesFilter) + "_" +
                                                Convert.ToString(_gameSettings.ShaderMacro1);


                switch (texturesFilterInConfig)
                {
                    case "4_MIP_FILTER Point":
                        texturesFilterForSelect = "Билинейная";
                        break;
                    case "5_MIP_FILTER Linear":
                        texturesFilterForSelect = "Трилинейная";
                        break;
                    case "3_MIP_FILTER Linear":
                        texturesFilterForSelect = "Анизотропная";
                        break;
                }
                TexturesFilterComboBox.SelectedItem = texturesFilterForSelect;

                #endregion
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - texturesFilter");
            }

            try
            {
                #region checkBoxes

                AutoPlayVideoCheckBox.IsChecked = _gameSettings.AutoPlayVideo;
                DoNotLoadMainmenuLevelCheckBox.IsChecked = !_gameSettings.DoNotLoadMainmenuLevel;
                SwitchCameraAllowCheckBox.IsChecked = _gameSettings.SwitchCameraAllow;

                #endregion
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - checkBoxes");
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WriteConfig writeConfig = new WriteConfig(_errorHandler);

                var resolution = ResolutionComboBox.SelectedItem.ToString().Split('×');
                _gameSettings.DesiredHeight = Convert.ToInt32(resolution[1]);
                _gameSettings.DesiredWidth = Convert.ToInt32(resolution[0]);
                _gameSettings.Height = Convert.ToInt32(resolution[1]);
                _gameSettings.Width = Convert.ToInt32(resolution[0]);
                _gameSettings.LsViewDistanceDivider = ViewDistanceSlider.Value;
                _gameSettings.ProjectorsFarDist = Convert.ToInt32(ProjectorsFarDistSlider.Value);
                _gameSettings.GrassDrawDist = GrassDrawDistSlider.Value;
                switch ((string)WaterQualityComboBox.SelectedItem)
                {
                    case "Низкое":
                        _gameSettings.WaterQuality = 1;
                        break;
                    case "Среднее":
                        _gameSettings.WaterQuality = 2;
                        break;
                    case "Высокое":
                        _gameSettings.WaterQuality = 3;
                        break;
                }
                switch ((string)ShadowsQualityComboBox.SelectedItem)
                {
                    case "Низкое":
                        _gameSettings.DsShadows = false;
                        _gameSettings.DetShadowTexSz = 512;
                        _gameSettings.LgtShadowTexSz = 256;
                        break;
                    case "Среднее":
                        _gameSettings.DsShadows = true;
                        _gameSettings.DetShadowTexSz = 512;
                        _gameSettings.LgtShadowTexSz = 256;
                        break;
                    case "Высокое":
                        _gameSettings.DsShadows = true;
                        _gameSettings.DetShadowTexSz = 1024;
                        _gameSettings.LgtShadowTexSz = 512;
                        break;
                }
                _gameSettings.ShadowBlurCoeff = ShadowBlurCoeffSlider.Value;
                switch ((string)PostEffectBloomComboBox.SelectedItem)
                {
                    case "Низкое":
                        _gameSettings.PostEffectBloom = 0;
                        break;
                    case "Среднее":
                        _gameSettings.PostEffectBloom = 1;
                        break;
                    case "Высокое":
                        _gameSettings.PostEffectBloom = 2;
                        break;
                }
                switch ((string)SmoothingComboBox.SelectedItem)
                {
                    case "Отключено":
                        _gameSettings.MultiSamplesNum = 0;
                        break;
                    case "×2":
                        _gameSettings.MultiSamplesNum = 2;
                        break;
                    case "×4":
                        _gameSettings.MultiSamplesNum = 4;
                        break;
                    case "×8":
                        _gameSettings.MultiSamplesNum = 8;
                        break;
                }
                switch ((string)TexturesFilterComboBox.SelectedItem)
                {
                    case "Билинейная":
                        _gameSettings.TexturesFilter = 4;
                        _gameSettings.ShaderMacro1 = "MIP_FILTER Point";
                        break;
                    case "Трилинейная":
                        _gameSettings.TexturesFilter = 5;
                        _gameSettings.ShaderMacro1 = "MIP_FILTER Linear";
                        break;
                    case "Анизотропная":
                        _gameSettings.TexturesFilter = 3;
                        _gameSettings.ShaderMacro1 = "MIP_FILTER Linear";
                        break;
                }
                _gameSettings.Gamma = GammaGammaSlider.Value;
                if (AutoPlayVideoCheckBox.IsChecked != null) _gameSettings.AutoPlayVideo = (bool)AutoPlayVideoCheckBox.IsChecked;
                if (DoNotLoadMainmenuLevelCheckBox.IsChecked != null) _gameSettings.DoNotLoadMainmenuLevel = (bool)!DoNotLoadMainmenuLevelCheckBox.IsChecked;
                _gameSettings.Fov = FovSlider.Value;
                if (SwitchCameraAllowCheckBox.IsChecked != null) _gameSettings.SwitchCameraAllow = (bool)SwitchCameraAllowCheckBox.IsChecked;
                _gameSettings.CamSpeed = Convert.ToInt32(CamSpeedSlider.Value);
                _gameSettings.Volume = Convert.ToInt32(MusicVolumeSlider.Value);
                _gameSettings.Volume3D = Convert.ToInt32(EffectVolumeSlider.Value);
                _gameSettings.Volume2D = Convert.ToInt32(SpeakVolumeSlider.Value);

                _gameSettings.SaveSettingsToConfig();

                if (HdCheckBox.IsChecked != null && (bool)HdCheckBox.IsChecked)
                {
                    string sightQualityToConfig;
                    switch ((string)SightComboBox.SelectedItem)
                    {
                        case "Маленький":
                            sightQualityToConfig = "WithHDWithSmallSight";
                            break;
                        case "Овальный":
                            sightQualityToConfig = "WithHDWithOvalSight";
                            break;
                        default:
                            sightQualityToConfig = "WithHDWithDefaultSight";
                            break;
                    }
                    writeConfig.UpdateLauncherConfig(new Dictionary<string, string>() { { "<launcherHDMode>", sightQualityToConfig } });
                }
                else
                {
                    writeConfig.UpdateLauncherConfig(new Dictionary<string, string>() { { "<launcherHDMode>", "WithOutHD" } });
                }

                this.Close();
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "Save_Click");
                this.Close();
            }
        }

        private void ViewDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                ViewDistanceTextBox.Text = Convert.ToString(Math.Round(ViewDistanceSlider.Value, 2), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ViewDistanceSlider_ValueChanged");
            }
        }

        private void ProjectorsFarDistSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                ProjectorsFarDistTextBox.Text = Convert.ToString(ProjectorsFarDistSlider.Value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ProjectorsFarDistSlider_ValueChanged");
            }
        }

        private void GrassDrawDistSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                GrassDrawDistTextBox.Text = Convert.ToString(Math.Round(GrassDrawDistSlider.Value, 0), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GrassDrawDistSlider_ValueChanged");
            }
        }

        private void ShadowBlurCoeffSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                ShadowBlurCoeffTextBox.Text = Convert.ToString(Math.Round(ShadowBlurCoeffSlider.Value, 0), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ShadowBlurCoeffSlider_ValueChanged");
            }
        }

        private void GammaGammaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                GammaGammaTextBox.Text = Convert.ToString(Math.Round(GammaGammaSlider.Value, 2), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GammaGammaSlider_ValueChanged");
            }
        }

        private void FovSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                FovTextBox.Text = Convert.ToString(Math.Round(FovSlider.Value, 2), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "FovSlider_ValueChanged");
            }
        }

        private void CamSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                CamSpeedTextBox.Text = Convert.ToString(CamSpeedSlider.Value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "CamSpeedSlider_ValueChanged");
            }
        }

        private void MusicVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                MusicVolumeTextBox.Text = Convert.ToString(MusicVolumeSlider.Value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "MusicVolumeSlider_ValueChanged");
            }
        }

        private void EffectVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                EffectVolumeTextBox.Text = Convert.ToString(EffectVolumeSlider.Value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "EffectVolumeSlider_ValueChanged");
            }
        }

        private void SpeakVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                SpeakVolumeTextBox.Text = Convert.ToString(SpeakVolumeSlider.Value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "SpeakVolumeSlider_ValueChanged");
            }
        }

        private void HdCheckBoxCheck(object sender, RoutedEventArgs e)
        {
            try
            {
                SightComboBox.IsEnabled = true;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "HdCheckBoxCheck");
            }
        }

        private void HdCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                SightComboBox.IsEnabled = false;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "HdCheckBoxUnchecked");
            }
        }

        private void ResolutionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var resolution = ResolutionComboBox.SelectedItem.ToString().Split('×');
                var height = Convert.ToInt32(resolution[1]);
                var width = Convert.ToInt32(resolution[0]);
                var ratio = (double)width / (double)height;

                if (Math.Abs(ratio - 16.0 / 9.0) < 0.01 || Math.Abs(ratio - 16.0 / 10.0) < 0.01)
                {
                    LastHdMode = ConfigReaderLocal.ReadHdMode();
                    if (LastHdMode == "WithOutHD" && e.AddedItems.Count != e.RemovedItems.Count)
                    {
                        SightComboBox.IsEnabled = false;
                        HdCheckBox.IsChecked = false;
                    }
                    else
                    {
                        SightComboBox.IsEnabled = true;
                        HdCheckBox.IsChecked = true;
                    }
                    HdCheckBox.IsEnabled = true;
                }
                else
                {
                    HdCheckBox.IsChecked = false;
                    HdCheckBox.IsEnabled = false;
                    SightComboBox.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ResolutionComboBox_SelectionChanged");
            }
        }

        private void ViewDistanceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double viewDistance;
                bool isDouble = Double.TryParse(ViewDistanceTextBox.Text.Replace(",", "."), out viewDistance);
                if (isDouble && viewDistance >= 0 && viewDistance <= 1)
                {
                    ViewDistanceSlider.Value = viewDistance;
                }
                else
                {
                    ViewDistanceTextBox.Text = Convert.ToString(Math.Round(ViewDistanceSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите значение от 0 до 1.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ViewDistanceTextBox_LostFocus");
            }
        }

        private void ProjectorsFarDistTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int projectorsFarDistSlider;
                bool isInt = Int32.TryParse(ProjectorsFarDistTextBox.Text.Replace(",", "."), out projectorsFarDistSlider);
                if (isInt && projectorsFarDistSlider >= 0 && projectorsFarDistSlider <= 5)
                {
                    ProjectorsFarDistSlider.Value = projectorsFarDistSlider;
                }
                else
                {
                    ProjectorsFarDistTextBox.Text = Convert.ToString(Math.Round(ProjectorsFarDistSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите целое значение от 0 до 5.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ProjectorsFarDistTextBox_LostFocus");
            }
        }

        private void GrassDrawDistTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int grassDrawDistance;
                bool isInt = Int32.TryParse(GrassDrawDistTextBox.Text.Replace(",", "."), out grassDrawDistance);
                if (isInt && grassDrawDistance >= 0 && grassDrawDistance <= 350)
                {
                    GrassDrawDistSlider.Value = grassDrawDistance;
                }
                else
                {
                    GrassDrawDistTextBox.Text = Convert.ToString(Math.Round(GrassDrawDistSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите целое значение от 0 до 350.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GrassDrawDistTextBox_LostFocus");
            }
        }

        private void ShadowBlurCoeffTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int shadowBlurCoeff;
                bool isInt = Int32.TryParse(ShadowBlurCoeffTextBox.Text.Replace(",", "."), out shadowBlurCoeff);
                if (isInt && shadowBlurCoeff >= 0 && shadowBlurCoeff <= 50)
                {
                    ShadowBlurCoeffSlider.Value = shadowBlurCoeff;
                }
                else
                {
                    ShadowBlurCoeffTextBox.Text = Convert.ToString(Math.Round(ShadowBlurCoeffSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите целое значение от 0 до 50.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ShadowBlurCoeffTextBox_LostFocus");
            }
        }

        private void GammaGammaTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                double gamma;
                bool isDouble = Double.TryParse(GammaGammaTextBox.Text.Replace(",", "."), out gamma);
                if (isDouble && gamma >= 0 && gamma <= 1)
                {
                    GammaGammaSlider.Value = gamma;
                }
                else
                {
                    GammaGammaTextBox.Text = Convert.ToString(Math.Round(GammaGammaSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите значение от 0 до 1.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GammaGammaTextBox_LostFocus");
            }
        }

        private void FovTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int fov;
                bool isInt = Int32.TryParse(FovTextBox.Text.Replace(",", "."), out fov);
                if (isInt && fov >= 0 && fov <= 180)
                {
                    FovSlider.Value = fov;
                }
                else
                {
                    FovTextBox.Text = Convert.ToString(Math.Round(FovSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите целое значение от 0 до 180.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "FovTextBox_LostFocus");
            }
        }

        private void CamSpeedTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int camSpeed;
                bool isInt = Int32.TryParse(CamSpeedTextBox.Text.Replace(",", "."), out camSpeed);
                if (isInt && camSpeed >= 0 && camSpeed <= 1000)
                {
                    CamSpeedSlider.Value = camSpeed;
                }
                else
                {
                    CamSpeedTextBox.Text = Convert.ToString(Math.Round(CamSpeedSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите целое значение от 0 до 1000.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "CamSpeedTextBox_LostFocus");
            }
        }

        private void MusicVolumeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int musicVolume;
                bool isInt = Int32.TryParse(MusicVolumeTextBox.Text.Replace(",", "."), out musicVolume);
                if (isInt && musicVolume >= 0 && musicVolume <= 100)
                {
                    MusicVolumeSlider.Value = musicVolume;
                }
                else
                {
                    MusicVolumeTextBox.Text = Convert.ToString(Math.Round(MusicVolumeSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите целое значение от 0 до 100.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "MusicVolumeTextBox_LostFocus");
            }
        }

        private void EffectVolumeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int effectVolume;
                bool isInt = Int32.TryParse(EffectVolumeTextBox.Text.Replace(",", "."), out effectVolume);
                if (isInt && effectVolume >= 0 && effectVolume <= 100)
                {
                    EffectVolumeSlider.Value = effectVolume;
                }
                else
                {
                    EffectVolumeTextBox.Text = Convert.ToString(Math.Round(EffectVolumeSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите целое значение от 0 до 100.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "EffectVolumeTextBox_LostFocus");
            }
        }

        private void SpeakVolumeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                int speakVolume;
                bool isInt = Int32.TryParse(SpeakVolumeTextBox.Text.Replace(",", "."), out speakVolume);
                if (isInt && speakVolume >= 0 && speakVolume <= 100)
                {
                    SpeakVolumeSlider.Value = speakVolume;
                }
                else
                {
                    SpeakVolumeTextBox.Text = Convert.ToString(Math.Round(SpeakVolumeSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error(this, "Неверное значение. Введите целое значение от 0 до 100.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "SpeakVolumeTextBox_LostFocus");
            }
        }

        private void DoNotLoadMainmenuLevelCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DoNotLoadMainmenuLevelCheckBox.IsChecked != null && DoNotLoadMainmenuLevelCheckBox.IsChecked == false)
            {
                MessageBox.Show("Может привести к нестабильности и вылетам. Выключайте на свой страх и риск.");
            }
        }
    }
}
