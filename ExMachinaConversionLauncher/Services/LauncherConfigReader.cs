using ExMachinaConversionLauncher.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Xml;

namespace ExMachinaConversionLauncher.Services
{
    public class LauncherConfigReader
    {
        public string Version { get; set; }
        public string FullScreen { get; set; }
        public string Uri { get; set; }
        public List<GameModel> Games { get; } = new List<GameModel>();
        public List<ResolutionModel> Resolutions { get; } = new List<ResolutionModel>();
        public List<FontScaleParamForHdModel> FontScaleParamsForHd { get; } = new List<FontScaleParamForHdModel>();
        public bool Console { get; set; }
        public bool AdvancedGraphic { get; set; }
        public string ExeName { get; set; }
        public string LastLaunchGame { get; set; }
        public string LastLaunchMode { get; set; }
        public string LastLaunchHdMode { get; set; }
        private List<string> GeneralParameterNames { get; } = new List<string>();
        private List<string> UiParameterNames { get; } = new List<string>();
        private readonly string _pathToMainDirectory = ((App)Application.Current).PathToMainDirectory;
        private readonly ToolsService _toolsService;
        private readonly ErrorHandler _errorHandler;


        public LauncherConfigReader(string uri, ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            Uri = uri;
            _toolsService = new ToolsService(_errorHandler);
        }

        public void GetGamesFromConfig(XmlDocument xDoc)
        {
            try
            {
                var xmlNode = xDoc.GetElementsByTagName("Games");
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

        public void GetResolutionsFromConfig(XmlDocument xDoc)
        {
            try
            {
                var xmlNode = xDoc.GetElementsByTagName("Resolutions");
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

        public void GetFontScaleParamsForHdFromConfig(XmlDocument xDoc)
        {
            try
            {
                var xmlNode = xDoc.GetElementsByTagName("FontScaleParamsForHD");
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

        public void GetOtherParamsFromConfig(XmlDocument xDoc)
        {
            try
            {
                var xmlNode = xDoc.GetElementsByTagName("OtherParams");
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
                            Console = _toolsService.BooleanValue(value.Value);
                            break;
                        case "advancedGraphic":
                            AdvancedGraphic = _toolsService.BooleanValue(value.Value);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "LauncherConfigReader > GetOtherParamsFromConfig");
            }
        }

        public void GetArrayOfParametersFromConfig(XmlDocument xDoc, string nodeName, List<string> parametersResultArray)
        {
            try
            {
                var xmlNode = xDoc.GetElementsByTagName(nodeName);
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
                using (var xmlFile = new FileStream(Uri, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var xDoc = new XmlDocument();
                    xDoc.Load(xmlFile);

                    GetGamesFromConfig(xDoc);
                    GetResolutionsFromConfig(xDoc);
                    GetFontScaleParamsForHdFromConfig(xDoc);
                    GetOtherParamsFromConfig(xDoc);
                    GetArrayOfParametersFromConfig(xDoc, "GeneralParameters", GeneralParameterNames);
                    GetArrayOfParametersFromConfig(xDoc, "UiParameters", UiParameterNames);
                }
                var gameConfigsReader = new GameConfigsReader(_errorHandler, GeneralParameterNames, UiParameterNames);

                foreach (var game in Games)
                {
                    var pathToFile = $@"{_pathToMainDirectory}\LauncherConfig\" + game.ConfigPath;
                    game.GameConfigs = gameConfigsReader.GetGameConfigs(pathToFile);
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "LauncherConfigReader > GetDataFromFile");
            }
        }

        public void RefreshOtherParamsFromConfig()
        {
            try
            {
                using (var src = new DataSet())
                {
                    src.ReadXml(Uri);

                    var xDoc = new XmlDocument();
                    xDoc.LoadXml(src.GetXml());

                    GetOtherParamsFromConfig(xDoc);
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "LauncherConfigReader > RefreshOtherParamsFromConfig");
            }
        }
    }
}
