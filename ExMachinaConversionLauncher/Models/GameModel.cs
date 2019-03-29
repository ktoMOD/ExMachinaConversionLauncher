namespace ExMachinaConversionLauncher.Models
{
    class GameModel
    {
        internal readonly string Name;
        internal readonly string Description;
        internal readonly string PicturePath;
        internal readonly string ConfigPath;

        public GameModel(string name, string picturePath, string configPath, string description)
        {
            Name = name;
            Description = description;
            PicturePath = picturePath;
            ConfigPath = configPath;
    }
    }
}
