namespace ExMachinaConversionLauncher.Services
{
    static class ToolsService
    {
        internal static bool BooleanValue(string value)
        {
            switch (value.ToLower())
            {
                case "yes":
                case "true":
                case "1":
                    return true;
                default:
                    return false;
            }
        }
    }
}
