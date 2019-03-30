using System.Collections.Generic;

namespace ExMachinaConversionLauncher.Models
{
    public class ConfigUiOptionModel
    {
        public string Name { get; set; }
        public Dictionary<string, string> UiParameters { get; set; }

        public ConfigUiOptionModel(string name, Dictionary<string, string> uiParameters)
        {
            Name = name;
            UiParameters = uiParameters;
        }
    }
}