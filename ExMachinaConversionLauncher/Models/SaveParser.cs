using System;
using System.Collections.Generic;
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
        internal void NormalizeInventoryForHdMode(string path)
        {
            try
            {
                if (path.First() != '\\') path = "\\" + path;
                var xmlFilesFromSaveFolder = Directory.GetFiles(Directory.GetCurrentDirectory() + path, "*.xml", SearchOption.AllDirectories);
                foreach (var xmlFile in xmlFilesFromSaveFolder)
                {
                    var fileEncodingStrings = File.ReadAllLines(xmlFile);
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
                _errorHandler.CallErrorWindows(ex, "NormalizeInventoryForHdMode");
            }
        }
        internal void NormalizeWandererGunSkin(string path)
        {
            try
            {
                if (path.First() != '\\') path = "\\" + path;
                var xmlFilesFromSaveFolder = Directory.GetFiles(Directory.GetCurrentDirectory() + path, "*.xml", SearchOption.AllDirectories);
                foreach (var xmlFile in xmlFilesFromSaveFolder)
                {
                    var fileEncodingStrings = File.ReadAllLines(xmlFile);
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
                    var varName = "Name=\"Wanderer\"";
                    var paramName = "Skin";

                    int start = 1;
                    int finish = 0;
                    
                    if (saveFile.IndexOf(varName, start, StringComparison.InvariantCulture) == -1) continue;

                    while (start != -1)
                    {
                        
                        var range = Enumerable.Range(1, 35).Where(i => i!=8);
                        var rand = new Random();
                        int index = rand.Next(0, 34);



                        start = saveFile.IndexOf(varName, start, StringComparison.InvariantCulture);
                        if (start == -1) break;
                        finish = saveFile.IndexOf(">", start, StringComparison.InvariantCulture);

                        var substring = saveFile.Substring(start, finish - start);
                        if (substring.Contains("Skin=\"8\""))
                        {
                            substring = substring.Replace("Skin=\"8\"", "Skin=\""+ range.ElementAt(index) + "\"");
                        }
                        else if (!substring.Contains("Skin="))
                        {
                            substring = substring + "\n\tSkin=\"" + range.ElementAt(index) + "\"";
                        }

                        var saveFileStringBuilder = new StringBuilder(saveFile);
                        saveFileStringBuilder.Remove(start, finish - start);
                        saveFileStringBuilder.Insert(start, substring);
                        saveFile = saveFileStringBuilder.ToString();


                        start = finish;
                    }
                    File.WriteAllText(xmlFile, saveFile, Encoding.GetEncoding(encodingName));

                }
            }
            catch (Exception ex)
            {
                _errorHandler.CallErrorWindows(ex, "NormalizeWandererGunSkin");
            }
        }
    }
}