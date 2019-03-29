using ExMachinaConversionLauncher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ExMachinaConversionLauncher.Services
{
    class AdvancedGraphicSettingsService
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
                XmlDataDocument xmldoc = new XmlDataDocument();
                var fs = new FileStream(_uri, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                var xmlnode = xmldoc.GetElementsByTagName("Parametr");
                var advancedGraphicSettingsModels = new List<AdvancedGraphicSettingModel>();
                for (var i = 0; i <= xmlnode.Count - 1; i++)
                {
                    var name = xmlnode[i].Attributes["Name"];
                    var advancedValue = xmlnode[i].Attributes["AdvancedValue"];
                    var defaultValue = xmlnode[i].Attributes["DefaultValue"];
                    if (name != null && !string.IsNullOrEmpty(name.Value) &&
                        advancedValue != null && !string.IsNullOrEmpty(advancedValue.Value) &&
                        defaultValue != null && !string.IsNullOrEmpty(defaultValue.Value))
                    {
                        advancedGraphicSettingsModels.Add(new AdvancedGraphicSettingModel(name.Value, advancedValue.Value, defaultValue.Value));
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