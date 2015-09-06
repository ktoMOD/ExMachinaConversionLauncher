using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ExMachinaConversionLauncher.Models
{
    public class SaveParser
    {
        private readonly ErrorHandler _errorHandler;

        public SaveParser(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
        }
        internal void Parser(string path)
        {
            try
            {
                if (path.First() != '\\') path = "\\" + path;
                var xmlFilesFromSaveFolder = Directory.GetFiles(Directory.GetCurrentDirectory() + path, "*.xml", SearchOption.AllDirectories);
                foreach (var xmlFile in xmlFilesFromSaveFolder)
                {
                    var fileEncodingStrings = File.ReadLines(xmlFile);
                    var encodingParametrName = "encoding=\"";
                    var encodingName = String.Empty;
                    foreach (var fileEncodingString in fileEncodingStrings)
                    {
                        var encodingBegin = fileEncodingString.IndexOf(encodingParametrName, StringComparison.InvariantCulture);
                        if (encodingBegin == -1) continue;

                        var encodingEnd = fileEncodingString.IndexOf("\"", encodingBegin + encodingParametrName.Length, StringComparison.InvariantCulture);
                        encodingName = fileEncodingString.Substring(encodingBegin + encodingParametrName.Length, encodingEnd - encodingBegin - encodingParametrName.Length);
                        break;
                    }
                    var saveFile = File.ReadAllText(xmlFile, Encoding.GetEncoding(encodingName));
                    var varName = "NormalizeInventoryForHDMode";
                    var paramName = "GAIParam_Value=\"";

                    var start = saveFile.IndexOf(varName, StringComparison.InvariantCulture);
                    if (start == -1) continue;



                    var begin = saveFile.IndexOf(paramName, start, StringComparison.InvariantCulture);
                    var end = saveFile.IndexOf("\"", begin + paramName.Length, StringComparison.InvariantCulture);

                    var saveFileStringBuilder = new StringBuilder(saveFile);
                    saveFileStringBuilder.Remove(begin + paramName.Length, end - begin - paramName.Length);
                    saveFileStringBuilder.Insert(begin + paramName.Length, "0.000");
                    saveFile = saveFileStringBuilder.ToString();
                    File.WriteAllText(xmlFile, saveFile, Encoding.GetEncoding(encodingName));

                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "Parser");
            }
        }
    }
}