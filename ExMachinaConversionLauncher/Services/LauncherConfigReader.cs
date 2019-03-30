using ExMachinaConversionLauncher.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace ExMachinaConversionLauncher.Services
{
    public class LauncherConfigReader
    {
        internal string Version { get; set; }
        internal string FullScreen { get; set; }
        internal string Uri { get; set; }
        internal List<GameModel> Games { get; } = new List<GameModel>();
        internal List<ResolutionModel> Resolutions { get; } = new List<ResolutionModel>();
        internal List<FontScaleParamForHdModel> FontScaleParamsForHd { get; } = new List<FontScaleParamForHdModel>();
        internal bool Console { get; set; }
        internal bool AdvancedGraphic { get; set; }
        internal string ExeName { get; set; }
        internal string LastLaunchGame { get; set; }
        internal string LastLaunchMode { get; set; }
        internal string LastLaunchHdMode { get; set; }
        private List<string> GeneralParameterNames { get; } = new List<string>();
        private List<string> UiParameterNames { get; } = new List<string>();
        private readonly ErrorHandler _errorHandler;


        public LauncherConfigReader(string uri, ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            Uri = uri;
        }

        public void GetGamesFromConfig(XmlDataDocument xmlDoc)
        {
            try
            {
                var xmlNode = xmlDoc.GetElementsByTagName("Games");
                if (xmlNode.Count == 0)
                {
                    throw new Exception("Launcher.config tag <Games> could not be empty.");
                }

                var childNodes = xmlNode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    var xmlAttributeCollection = childNodes[i].Attributes;
                    if (xmlAttributeCollection == null) continue;

                    var name = xmlAttributeCollection["Name"];
                    var picturePath = xmlAttributeCollection["PicturePath"];
                    var configPath = xmlAttributeCollection["ConfigPath"];
                    var description = xmlAttributeCollection["Description"];
                    if (name != null && !string.IsNullOrEmpty(name.Value))
                    {
                        Games.Add(new GameModel(name.Value, picturePath.Value, configPath.Value, description.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "LauncherConfigReader > GetGamesFromConfig");
            }
        }

        public void GetResolutionsFromConfig(XmlDataDocument xmlDoc)
        {
            try
            {
                var xmlNode = xmlDoc.GetElementsByTagName("Resolutions");
                if (xmlNode.Count == 0)
                {
                    throw new Exception("Launcher.config tag <Resolutions> could not be empty.");
                }

                var childNodes = xmlNode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    var xmlAttributeCollection = childNodes[i].Attributes;
                    if (xmlAttributeCollection == null) continue;

                    var width = xmlAttributeCollection["Width"];
                    var height = xmlAttributeCollection["Height"];
                    if (width != null && !string.IsNullOrEmpty(width.Value) && height != null && !string.IsNullOrEmpty(height.Value))
                    {
                        Resolutions.Add(new ResolutionModel(Convert.ToInt32(width.Value), Convert.ToInt32(height.Value)));
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "LauncherConfigReader > GetResolutionsFromConfig");
            }
        }

        public void GetFontScaleParamsForHdFromConfig(XmlDataDocument xmlDoc)
        {
            try
            {
                var xmlNode = xmlDoc.GetElementsByTagName("FontScaleParamsForHD");
                if (xmlNode.Count == 0)
                {
                    throw new Exception("Launcher.config tag <FontScaleParamsForHD> could not be empty.");
                }

                var childNodes = xmlNode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    var xmlAttributeCollection = childNodes[i].Attributes;
                    if (xmlAttributeCollection == null) continue;

                    var scaleFactor = xmlAttributeCollection["ScaleFactor"];
                    var wndFontSize = xmlAttributeCollection["WndFontSize"];
                    var micAndTooltipFontSize = xmlAttributeCollection["MicAndTooltipFontSize"];
                    if (scaleFactor != null && !string.IsNullOrEmpty(scaleFactor.Value) &&
                        wndFontSize != null && !string.IsNullOrEmpty(wndFontSize.Value) &&
                        micAndTooltipFontSize != null && !string.IsNullOrEmpty(micAndTooltipFontSize.Value))
                    {
                        FontScaleParamsForHd.Add(new FontScaleParamForHdModel(
                            double.Parse(scaleFactor.Value, CultureInfo.InvariantCulture),
                            int.Parse(wndFontSize.Value, CultureInfo.InvariantCulture),
                            int.Parse(micAndTooltipFontSize.Value, CultureInfo.InvariantCulture)));
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "LauncherConfigReader > GetFontScaleParamsForHDFromConfig");
            }
        }

        public void GetOtherParamsFromConfig(XmlDataDocument xmlDoc)
        {
            try
            {
                var xmlNode = xmlDoc.GetElementsByTagName("OtherParams");
                if (xmlNode.Count == 0)
                {
                    throw new Exception("Launcher.config tag <OtherParams> could not be empty.");
                }

                var childNodes = xmlNode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    var xmlAttributeCollection = childNodes[i].Attributes;

                    if (xmlAttributeCollection == null) continue;

                    var name = xmlAttributeCollection["Name"];
                    var value = xmlAttributeCollection["Value"];

                    if (name == null || string.IsNullOrEmpty(name.Value) || value == null || string.IsNullOrEmpty(value.Value)) continue;

                    switch (name.Value)
                    {
                        case "exeName":
                            ExeName = value.Value;
                            break;
                        case "lastLaunchGame":
                            LastLaunchGame = value.Value;
                            break;
                        case "conVersion":
                            Version = value.Value;
                            break;
                        case "fullScreen":
                            FullScreen = value.Value;
                            break;
                        case "lastLaunchMode":
                            LastLaunchMode = value.Value;
                            break;
                        case "launcherHDMode":
                            LastLaunchHdMode = value.Value;
                            break;
                        case "console":
                            Console = ToolsService.BooleanValue(value.Value);
                            break;
                        case "advancedGraphic":
                            AdvancedGraphic = ToolsService.BooleanValue(value.Value);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "LauncherConfigReader > GetOtherParamsFromConfig");
            }
        }

        public void GetArrayOfParametersFromConfig(XmlDataDocument xmlDoc, string nodeName, List<string> parametersResultArray)
        {
            try
            {
                var xmlNode = xmlDoc.GetElementsByTagName(nodeName);
                if (xmlNode.Count == 0)
                {
                    throw new Exception($"Launcher.config tag <{nodeName}> could not be empty.");
                }

                var childNodes = xmlNode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    parametersResultArray.Add(childNodes[i].InnerText);
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "LauncherConfigReader > GetArrayOfParametersFromConfig");
            }
        }

        public void GetDataFromFile()
        {
            try
            {
                var xmlDoc = new XmlDataDocument();
                var fs = new FileStream(Uri, FileMode.Open, FileAccess.Read);
                xmlDoc.Load(fs);

                GetGamesFromConfig(xmlDoc);
                GetResolutionsFromConfig(xmlDoc);
                GetFontScaleParamsForHdFromConfig(xmlDoc);
                GetOtherParamsFromConfig(xmlDoc);
                GetArrayOfParametersFromConfig(xmlDoc, "GeneralParameters", GeneralParameterNames);
                GetArrayOfParametersFromConfig(xmlDoc, "UiParameters", UiParameterNames);

                var gameConfigsReader = new GameConfigsReader(_errorHandler, GeneralParameterNames, UiParameterNames);

                foreach (var game in Games)
                {
                    var pathToFile = Directory.GetCurrentDirectory() + @"\LauncherConfig\" + game.ConfigPath;
                    game.GameConfigs = gameConfigsReader.GetGameConfigs(pathToFile);
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "LauncherConfigReader > GetDataFromFile");
            }
        }
    }
}
