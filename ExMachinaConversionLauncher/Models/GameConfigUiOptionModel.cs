using System.Collections.Generic;

namespace ExMachinaConversionLauncher.Models
{
    public class GameConfigUiOptionModel
    {
        internal string Name { get; set; }
        internal List<GameConfigParameterModel> UiParameters { get; set; }

        public GameConfigUiOptionModel(string name, List<GameConfigParameterModel> uiParameters)
        {
            Name = name;
            UiParameters = uiParameters;
        }
    }
}