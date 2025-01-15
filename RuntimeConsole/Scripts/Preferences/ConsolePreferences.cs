using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public static class ConsolePreferences
    {
        public const string FileName = "Console Preferences";

        private static ConsolePreferencesSO _preferencesSO;

        public static ConsolePreferencesSO PreferencesSO
        {
            get
            {
                if (_preferencesSO == null)
                    _preferencesSO = Resources.Load<ConsolePreferencesSO>(FileName);
                if (_preferencesSO == null)
                    _preferencesSO = ScriptableObject.CreateInstance<ConsolePreferencesSO>();

                return _preferencesSO;
            }
        }

        public static KeyCode ToggleKey => PreferencesSO.ToggleKey;

        public static Color NormalLogTextColor => PreferencesSO.NormalLogTextColor;
        public static Color WarningLogTextColor => PreferencesSO.WarningLogTextColor;
        public static Color ErrorLogTextColor => PreferencesSO.ErrorLogTextColor;

        public static Color NormalLogBackgroundColor => PreferencesSO.NormalLogBackgroundColor;
        public static Color HighlightedLogBackroundColor => PreferencesSO.HighlightedLogBackroundColor;

        public static Color HighlightedCommandColor => PreferencesSO.HighlightedCommandColor;
        public static Color PressedCommandColor => PreferencesSO.PressedCommandColor;

        public static Color GetLogTextColor(LogType logType)
        {
            return logType switch
            {
                LogType.Log => NormalLogTextColor,
                LogType.Warning => WarningLogTextColor,
                LogType.Error => ErrorLogTextColor,
                _ => NormalLogTextColor
            };
        }
    }
}