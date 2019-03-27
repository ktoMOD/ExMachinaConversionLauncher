using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;

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
                    //var value = gameConfig.Substring(begin, end - begin + 1);

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

        internal void UpdateLauncherConfig(Dictionary<string, string> data)
        {
            try
            {
                var launcherConfig = File.ReadAllText(Directory.GetCurrentDirectory() + @"\LauncherConfig\Launcher.config");
                foreach (var parametr in data)
                {
                    var startIndex = launcherConfig.IndexOf(parametr.Key, StringComparison.InvariantCulture) + parametr.Key.Length;
                    var endIndex = launcherConfig.IndexOf("</", startIndex, StringComparison.InvariantCulture);

                    var launcherConfigStringBuilder = new StringBuilder(launcherConfig);
                    launcherConfigStringBuilder.Remove(startIndex, endIndex - startIndex);
                    launcherConfigStringBuilder.Insert(startIndex, parametr.Value);
                    launcherConfig = launcherConfigStringBuilder.ToString();
                }
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\LauncherConfig\Launcher.config", launcherConfig);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "UpdateLauncherConfig");
            }
        }

        internal void UpdateUiSchema2Hd(double scaleValue, List<string> paramsArray)
        {
            try
            {
                var myRegex = new Regex($@"^{scaleValue}\|");
                var valueString = paramsArray.FirstOrDefault(myRegex.IsMatch);
                if (string.IsNullOrEmpty(valueString))
                {
                    return;
                }
                var valueStringArray = valueString.Split('|');
                var wndFontSize = !string.IsNullOrEmpty(valueStringArray[1]) ? valueStringArray[1] : "7";
                var micAndTooltipFontSize = !string.IsNullOrEmpty(valueStringArray[2]) ? valueStringArray[2] : "8";

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
