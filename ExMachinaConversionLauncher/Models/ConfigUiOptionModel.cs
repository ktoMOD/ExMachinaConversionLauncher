using System.Collections.Generic;

namespace ExMachinaConversionLauncher.Models
{
    public class ConfigUiOptionModel
    {
        internal string Name { get; set; }
        internal Dictionary<string, string> UiParameters { get; set; }

        public ConfigUiOptionModel(string name, Dictionary<string, string> uiParameters)
        {
            Name = name;
            UiParameters = uiParameters;
        }
    }
}