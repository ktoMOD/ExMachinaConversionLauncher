using ExMachinaConversionLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
//using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExMachinaConversionLauncher.Services
{
    public class ConfigReader
    {
        internal string Version { get; set; }
        internal string FullScreen { get; set; }
        internal string Uri { get; set; }
        internal string[] BaseParametrsNames { get; set; }
        internal string[] HdParametrsNames { get; set; }
        internal List<GameModel> Games { get; } = new List<GameModel>();
        internal List<ResolutionModel> Resolutions { get; } = new List<ResolutionModel>();
        internal List<FontScaleParamForHdModel> FontScaleParamsForHD { get; } = new List<FontScaleParamForHdModel>();
        internal List<Dictionary<string, string>> ListBaseConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithOutHdConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithHdWithDefaultSightConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithHdWithSmallSightConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithHdWithHardcoreSightConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithHdWithOvalSightConfigDictionary { get; set; }
        internal bool Console { get; set; }
        internal bool AdvancedGraphic { get; set; }
        internal string ExeName { get; set; }
        internal string LastLaunchGame { get; set; }
        internal string LastLaunchMode { get; set; }
        internal string LastLaunchHdMode { get; set; }
        private readonly ErrorHandler _errorHandler;
        private readonly ToolsService _toolsService;


        public ConfigReader(string uri, ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            Uri = uri;
            ListBaseConfigDictionary = new List<Dictionary<string, string>>();
            ListWithOutHdConfigDictionary = new List<Dictionary<string, string>>();
            ListWithHdWithDefaultSightConfigDictionary = new List<Dictionary<string, string>>();
            ListWithHdWithSmallSightConfigDictionary = new List<Dictionary<string, string>>();
            ListWithHdWithHardcoreSightConfigDictionary = new List<Dictionary<string, string>>();
            ListWithHdWithOvalSightConfigDictionary = new List<Dictionary<string, string>>();
            _toolsService = new ToolsService();
        }

        public void GetGamesFromConfig(XmlDataDocument xmlDoc)
        {
            try
            {
                var xmlnode = xmlDoc.GetElementsByTagName("Games");
                if (xmlnode.Count == 0)
                {
                    throw new Exception("Launcher.config tag <Games> could not be empty.");
                }

                var childNodes = xmlnode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    var name = childNodes[i].Attributes["Name"];
                    var picturePath = childNodes[i].Attributes["PicturePath"];
                    var configPath = childNodes[i].Attributes["ConfigPath"];
                    var description = childNodes[i].Attributes["Description"];
                    if (name != null && !string.IsNullOrEmpty(name.Value))
                    {
                        Games.Add(new GameModel(name.Value, picturePath.Value, configPath.Value, description.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ConfigReader > GetGamesFromConfig");
            }
        }

        public void GetResolutionsFromConfig(XmlDataDocument xmlDoc)
        {
            try
            {
                var xmlnode = xmlDoc.GetElementsByTagName("Resolutions");
                if (xmlnode.Count == 0)
                {
                    throw new Exception("Launcher.config tag <Resolutions> could not be empty.");
                }

                var childNodes = xmlnode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    var width = childNodes[i].Attributes["Width"];
                    var heigth = childNodes[i].Attributes["Height"];
                    if (width != null && !string.IsNullOrEmpty(width.Value) && heigth != null && !string.IsNullOrEmpty(heigth.Value))
                    {
                        Resolutions.Add(new ResolutionModel(Convert.ToInt32(width.Value), Convert.ToInt32(heigth.Value)));
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ConfigReader > GetResolutionsFromConfig");
            }
        }

        public void GetFontScaleParamsForHDFromConfig(XmlDataDocument xmlDoc)
        {
            try
            {
                var xmlnode = xmlDoc.GetElementsByTagName("FontScaleParamsForHD");
                if (xmlnode.Count == 0)
                {
                    throw new Exception("Launcher.config tag <FontScaleParamsForHD> could not be empty.");
                }

                var childNodes = xmlnode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    var scaleFactor = childNodes[i].Attributes["ScaleFactor"];
                    var wndFontSize = childNodes[i].Attributes["WndFontSize"];
                    var micAndTooltipFontSize = childNodes[i].Attributes["MicAndTooltipFontSize"];
                    if (scaleFactor != null && !string.IsNullOrEmpty(scaleFactor.Value) &&
                        wndFontSize != null && !string.IsNullOrEmpty(wndFontSize.Value) &&
                        micAndTooltipFontSize != null && !string.IsNullOrEmpty(micAndTooltipFontSize.Value))
                    {
                        FontScaleParamsForHD.Add(new FontScaleParamForHdModel(
                            Convert.ToDouble(scaleFactor.Value),
                            Convert.ToInt32(wndFontSize.Value),
                            Convert.ToInt32(micAndTooltipFontSize.Value)));
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ConfigReader > GetFontScaleParamsForHDFromConfig");
            }
        }

        public void GetOtherParamsFromConfig(XmlDataDocument xmlDoc)
        {
            try
            {
                var xmlnode = xmlDoc.GetElementsByTagName("OtherParams");
                if (xmlnode.Count == 0)
                {
                    throw new Exception("Launcher.config tag <OtherParams> could not be empty.");
                }

                var childNodes = xmlnode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    var name = childNodes[i].Attributes["Name"];
                    var value = childNodes[i].Attributes["Value"];
                    if (name != null && !string.IsNullOrEmpty(name.Value) && value != null && !string.IsNullOrEmpty(value.Value))
                    {
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
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ConfigReader > GetOtherParamsFromConfig");
            }
        }

        public void GetDataFromFile()
        {
            try
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                var fs = new FileStream(Uri, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);

                GetGamesFromConfig(xmldoc);
                GetResolutionsFromConfig(xmldoc);
                GetFontScaleParamsForHDFromConfig(xmldoc);
                GetOtherParamsFromConfig(xmldoc);

                var xmlDoc = XDocument.Load(Uri);

                BaseParametrsNames = xmlDoc.Element("Configuration").Element("ParametrsNameInConfig").Elements("Value").Select(x => x.Value).ToArray();
                HdParametrsNames = xmlDoc.Element("Configuration").Element("ParametrsNameInConfigHD").Elements("Value").Select(x => x.Value).ToArray();

                GetGameConfigs(Games.Select(x=>x.ConfigPath).ToArray());
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GetDataFromFile");
            }
        }

        private void GetGameConfigs(string[] configs)
        {
            try
            {
                for (int i = 0; i < configs.Length; i++)
                {
                    var baseDictionary = new Dictionary<string, string>();
                    var withOutHdConfigDictionary = new Dictionary<string, string>();
                    var withHdWithDefaultSightConfigDictionary = new Dictionary<string, string>();
                    var withHdWithSmallSightConfigDictionary = new Dictionary<string, string>();
                    var withHdWithHardcoreSightConfigDictionary = new Dictionary<string, string>();
                    var withHdWithOvalSightConfigDictionary = new Dictionary<string, string>();

                    var settingsUri = Directory.GetCurrentDirectory() + @"\LauncherConfig\" + configs[i];

                    var settingsXmlDoc = XDocument.Load(settingsUri);
                    var baseConfigList = settingsXmlDoc.Element("configuration").Element("Base").Elements("Value").Select(x => x.Value).ToArray();
                    var withOutHdConfigList = settingsXmlDoc.Element("configuration").Element("WithOutHD").Elements("Value").Select(x => x.Value).ToArray();
                    var withHdWithDefaultSightConfigList = settingsXmlDoc.Element("configuration").Element("WithHDWithDefaultSight").Elements("Value").Select(x => x.Value).ToArray();
                    var withHdWithSmallSightConfigList = settingsXmlDoc.Element("configuration").Element("WithHDWithSmallSight").Elements("Value").Select(x => x.Value).ToArray();
                    var withHdWithHardcoreSightConfigList = settingsXmlDoc.Element("configuration").Element("WithHDWithHardcoreSight").Elements("Value").Select(x => x.Value).ToArray();
                    var withHdWithOvalSightConfigList = settingsXmlDoc.Element("configuration").Element("WithHDWithOvalSight").Elements("Value").Select(x => x.Value).ToArray();

                    for (int j = 0; j < BaseParametrsNames.Length; j++)
                    {
                        baseDictionary.Add(BaseParametrsNames[j], baseConfigList[j]);
                    }
                    for (int j = 0; j < HdParametrsNames.Length; j++)
                    {
                        withOutHdConfigDictionary.Add(HdParametrsNames[j], withOutHdConfigList[j]);
                        withHdWithDefaultSightConfigDictionary.Add(HdParametrsNames[j], withHdWithDefaultSightConfigList[j]);
                        withHdWithSmallSightConfigDictionary.Add(HdParametrsNames[j], withHdWithSmallSightConfigList[j]);
                        withHdWithHardcoreSightConfigDictionary.Add(HdParametrsNames[j], withHdWithHardcoreSightConfigList[j]);
                        withHdWithOvalSightConfigDictionary.Add(HdParametrsNames[j], withHdWithOvalSightConfigList[j]);
                    }
                    ListBaseConfigDictionary.Add(baseDictionary);
                    ListWithOutHdConfigDictionary.Add(withOutHdConfigDictionary);
                    ListWithHdWithDefaultSightConfigDictionary.Add(withHdWithDefaultSightConfigDictionary);
                    ListWithHdWithSmallSightConfigDictionary.Add(withHdWithSmallSightConfigDictionary);
                    ListWithHdWithHardcoreSightConfigDictionary.Add(withHdWithHardcoreSightConfigDictionary);
                    ListWithHdWithOvalSightConfigDictionary.Add(withHdWithOvalSightConfigDictionary);
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GetGameConfigs");
            }
        }
    }
}
