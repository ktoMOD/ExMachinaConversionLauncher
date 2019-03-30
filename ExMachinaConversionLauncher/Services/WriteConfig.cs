using ExMachinaConversionLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ExMachinaConversionLauncher.Services
{
    internal class WriteConfig
    {
        private readonly ErrorHandler _errorHandler;

        public WriteConfig(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }

        internal void UpdateGameConfigWithParameters(IEnumerable<GameConfigParameterModel> parameters)
        {
            var gameConfig = File.ReadAllText(Directory.GetCurrentDirectory() + @"\data\config.cfg");
            foreach (var parameter in parameters)
            {
                var begin = gameConfig.IndexOf(parameter.Name, StringComparison.InvariantCulture);
                var end = gameConfig.IndexOf("\"", begin + parameter.Name.Length + 2, StringComparison.InvariantCulture);

                var gameConfigStringBuilder = new StringBuilder(gameConfig);
                gameConfigStringBuilder.Remove(begin, end - begin + 1);
                gameConfigStringBuilder.Insert(begin,
                    $"{parameter.Name}=\"{parameter.Value}\"");
                gameConfig = gameConfigStringBuilder.ToString();
            }
            File.WriteAllText(Directory.GetCurrentDirectory() + @"\data\config.cfg", gameConfig);
        }

        internal void WriteConfigBySelectionGame(GameModel game, string hdMode)
        {
            try
            {
                List<GameConfigParameterModel> uiParameters;
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


                var generalParameters = game.GameConfigs.GeneralParameters;
                var mergedParameters = generalParameters.Concat(uiParameters ?? throw new InvalidOperationException());
                UpdateGameConfigWithParameters(mergedParameters);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "WriteConfigBySelectionGame");
            }
        }

        internal void UpdateLauncherConfig(Dictionary<string, string> keyValuePairs)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(Directory.GetCurrentDirectory() + @"\LauncherConfig\Launcher.config");

                foreach (var keyValuePair in keyValuePairs)
                {
                    var nodes = doc.SelectNodes($"/Configuration/OtherParams/Value[@Name=\"{keyValuePair.Key}\"]");
                    if (nodes == null || nodes.Count == 0)
                    {
                        throw new Exception($"Launcher.config is corrupted. /Configuration/OtherParams/Value[@Name=\"{keyValuePair.Key}\"] was not found.");
                    }
                    foreach (XmlElement n in nodes)
                    {
                        n.SetAttribute("Value", keyValuePair.Value);
                    }
                }


                var settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                    NewLineOnAttributes = true,
                    NewLineHandling = NewLineHandling.None
                };
                using (var writer = XmlWriter.Create(Directory.GetCurrentDirectory() + @"\LauncherConfig\Launcher.config", settings))
                {
                    doc.Save(writer);
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "UpdateUiSchema2Hd");
            }
        }

        internal void UpdateUiSchema2Hd(double scaleValue, List<FontScaleParamForHdModel> paramsArray)
        {
            try
            {
                var fontScaleParamForHdModel = paramsArray.FirstOrDefault(x => Math.Abs(x.ScaleFactor - scaleValue) < 0.01);
                var wndFontSize = fontScaleParamForHdModel?.WndFontSize ?? 7;
                var micAndTooltipFontSize = fontScaleParamForHdModel?.MicAndTooltipFontSize ?? 8;

                var uischema2Hd = File.ReadAllText(Directory.GetCurrentDirectory() + @"\data\if\frames\uischema2_hd.xml");
                uischema2Hd = Regex.Replace(uischema2Hd, "wndFontSize=\"(\\d*)\"", $"wndFontSize=\"{wndFontSize}\"");
                uischema2Hd = Regex.Replace(uischema2Hd, "tooltipFontSize=\"(\\d*)\"", $"tooltipFontSize=\"{micAndTooltipFontSize}\"");
                uischema2Hd = Regex.Replace(uischema2Hd, "miscFontSize=\"(\\d*)\"", $"miscFontSize=\"{micAndTooltipFontSize}\"");
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\data\if\frames\uischema2_hd.xml", uischema2Hd);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "UpdateUiSchema2Hd");
            }
        }
    }
}
