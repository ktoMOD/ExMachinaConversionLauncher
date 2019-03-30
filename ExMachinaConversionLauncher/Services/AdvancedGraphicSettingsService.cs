using ExMachinaConversionLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ExMachinaConversionLauncher.Services
{
    internal class AdvancedGraphicSettingsService
    {
        private readonly ErrorHandler _errorHandler;
        private readonly string _uri;

        public AdvancedGraphicSettingsService(string uri, ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            _uri = uri;
        }

        public List<AdvancedGraphicSettingModel> GetDataFromFile()
        {
            try
            {
                var advancedGraphicSettingsModels = new List<AdvancedGraphicSettingModel>();
                using (var xmlFile = new FileStream(_uri, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var xDoc = new XmlDocument();
                    xDoc.Load(xmlFile);

                    var xmlNode = xDoc.GetElementsByTagName("Parametr");

                    for (var i = 0; i <= xmlNode.Count - 1; i++)
                    {
                        var xmlAttributeCollection = xmlNode[i].Attributes;
                        if (xmlAttributeCollection == null) continue;

                        var name = xmlAttributeCollection["Name"];
                        var advancedValue = xmlAttributeCollection["AdvancedValue"];
                        var defaultValue = xmlAttributeCollection["DefaultValue"];
                        if (name != null && !string.IsNullOrEmpty(name.Value) &&
                            advancedValue != null && !string.IsNullOrEmpty(advancedValue.Value) &&
                            defaultValue != null && !string.IsNullOrEmpty(defaultValue.Value))
                        {
                            advancedGraphicSettingsModels.Add(new AdvancedGraphicSettingModel(name.Value, advancedValue.Value, defaultValue.Value));
                        }
                    }
                }
                return advancedGraphicSettingsModels;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "AdvancedGraphicSettingsModel > GetDataFromFile");
                return null;
            }
        }

        public Dictionary<string, string> ConvertAdvancedGraphicSettingsListToDictionary(List<AdvancedGraphicSettingModel> advancedGraphicSettingsModels, bool advancedGraphicEnable)
        {
            try
            {
                var result = new Dictionary<string, string>();
                foreach (var advancedGraphicSettingsModel in advancedGraphicSettingsModels)
                {
                    result.Add(advancedGraphicSettingsModel.Name, advancedGraphicEnable ? advancedGraphicSettingsModel.AdvancedValue : advancedGraphicSettingsModel.DefaultValue);
                }
                return result;
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "AdvancedGraphicSettingsModel > ConvertAdvancedGraphicSettingsListToDictionary");
                return null;
            }
        }
    }
}