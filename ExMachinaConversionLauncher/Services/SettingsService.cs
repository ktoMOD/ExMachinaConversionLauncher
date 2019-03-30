using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ExMachinaConversionLauncher.Services
{
    public class SettingsService
    {
        private Dictionary<string, string> _launcherParams;
        private Dictionary<string, string> _gameParams;
        private Dictionary<string, int> _uiSchema2HdParams;
        private readonly string _pathToMainDirectory;
        private readonly ToolsService _toolsService;
        private readonly ErrorHandler _errorHandler;

        public SettingsService(string pathToMainDirectory)
        {
            _pathToMainDirectory = pathToMainDirectory;
            _launcherParams = new Dictionary<string, string>();
            _gameParams = new Dictionary<string, string>();
            _uiSchema2HdParams = new Dictionary<string, int>();
            _errorHandler = new ErrorHandler();
            _toolsService = new ToolsService(_errorHandler);
        }

        public void AddParamsToLauncherParams(Dictionary<string, string> newParams)
        {
            _launcherParams = _toolsService.ConcatTwoDictionariesWithoutDuplicates(_launcherParams, newParams);
        }

        public void AddParamsToGameParams(Dictionary<string, string> newParams)
        {
            _gameParams = _toolsService.ConcatTwoDictionariesWithoutDuplicates(_gameParams, newParams);
        }

        public void AddParamsToUiSchema2Hd(Dictionary<string, int> newParams)
        {
            _uiSchema2HdParams = _toolsService.ConcatTwoDictionariesWithoutDuplicates(_uiSchema2HdParams, newParams);
        }

        public void SaveLauncherParams()
        {
            UpdateConfig(_launcherParams, GetNodeXPathForLauncherParams, GetAttributeNameAsConst,
                "LauncherConfig\\Launcher.config", "Launcher.config", "utf-8");
        }

        public void SaveGameParams()
        {
            UpdateConfig(_gameParams, GetNodeXPathForGameParams, GetAttributeByName, "data\\config.cfg",
                "config.cfg", "windows-1251");
        }

        public void SaveUiSchema2HdParams()
        {
            UpdateConfig(_uiSchema2HdParams, GetNodeXPathForUiSchema2HdParams, GetAttributeByName, @"data\if\frames\uischema2_hd.xml",
                "config.cfg", "windows-1251");
        }

        private string GetNodeXPathForLauncherParams(string paramName)
        {
            return $"Configuration/OtherParams/Value[@Name=\"{paramName}\"]";
        }

        private string GetNodeXPathForGameParams(string paramName)
        {
            return $"config[@{paramName}]";
        }

        private string GetNodeXPathForUiSchema2HdParams(string paramName)
        {
            return $"resource/schema[@{paramName}]";
        }

        private string GetAttributeNameAsConst(string paramName)
        {
            return "Value";
        }

        private string GetAttributeByName(string paramName)
        {
            return paramName;
        }

        private void UpdateConfig<TValue>(Dictionary<string, TValue> keyValuePairs, Func<string, string> getNodeXPath,
            Func<string, string> getAttributeName, string filePath, string fileName, string encoding)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load($@"{_pathToMainDirectory}\{filePath}");

                foreach (var keyValuePair in keyValuePairs)
                {
                    var nodeXPath = getNodeXPath(keyValuePair.Key);
                    var nodes = doc.SelectNodes(nodeXPath);
                    if (nodes == null || nodes.Count == 0)
                    {
                        throw new Exception($"{fileName} is corrupted. /{nodeXPath}[@Name=\"{keyValuePair.Key}\"] was not found.");
                    }
                    foreach (XmlElement node in nodes)
                    {
                        var attributeName = getAttributeName(keyValuePair.Key);
                        node.SetAttribute(attributeName, keyValuePair.Value.ToString());
                    }
                }

                doc.CreateXmlDeclaration("1.0", encoding, "yes");

                var settings = new XmlWriterSettings
                {
                    Encoding = Encoding.GetEncoding(encoding),
                    OmitXmlDeclaration = false,
                    Indent = true,
                    NewLineOnAttributes = true,
                    NewLineHandling = NewLineHandling.None,
                    CheckCharacters = false
            };
                using (var writer = XmlWriter.Create($@"{_pathToMainDirectory}\{filePath}", settings))
                {
                    doc.Save(writer);
                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "SettingsService > UpdateConfig");
            }
        }

    }
}