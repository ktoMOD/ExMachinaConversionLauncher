namespace ExMachinaConversionLauncher.Models
{
    public class AdvancedGraphicSettingModel
    {
        public readonly string Name;
        public readonly string AdvancedValue;
        public readonly string DefaultValue;

        public AdvancedGraphicSettingModel(string name, string advancedValue, string defaultValue)
        {
            Name = name;
            AdvancedValue = advancedValue;
            DefaultValue = defaultValue;
        }
    }
}
