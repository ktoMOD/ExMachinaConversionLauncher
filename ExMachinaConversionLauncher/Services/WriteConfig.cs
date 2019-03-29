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
        internal void WriteConfigBySelectionGame(string gameName, string[] games, List<Dictionary<string, string>> listParametrsDictionary)
        {
            var index = Array.IndexOf(games, gameName);
            try
            {
                var gameConfig = File.ReadAllText(Directory.GetCurrentDirectory() + @"\data\config.cfg");
                foreach (var configString in listParametrsDictionary[index])
                {
                    int begin, end;
                    begin = gameConfig.IndexOf(configString.Key, StringComparison.InvariantCulture);
                    end = gameConfig.IndexOf("\"", begin + configString.Key.Length + 2, StringComparison.InvariantCulture);

                    var gameConfigStringBuilder = new StringBuilder(gameConfig);
                    gameConfigStringBuilder.Remove(begin, end - begin + 1);
                    gameConfigStringBuilder.Insert(begin,
                        String.Format("{0}=\"{1}\"", configString.Key, configString.Value));
                    gameConfig = gameConfigStringBuilder.ToString();
                }
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\data\config.cfg", gameConfig);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "WriteConfigBySelectionGame");
            }

        }

        internal void UpdateLauncherConfig(Dictionary<string, string> keyValuePairs)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Directory.GetCurrentDirectory() + @"\LauncherConfig\Launcher.config");


            foreach (var keyValuePair in keyValuePairs)
            {
                var nodes = doc.SelectNodes(String.Format("/Configuration/OtherParams/Value[@Name=\"{0}\"]", keyValuePair.Key));
                foreach (XmlElement n in nodes)
                {
                    n.SetAttribute("Value", keyValuePair.Value);
                }
            }

            doc.Save(Directory.GetCurrentDirectory() + @"\LauncherConfig\Launcher.config");
        }

        internal void UpdateUiSchema2Hd(double scaleValue, List<FontScaleParamForHdModel> paramsArray)
        {
            try
            {
                var fontScaleParamForHdModel = paramsArray.FirstOrDefault(x=>x.ScaleFactor == scaleValue);
                var wndFontSize = fontScaleParamForHdModel != null ? fontScaleParamForHdModel.WndFontSize : 7;
                var micAndTooltipFontSize = fontScaleParamForHdModel != null ? fontScaleParamForHdModel.MicAndTooltipFontSize : 8;

                var uischema2_hd = File.ReadAllText(Directory.GetCurrentDirectory() + @"\data\if\frames\uischema2_hd.xml");
                uischema2_hd = Regex.Replace(uischema2_hd, "wndFontSize=\"(\\d*)\"", $"wndFontSize=\"{wndFontSize}\"");
                uischema2_hd = Regex.Replace(uischema2_hd, "tooltipFontSize=\"(\\d*)\"", $"tooltipFontSize=\"{micAndTooltipFontSize}\"");
                uischema2_hd = Regex.Replace(uischema2_hd, "miscFontSize=\"(\\d*)\"", $"miscFontSize=\"{micAndTooltipFontSize}\"");
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\data\if\frames\uischema2_hd.xml", uischema2_hd);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "UpdateUiSchema2Hd");
            }
        }
    }
}
