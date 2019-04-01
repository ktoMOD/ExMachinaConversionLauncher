using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExMachinaConversionLauncher.Models;
using ExMachinaConversionLauncher.Services;

namespace ExMachinaConversionLauncher
{
    public partial class Settings : Window
    {
        private readonly LauncherConfigReader _launcherConfigReader;
        private readonly UserSettingsService _userSettingsService;
        private readonly AdvancedGraphicConfigReader _advancedGraphicSettingsService;
        private readonly List<AdvancedGraphicSettingModel> _advancedGraphicSettingsModels;
        private readonly string _pathToMainDirectory = ((App)Application.Current).PathToMainDirectory;
        private readonly SettingsService _settingsService = ((App)Application.Current).SettingsService;
        private readonly ToolsService _toolsService;
        private readonly ErrorHandler _errorHandler;

        public Settings(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            _toolsService = new ToolsService(_errorHandler);
            try
            {
                _userSettingsService = new UserSettingsService(_errorHandler);
                _launcherConfigReader = new LauncherConfigReader($@"{_pathToMainDirectory}\LauncherConfig\Launcher.config", _errorHandler);
                _launcherConfigReader.GetDataFromFile();
                InitializeComponent();
                _userSettingsService.GetDataFromFile();
                InitializeSettings();

                _advancedGraphicSettingsService = new AdvancedGraphicConfigReader($@"{_pathToMainDirectory}\LauncherConfig\AdvancedGraphicSettings.config", _errorHandler);
                _advancedGraphicSettingsModels = _advancedGraphicSettingsService.GetDataFromFile();
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

                ViewDistanceTextBox.Text = Convert.ToString(_userSettingsService.LsViewDistanceDivider, CultureInfo.InvariantCulture);
                ViewDistanceSlider.Value = _userSettingsService.LsViewDistanceDivider;
                ProjectorsFarDistTextBox.Text = Convert.ToString(_userSettingsService.ProjectorsFarDist, CultureInfo.InvariantCulture);
                ProjectorsFarDistSlider.Value = _userSettingsService.ProjectorsFarDist;
                GrassDrawDistTextBox.Text = Convert.ToString(_userSettingsService.GrassDrawDist, CultureInfo.InvariantCulture);
                GrassDrawDistSlider.Value = _userSettingsService.GrassDrawDist;
                ShadowBlurCoeffTextBox.Text = Convert.ToString(_userSettingsService.ShadowBlurCoeff, CultureInfo.InvariantCulture);
                ShadowBlurCoeffSlider.Value = _userSettingsService.ShadowBlurCoeff;
                GammaGammaTextBox.Text = Convert.ToString(_userSettingsService.Gamma, CultureInfo.InvariantCulture);
                GammaGammaSlider.Value = _userSettingsService.Gamma;
                FovTextBox.Text = Convert.ToString(_userSettingsService.Fov, CultureInfo.InvariantCulture);
                FovSlider.Value = _userSettingsService.Fov;
                CamSpeedTextBox.Text = Convert.ToString(_userSettingsService.CamSpeed, CultureInfo.InvariantCulture);
                CamSpeedSlider.Value = _userSettingsService.CamSpeed;
                MusicVolumeTextBox.Text = Convert.ToString(_userSettingsService.Volume, CultureInfo.InvariantCulture);
                MusicVolumeSlider.Value = _userSettingsService.Volume;
                EffectVolumeTextBox.Text = Convert.ToString(_userSettingsService.Volume3D, CultureInfo.InvariantCulture);
                EffectVolumeSlider.Value = _userSettingsService.Volume3D;
                SpeakVolumeTextBox.Text = Convert.ToString(_userSettingsService.Volume2D, CultureInfo.InvariantCulture);
                SpeakVolumeSlider.Value = _userSettingsService.Volume2D;

                #endregion
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "InitializeSettings - sliders");
            }

            try
            {
                #region screenResolution

                var resolutionsCollection = _launcherConfigReader.Resolutions;
                ResolutionComboBox.ItemsSource = resolutionsCollection.Select(x => $"{x.Width}×{x.Height}");
                ResolutionComboBox.SelectedItem = _userSettingsService.Width + "×" + _userSettingsService.Height;

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
                "Овальный",
                "Реалистичный",
            };
                SightComboBox.ItemsSource = sightCollection;
                string sightQualityInConfig;
                switch (_launcherConfigReader.LastLaunchHdMode)
                {
                    case "WithHDWithSmallSight":
                        sightQualityInConfig = "Маленький";
                        break;
                    case "WithHDWithOvalSight":
                        sightQualityInConfig = "Овальный";
                        break;
                    case "WithHDWithHardcoreSight":
                        sightQualityInConfig = "Реалистичный";
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
                var waterQualityInConfig = String.Empty;
                switch (_userSettingsService.WaterQuality)
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
                var shadowsQualityForSelect = String.Empty;
                var shadowsQualityInConfig = Convert.ToString(_userSettingsService.DsShadows, CultureInfo.InvariantCulture) + "_" +
                                                Convert.ToString(_userSettingsService.DetShadowTexSz, CultureInfo.InvariantCulture) + "_" +
                                                Convert.ToString(_userSettingsService.LgtShadowTexSz, CultureInfo.InvariantCulture);


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
                var postEffectBloomInConfig = String.Empty;
                switch (_userSettingsService.PostEffectBloom)
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
                var smoothingInConfig = String.Empty;
                switch (_userSettingsService.MultiSamplesNum)
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
                var texturesFilterForSelect = String.Empty;
                var texturesFilterInConfig = Convert.ToString(_userSettingsService.TexturesFilter, CultureInfo.InvariantCulture) + "_" +
                                                Convert.ToString(_userSettingsService.ShaderMacro1, CultureInfo.InvariantCulture);


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

                AutoPlayVideoCheckBox.IsChecked = _userSettingsService.AutoPlayVideo;
                DoNotLoadMainmenuLevelCheckBox.IsChecked = !_userSettingsService.DoNotLoadMainmenuLevel;
                SwitchCameraAllowCheckBox.IsChecked = _userSettingsService.SwitchCameraAllow;
                AdvancedGraphicSettingsCheckBox.IsChecked = _launcherConfigReader.AdvancedGraphic;

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
                var resolution = ResolutionComboBox.SelectedItem.ToString().Split('×');
                _userSettingsService.DesiredHeight = Convert.ToInt32(resolution[1], CultureInfo.InvariantCulture);
                _userSettingsService.DesiredWidth = Convert.ToInt32(resolution[0], CultureInfo.InvariantCulture);
                _userSettingsService.Height = Convert.ToInt32(resolution[1], CultureInfo.InvariantCulture);
                _userSettingsService.Width = Convert.ToInt32(resolution[0], CultureInfo.InvariantCulture);
                _userSettingsService.LsViewDistanceDivider = ViewDistanceSlider.Value;
                _userSettingsService.ProjectorsFarDist = Convert.ToInt32(ProjectorsFarDistSlider.Value, CultureInfo.InvariantCulture);
                _userSettingsService.GrassDrawDist = GrassDrawDistSlider.Value;
                switch ((string)WaterQualityComboBox.SelectedItem)
                {
                    case "Низкое":
                        _userSettingsService.WaterQuality = 1;
                        break;
                    case "Среднее":
                        _userSettingsService.WaterQuality = 2;
                        break;
                    case "Высокое":
                        _userSettingsService.WaterQuality = 3;
                        break;
                }
                switch ((string)ShadowsQualityComboBox.SelectedItem)
                {
                    case "Низкое":
                        _userSettingsService.DsShadows = false;
                        _userSettingsService.DetShadowTexSz = 512;
                        _userSettingsService.LgtShadowTexSz = 256;
                        break;
                    case "Среднее":
                        _userSettingsService.DsShadows = true;
                        _userSettingsService.DetShadowTexSz = 512;
                        _userSettingsService.LgtShadowTexSz = 256;
                        break;
                    case "Высокое":
                        _userSettingsService.DsShadows = true;
                        _userSettingsService.DetShadowTexSz = 1024;
                        _userSettingsService.LgtShadowTexSz = 512;
                        break;
                }
                _userSettingsService.ShadowBlurCoeff = ShadowBlurCoeffSlider.Value;
                switch ((string)PostEffectBloomComboBox.SelectedItem)
                {
                    case "Низкое":
                        _userSettingsService.PostEffectBloom = 0;
                        break;
                    case "Среднее":
                        _userSettingsService.PostEffectBloom = 1;
                        break;
                    case "Высокое":
                        _userSettingsService.PostEffectBloom = 2;
                        break;
                }
                switch ((string)SmoothingComboBox.SelectedItem)
                {
                    case "Отключено":
                        _userSettingsService.MultiSamplesNum = 0;
                        break;
                    case "×2":
                        _userSettingsService.MultiSamplesNum = 2;
                        break;
                    case "×4":
                        _userSettingsService.MultiSamplesNum = 4;
                        break;
                    case "×8":
                        _userSettingsService.MultiSamplesNum = 8;
                        break;
                }
                switch ((string)TexturesFilterComboBox.SelectedItem)
                {
                    case "Билинейная":
                        _userSettingsService.TexturesFilter = 4;
                        _userSettingsService.ShaderMacro1 = "MIP_FILTER Point";
                        break;
                    case "Трилинейная":
                        _userSettingsService.TexturesFilter = 5;
                        _userSettingsService.ShaderMacro1 = "MIP_FILTER Linear";
                        break;
                    case "Анизотропная":
                        _userSettingsService.TexturesFilter = 3;
                        _userSettingsService.ShaderMacro1 = "MIP_FILTER Linear";
                        break;
                }
                _userSettingsService.Gamma = GammaGammaSlider.Value;
                if (AutoPlayVideoCheckBox.IsChecked != null) _userSettingsService.AutoPlayVideo = (bool)AutoPlayVideoCheckBox.IsChecked;
                if (DoNotLoadMainmenuLevelCheckBox.IsChecked != null) _userSettingsService.DoNotLoadMainmenuLevel = (bool)!DoNotLoadMainmenuLevelCheckBox.IsChecked;
                _userSettingsService.Fov = FovSlider.Value;
                if (SwitchCameraAllowCheckBox.IsChecked != null) _userSettingsService.SwitchCameraAllow = (bool)SwitchCameraAllowCheckBox.IsChecked;
                if (AdvancedGraphicSettingsCheckBox.IsChecked != null) _launcherConfigReader.AdvancedGraphic = (bool)AdvancedGraphicSettingsCheckBox.IsChecked;
                _userSettingsService.CamSpeed = Convert.ToInt32(CamSpeedSlider.Value, CultureInfo.InvariantCulture);
                _userSettingsService.Volume = Convert.ToInt32(MusicVolumeSlider.Value, CultureInfo.InvariantCulture);
                _userSettingsService.Volume3D = Convert.ToInt32(EffectVolumeSlider.Value, CultureInfo.InvariantCulture);
                _userSettingsService.Volume2D = Convert.ToInt32(SpeakVolumeSlider.Value, CultureInfo.InvariantCulture);


                Dictionary<string, string> launcherParams;
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
                        case "Реалистичный":
                            sightQualityToConfig = "WithHDWithHardcoreSight";
                            break;
                        default:
                            sightQualityToConfig = "WithHDWithDefaultSight";
                            break;
                    }

                    launcherParams = new Dictionary<string, string>
                    {
                        {"launcherHDMode", sightQualityToConfig},
                        {"advancedGraphic", AdvancedGraphicSettingsCheckBox.IsChecked.ToString().ToLower()}
                    };
                }
                else
                {
                    launcherParams = new Dictionary<string, string>
                    {
                        {"launcherHDMode", "WithOutHD"},
                        {"advancedGraphic", AdvancedGraphicSettingsCheckBox.IsChecked.ToString().ToLower()}
                    };
                }

                //Fill part start
                _settingsService.AddParamsToLauncherParams(launcherParams);

                var settingsParameters = _toolsService.PrepareSettingsParameters(_userSettingsService);
                var advancedGraphicSettings = _advancedGraphicSettingsService.ConvertAdvancedGraphicSettingsListToDictionary(_advancedGraphicSettingsModels, AdvancedGraphicSettingsCheckBox.IsChecked.GetValueOrDefault());
                var mergedSettings = _toolsService.ConcatTwoDictionariesWithoutDuplicates(advancedGraphicSettings, settingsParameters);
                _settingsService.AddParamsToGameParams(mergedSettings);
                //Fill part end

                //Save part start
                _settingsService.SaveGameParams();
                _settingsService.SaveLauncherParams();
                //Save part end

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
                var height = Convert.ToInt32(resolution[1], CultureInfo.InvariantCulture);
                var width = Convert.ToInt32(resolution[0], CultureInfo.InvariantCulture);
                var ratio = (double)width / (double)height;

                if (Math.Abs(ratio - 16.0 / 9.0) < 0.01 || Math.Abs(ratio - 16.0 / 10.0) < 0.01)
                {
                    if (_launcherConfigReader.LastLaunchHdMode == "WithOutHD" && e.AddedItems.Count != e.RemovedItems.Count)
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
                var isDouble = double.TryParse(ViewDistanceTextBox.Text.Replace(",", "."), out var viewDistance);
                if (isDouble && viewDistance >= 0 && viewDistance <= 1)
                {
                    ViewDistanceSlider.Value = viewDistance;
                }
                else
                {
                    ViewDistanceTextBox.Text = Convert.ToString(Math.Round(ViewDistanceSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите значение от 0 до 1.", false);
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
                var isInt = int.TryParse(ProjectorsFarDistTextBox.Text.Replace(",", "."), out var projectorsFarDistSlider);
                if (isInt && projectorsFarDistSlider >= 0 && projectorsFarDistSlider <= 5)
                {
                    ProjectorsFarDistSlider.Value = projectorsFarDistSlider;
                }
                else
                {
                    ProjectorsFarDistTextBox.Text = Convert.ToString(Math.Round(ProjectorsFarDistSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите целое значение от 0 до 5.", false);
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
                var isInt = int.TryParse(GrassDrawDistTextBox.Text.Replace(",", "."), out var grassDrawDistance);
                if (isInt && grassDrawDistance >= 0 && grassDrawDistance <= 350)
                {
                    GrassDrawDistSlider.Value = grassDrawDistance;
                }
                else
                {
                    GrassDrawDistTextBox.Text = Convert.ToString(Math.Round(GrassDrawDistSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите целое значение от 0 до 350.", false);
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
                var isInt = int.TryParse(ShadowBlurCoeffTextBox.Text.Replace(",", "."), out var shadowBlurCoeff);
                if (isInt && shadowBlurCoeff >= 0 && shadowBlurCoeff <= 50)
                {
                    ShadowBlurCoeffSlider.Value = shadowBlurCoeff;
                }
                else
                {
                    ShadowBlurCoeffTextBox.Text = Convert.ToString(Math.Round(ShadowBlurCoeffSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите целое значение от 0 до 50.", false);
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
                var isDouble = double.TryParse(GammaGammaTextBox.Text.Replace(",", "."), out var gamma);
                if (isDouble && gamma >= 0 && gamma <= 1)
                {
                    GammaGammaSlider.Value = gamma;
                }
                else
                {
                    GammaGammaTextBox.Text = Convert.ToString(Math.Round(GammaGammaSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите значение от 0 до 1.", false);
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
                var isInt = int.TryParse(FovTextBox.Text.Replace(",", "."), out var fov);
                if (isInt && fov >= 0 && fov <= 180)
                {
                    FovSlider.Value = fov;
                }
                else
                {
                    FovTextBox.Text = Convert.ToString(Math.Round(FovSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите целое значение от 0 до 180.", false);
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
                var isInt = int.TryParse(CamSpeedTextBox.Text.Replace(",", "."), out var camSpeed);
                if (isInt && camSpeed >= 0 && camSpeed <= 1000)
                {
                    CamSpeedSlider.Value = camSpeed;
                }
                else
                {
                    CamSpeedTextBox.Text = Convert.ToString(Math.Round(CamSpeedSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите целое значение от 0 до 1000.", false);
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
                var isInt = int.TryParse(MusicVolumeTextBox.Text.Replace(",", "."), out var musicVolume);
                if (isInt && musicVolume >= 0 && musicVolume <= 100)
                {
                    MusicVolumeSlider.Value = musicVolume;
                }
                else
                {
                    MusicVolumeTextBox.Text = Convert.ToString(Math.Round(MusicVolumeSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите целое значение от 0 до 100.", false);
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
                var isInt = int.TryParse(EffectVolumeTextBox.Text.Replace(",", "."), out var effectVolume);
                if (isInt && effectVolume >= 0 && effectVolume <= 100)
                {
                    EffectVolumeSlider.Value = effectVolume;
                }
                else
                {
                    EffectVolumeTextBox.Text = Convert.ToString(Math.Round(EffectVolumeSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите целое значение от 0 до 100.", false);
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
                var isInt = int.TryParse(SpeakVolumeTextBox.Text.Replace(",", "."), out var speakVolume);
                if (isInt && speakVolume >= 0 && speakVolume <= 100)
                {
                    SpeakVolumeSlider.Value = speakVolume;
                }
                else
                {
                    SpeakVolumeTextBox.Text = Convert.ToString(Math.Round(SpeakVolumeSlider.Value, 2), CultureInfo.InvariantCulture);
                    var errorWindow = new Error("Неверное значение. Введите целое значение от 0 до 100.", false);
                    errorWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "SpeakVolumeTextBox_LostFocus");
            }
        }
    }
}
