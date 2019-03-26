using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExMachinaConversionLauncher.Services
{
    public class ConfigReader
    {
        internal string Version { get; set; }
        internal string FullScreen { get; set; }
        internal string Uri { get; set; }
        internal string[] Games { get; set; }
        internal string[] Descriptions { get; set; }
        internal string[] Pictures { get; set; }
        internal string[] Configs { get; set; }
        internal string[] BaseParametrsNames { get; set; }
        internal string[] HdParametrsNames { get; set; }
        internal List<Dictionary<string, string>> ListBaseConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithOutHdConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithHdWithDefaultSightConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithHdWithSmallSightConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithHdWithHardcoreSightConfigDictionary { get; set; }
        internal List<Dictionary<string, string>> ListWithHdWithOvalSightConfigDictionary { get; set; }
        internal bool Console { get; set; }
        internal string ExeName { get; set; }
        internal string LastLaunchGame { get; set; }
        internal string LastLaunchMode { get; set; }
        internal List<string> ResolutionsCollection { get; set; }
        internal List<string> FontScaleParamsForHDCollection { get; set; }
        private readonly ErrorHandler _errorHandler;


        public ConfigReader(string uri, ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            try
            {
                Uri = uri;
                ListBaseConfigDictionary = new List<Dictionary<string, string>>();
                ListWithOutHdConfigDictionary = new List<Dictionary<string, string>>();
                ListWithHdWithDefaultSightConfigDictionary = new List<Dictionary<string, string>>();
                ListWithHdWithSmallSightConfigDictionary = new List<Dictionary<string, string>>();
                ListWithHdWithHardcoreSightConfigDictionary = new List<Dictionary<string, string>>();
                ListWithHdWithOvalSightConfigDictionary = new List<Dictionary<string, string>>();
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ConfigReader");
            }
        }

        public void GetDataFromFile()
        {
            try
            {
                var xmlDoc = XDocument.Load(Uri);

                var games = xmlDoc.Element("configuration").Element("InstallGames").Elements("Game").Select(x => x.Value).ToArray();
                Games = games;

                var descriptions = xmlDoc.Element("configuration").Element("DescriptionsGames").Elements("Descriptions").Select(x => x.Value).ToArray();
                Descriptions = descriptions;

                var pictures = xmlDoc.Element("configuration").Element("PictureForGames").Elements("Pictures").Select(x => x.Value).ToArray();
                Pictures = pictures;

                var configs = xmlDoc.Element("configuration").Element("GameConfigs").Elements("Config").Select(x => x.Value).ToArray();
                Configs = configs;

                var baseParametrsNames = xmlDoc.Element("configuration").Element("ParametrsNameInConfig").Elements("Value").Select(x => x.Value).ToArray();
                BaseParametrsNames = baseParametrsNames;

                var hdParametrsNames = xmlDoc.Element("configuration").Element("ParametrsNameInConfigHD").Elements("Value").Select(x => x.Value).ToArray();
                HdParametrsNames = hdParametrsNames;

                var resolutionsCollection = xmlDoc.Element("configuration").Element("Resolutions").Elements("Value").Select(x => x.Value).ToList();
                for (int i = 0; i < resolutionsCollection.Count; i++)
                {
                    resolutionsCollection[i] = resolutionsCollection[i].ToLower().Replace("x", "×").Replace("х", "×");
                }
                ResolutionsCollection = resolutionsCollection;

                var fontScaleParamsForHDCollection = xmlDoc.Element("configuration").Element("FontScaleParamsForHD").Elements("Value").Select(x => x.Value).ToList();
                for (int i = 0; i < fontScaleParamsForHDCollection.Count; i++)
                {
                    fontScaleParamsForHDCollection[i] = fontScaleParamsForHDCollection[i].ToLower();
                }
                FontScaleParamsForHDCollection = fontScaleParamsForHDCollection;

                var console = xmlDoc.Element("configuration").Element("console").Value;
                Console = Convert.ToBoolean(console);

                var exeName = xmlDoc.Element("configuration").Element("exeName").Value;
                ExeName = exeName;

                var lastLaunchGame = xmlDoc.Element("configuration").Element("lastLaunchGame").Value;
                LastLaunchGame = lastLaunchGame;

                var version = xmlDoc.Element("configuration").Element("conVersion").Value;
                Version = version;

                var fullScreen = xmlDoc.Element("configuration").Element("fullScreen").Value;
                FullScreen = fullScreen;

                var lastLaunchMode = xmlDoc.Element("configuration").Element("lastLaunchMode").Value;
                LastLaunchMode = lastLaunchMode;

                GetGameConfigs(Configs);
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

        internal string ReadHdMode()
        {
            try
            {
                var xmlDoc = XDocument.Load(Uri);
                var lastLaunchHdMode = xmlDoc.Element("configuration").Element("launcherHDMode").Value;
                return lastLaunchHdMode;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "ReadHdMode");
                return null;
            }
        }
    }
}
