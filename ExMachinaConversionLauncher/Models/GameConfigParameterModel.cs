namespace ExMachinaConversionLauncher.Models
{
    public class GameConfigParameterModel
    {
        internal string Name { get; set; }
        internal string Value { get; set; }

        public GameConfigParameterModel(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}