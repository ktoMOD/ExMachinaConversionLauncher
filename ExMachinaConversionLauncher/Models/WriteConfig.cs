using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMachinaConversionLauncher.Models
{
    internal class WriteConfig
    {
        private readonly ErrorHandler _errorHandler;

        public WriteConfig(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }
        internal string WriteConfigBySelectionGame(string gameName, string[] games, List<Dictionary<string, string>> listParametrsDictionary)
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
                return null;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "WriteConfigBySelectionGame");
                return null;
            }

        }

        internal string UpdateLauncherConfig(Dictionary<string, string> data)
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
                return null;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "UpdateLauncherConfig");
                return null;
            }
        }
    }
}
