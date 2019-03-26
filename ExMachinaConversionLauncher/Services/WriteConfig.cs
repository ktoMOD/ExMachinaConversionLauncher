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
                var wndFontSize = 7;
                var micAndTooltipFontSize = 8;

                var uischema2_hd = File.ReadAllText(Directory.GetCurrentDirectory() + @"\data\if\frames\uischema2_hd.xml");
                //foreach (var parametr in data)
                //{
                //    var startIndex = uischema2_hd.IndexOf(parametr.Key, StringComparison.InvariantCulture) + parametr.Key.Length;
                //    var endIndex = uischema2_hd.IndexOf("</", startIndex, StringComparison.InvariantCulture);

                //    var uischema2_hdStringBuilder = new StringBuilder(uischema2_hd);
                //    uischema2_hdStringBuilder.Remove(startIndex, endIndex - startIndex);
                //    uischema2_hdStringBuilder.Insert(startIndex, parametr.Value);
                //    uischema2_hd = uischema2_hdStringBuilder.ToString();
                //}
                File.WriteAllText(Directory.GetCurrentDirectory() + @"\data\if\frames\uischema2_hd.xml", uischema2_hd);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "UpdateUiSchema2Hd");
            }
        }
    }
}
