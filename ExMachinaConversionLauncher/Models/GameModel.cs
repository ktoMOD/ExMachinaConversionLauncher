namespace ExMachinaConversionLauncher.Models
{
    public class GameModel
    {
        public readonly string Name;
        public readonly string Description;
        public readonly string PicturePath;
        public readonly string ConfigPath;
        public ConfigsModel GameConfigs { get; set; }

        public GameModel(string name, string picturePath, string configPath, string description)
        {
            Name = name;
            Description = description;
            PicturePath = picturePath;
            ConfigPath = configPath;
        }
    }
}
