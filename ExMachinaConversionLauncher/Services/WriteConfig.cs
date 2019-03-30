using ExMachinaConversionLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ExMachinaConversionLauncher.Services
{
    internal class WriteConfig
    {
        private readonly string _pathToMainDirectory = ((App)Application.Current).PathToMainDirectory;
        private readonly ErrorHandler _errorHandler;

        public WriteConfig(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        internal Dictionary<string, string> WriteConfigBySelectionGame(GameModel game, string hdMode)
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
                        uiParameters = game.GameConfigs.UiParameters.FirstOrDefault(x=>x.Name == "WithOutHD")?.UiParameters;
                        break;
                }

                var mergedParameters = ToolsService.ConcatTwoDictionariesWithoutDuplicates(game.GameConfigs.GeneralParameters, uiParameters);
                return mergedParameters;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "WriteConfig > WriteConfigBySelectionGame");
                return null;
            }
        }

        internal Dictionary<string, int> GetUiSchema2HdParams(Window window, List<FontScaleParamForHdModel> paramsArray)
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
    }
}
