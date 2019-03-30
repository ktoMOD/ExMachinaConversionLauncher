using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using ExMachinaConversionLauncher.Models;

namespace ExMachinaConversionLauncher.Services
{
    class ToolsService
    {
        private readonly ErrorHandler _errorHandler;

        public ToolsService(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        public bool BooleanValue(string value)
        {
            switch (value.ToLower())
            {
                case "yes":
                case "true":
                case "1":
                    return true;
                default:
                    return false;
            }
        }

        public Dictionary<TKey, TValue> ConcatTwoDictionariesWithoutDuplicates<TKey, TValue>(Dictionary<TKey, TValue> first, Dictionary<TKey, TValue> second)
        {
            var mergedSettings = new Dictionary<TKey, TValue>(first);
            foreach (var keyValue in second)
            {
                mergedSettings[keyValue.Key] = keyValue.Value;
            }

            return mergedSettings;
        }

        public Dictionary<string, string> SelectAndPrepareGameParams(GameModel game, string hdMode)
        {
            try
            {
                Dictionary<string, string> uiParameters;
                switch (hdMode)
                {
                    case "WithHDWithDefaultSight":
                    case "WithHDWithSmallSight":
                    case "WithHDWithHardcoreSight":
                    case "WithHDWithOvalSight":
                        uiParameters = game.GameConfigs.UiParameters.FirstOrDefault(x => x.Name == hdMode)?.UiParameters;
                        break;
                    default:
                        uiParameters = game.GameConfigs.UiParameters.FirstOrDefault(x => x.Name == "WithOutHD")?.UiParameters;
                        break;
                }

                var mergedParameters = ConcatTwoDictionariesWithoutDuplicates(game.GameConfigs.GeneralParameters, uiParameters);
                return mergedParameters;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "WriteConfig > WriteConfigBySelectionGame");
                return null;
            }
        }

        public Dictionary<string, int> PrepareUiSchema2HdParams(Window window, List<FontScaleParamForHdModel> paramsArray)
        {
            try
            {

                var source = PresentationSource.FromVisual(window);
                double scaleValue = 1;
                if (source?.CompositionTarget != null)
                {
                    scaleValue = source.CompositionTarget.TransformToDevice.M11;
                }

                var fontScaleParamForHdModel = paramsArray.FirstOrDefault(x => Math.Abs(x.ScaleFactor - scaleValue) < 0.01);
                var wndFontSize = fontScaleParamForHdModel?.WndFontSize ?? 7;
                var micAndTooltipFontSize = fontScaleParamForHdModel?.MicAndTooltipFontSize ?? 8;

                var result = new Dictionary<string, int>
                {
                    {"wndFontSize", wndFontSize},
                    {"tooltipFontSize", micAndTooltipFontSize},
                    {"miscFontSize", micAndTooltipFontSize}
                };
                return result;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "WriteConfig > UpdateUiSchema2Hd");
                return null;
            }
        }

        public Dictionary<string, string> PrepareSettingsParameters(UserSettingsService userSettingsService)
        {
            try
            {
                var settingsParameters = new Dictionary<string, string>()
                {
                    {"lsViewDistanceDivider",String.Format(CultureInfo.InvariantCulture, "{0:N2}", userSettingsService.LsViewDistanceDivider)},
                    {"r_multiSamplesNum",Convert.ToString(userSettingsService.MultiSamplesNum, CultureInfo.InvariantCulture)},
                    {"g_postEffectBloom",Convert.ToString(userSettingsService.PostEffectBloom, CultureInfo.InvariantCulture)},
                    {"g_texturesFilter",Convert.ToString(userSettingsService.TexturesFilter, CultureInfo.InvariantCulture)},
                    {"shaderMacro1",userSettingsService.ShaderMacro1},
                    {"dsShadows",Convert.ToString(userSettingsService.DsShadows, CultureInfo.InvariantCulture).ToLower()},
                    {"detShadowTexSz",Convert.ToString(userSettingsService.DetShadowTexSz, CultureInfo.InvariantCulture)},
                    {"g_shadowBlurCoeff",String.Format(CultureInfo.InvariantCulture, "{0:N2}", userSettingsService.ShadowBlurCoeff)},
                    {"lgtShadowTexSz",Convert.ToString(userSettingsService.LgtShadowTexSz, CultureInfo.InvariantCulture)},
                    {"g_grassDrawDist",String.Format(CultureInfo.InvariantCulture, "{0:N2}", userSettingsService.GrassDrawDist)},
                    {"r_waterQuality",Convert.ToString(userSettingsService.WaterQuality, CultureInfo.InvariantCulture)},
                    {"gammaGamma",String.Format(CultureInfo.InvariantCulture, "{0:N2}", userSettingsService.Gamma)},
                    {"r_desiredHeight",Convert.ToString(userSettingsService.DesiredHeight, CultureInfo.InvariantCulture)},
                    {"r_desiredWidth",Convert.ToString(userSettingsService.DesiredWidth, CultureInfo.InvariantCulture)},
                    {"r_height",Convert.ToString(userSettingsService.Height, CultureInfo.InvariantCulture)},
                    {"r_width",Convert.ToString(userSettingsService.Width, CultureInfo.InvariantCulture)},
                    {"mus_Volume",Convert.ToString(userSettingsService.Volume, CultureInfo.InvariantCulture)},
                    {"snd_2dVolume",Convert.ToString(userSettingsService.Volume2D, CultureInfo.InvariantCulture)},
                    {"snd_3dVolume",Convert.ToString(userSettingsService.Volume3D, CultureInfo.InvariantCulture)},
                    {"fov",String.Format(CultureInfo.InvariantCulture, "{0:N2}", userSettingsService.Fov)},
                    {"autoPlayVideo",Convert.ToString(userSettingsService.AutoPlayVideo, CultureInfo.InvariantCulture).ToLower()},
                    {"DoNotLoadMainmenuLevel",Convert.ToString(userSettingsService.DoNotLoadMainmenuLevel, CultureInfo.InvariantCulture).ToLower()},
                    {"camSpeed",Convert.ToString(userSettingsService.CamSpeed, CultureInfo.InvariantCulture)},
                    {"g_projectorsFarDist",Convert.ToString(userSettingsService.ProjectorsFarDist, CultureInfo.InvariantCulture)},
                    {"g_switchCameraAllow",Convert.ToString(userSettingsService.SwitchCameraAllow, CultureInfo.InvariantCulture).ToLower()}
                };
                return settingsParameters;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "PrepareSettingsParameters");
                return new Dictionary<string, string>();
            }
        }
    }
}
