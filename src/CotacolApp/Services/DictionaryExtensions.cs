using System.Collections.Generic;

namespace CotacolApp.Services
{
    public static class DictionaryExtensions
    {
        private static T GetValue<T>(this IDictionary<string, T> settings, string setting, T defaultValue = default)
        {
            if (settings?.ContainsKey(setting)??false)
            {
                return settings[setting];
            }

            return defaultValue;
        }
    }
}