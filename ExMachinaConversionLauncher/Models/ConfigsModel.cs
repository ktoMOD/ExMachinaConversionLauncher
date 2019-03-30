﻿using System.Collections.Generic;

namespace ExMachinaConversionLauncher.Models
{
    public class ConfigsModel
    {
        internal Dictionary<string, string> GeneralParameters { get; set; }
        internal List<ConfigUiOptionModel> UiParameters { get; set; }

        public ConfigsModel(Dictionary<string, string> generalParameters, List<ConfigUiOptionModel> uiParameters)
        {
            GeneralParameters = generalParameters;
            UiParameters = uiParameters;
        }
    }
}