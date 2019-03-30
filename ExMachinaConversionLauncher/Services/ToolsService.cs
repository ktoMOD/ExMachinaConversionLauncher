using System.Collections.Generic;

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

        internal static Dictionary<TKey, TValue> ConcatTwoDictionariesWithoutDuplicates<TKey, TValue>(Dictionary<TKey, TValue> first, Dictionary<TKey, TValue> second)
        {
            var mergedSettings = new Dictionary<TKey, TValue>(first);
            foreach (var keyValue in second)
            {
                mergedSettings[keyValue.Key] = keyValue.Value;
            }

            return mergedSettings;
        }
    }
}
