using ExMachinaConversionLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ExMachinaConversionLauncher.Services
{
    public class GameConfigsReader
    {
        public List<string> GeneralParameterNames { get; set; }
        public List<string> UiParameterNames { get; set; }
        private readonly ErrorHandler _errorHandler;


        public GameConfigsReader(ErrorHandler errorHandler, List<string> generalParameterNames, List<string> uiParameterNames)
        {
            _errorHandler = errorHandler;
            GeneralParameterNames = generalParameterNames;
            UiParameterNames = uiParameterNames;
        }

        public XmlDocument GetXmlDoc(string uri)
        {
            using (var xmlFile = new FileStream(uri, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var xDoc = new XmlDocument();
                xDoc.Load(xmlFile);
                return xDoc;
            }
        }

        private Dictionary<string, string> GetParameters(XmlDocument xmlDoc, string parameterName)
        {
            var result = new Dictionary<string, string>();

            var launcherConfigSectionName = "GeneralParameters";
            var validationArray = GeneralParameterNames;
            if (parameterName != "GeneralParameters")
            {
                launcherConfigSectionName = "UiParameters";
                validationArray = UiParameterNames;
            }

            try
            {
                var xmlNode = xmlDoc.GetElementsByTagName(parameterName);
                if (xmlNode.Count == 0)
                {
                    throw new Exception($"Config tag <{parameterName}> could not be empty.");
                }

                var childNodes = xmlNode[0].ChildNodes;
                for (var i = 0; i <= childNodes.Count - 1; i++)
                {
                    var xmlAttributeCollection = childNodes[i].Attributes;
                    if (xmlAttributeCollection == null) continue;

                    var name = xmlAttributeCollection["Name"];
                    var value = xmlAttributeCollection["Value"];
                    if (name != null && !string.IsNullOrEmpty(name.Value) &&
                        validationArray.Contains(name.Value) &&
                        value != null && !string.IsNullOrEmpty(value.Value))
                    {
                        result.Add(name.Value, value.Value);
                    }
                }

                if (validationArray.Count == result.Count) return result;

                throw new Exception(
                    $"{launcherConfigSectionName} names and count from Launcher.config should be the same as "+
                    $"{parameterName} names and count in Game.config file.");
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GameConfigsReader > GetGeneralParameters");
                return null;
            }
        }

        public ConfigsModel GetGameConfigs(string uri)
        {
            try
            {
                var xmlDoc = GetXmlDoc(uri);

                var generalParameters = GetParameters(xmlDoc, "GeneralParameters");
                var uiParameters = new List<ConfigUiOptionModel>()
                {
                    new ConfigUiOptionModel("WithOutHD", GetParameters(xmlDoc, "WithOutHD")),
                    new ConfigUiOptionModel("WithHDWithDefaultSight", GetParameters(xmlDoc, "WithHDWithDefaultSight")),
                    new ConfigUiOptionModel("WithHDWithSmallSight", GetParameters(xmlDoc, "WithHDWithSmallSight")),
                    new ConfigUiOptionModel("WithHDWithOvalSight", GetParameters(xmlDoc, "WithHDWithOvalSight")),
                    new ConfigUiOptionModel("WithHDWithHardcoreSight", GetParameters(xmlDoc, "WithHDWithHardcoreSight"))
            };

                return new ConfigsModel(generalParameters, uiParameters);
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "GameConfigsReader > GetDataFromFile");
                return null;
            }
        }
    }
}
