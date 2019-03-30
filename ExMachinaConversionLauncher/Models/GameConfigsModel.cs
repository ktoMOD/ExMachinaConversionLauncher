using System.Collections.Generic;

namespace ExMachinaConversionLauncher.Models
{
    public class GameConfigsModel
    {
        internal List<GameConfigParameterModel> GeneralParameters { get; set; }
        internal List<GameConfigUiOptionModel> UiParameters { get; set; }

        public GameConfigsModel(List<GameConfigParameterModel> generalParameters, List<GameConfigUiOptionModel> uiParameters)
        {
            GeneralParameters = generalParameters;
            UiParameters = uiParameters;
        }
    }
}