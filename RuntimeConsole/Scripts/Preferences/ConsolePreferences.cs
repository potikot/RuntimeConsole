using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public static class ConsolePreferences
    {
        public const string FileName = "Console Preferences";

        private static ConsolePreferencesSO _preferencesSO;

        #region Properties

        public static bool InitializeOnStart => _preferencesSO.InitializeOnStart;

        public static KeyCode ToggleKey => _preferencesSO.ToggleKey;

        public static Color NormalLogTextColor => _preferencesSO.NormalLogTextColor;
        public static Color WarningLogTextColor => _preferencesSO.WarningLogTextColor;
        public static Color ErrorLogTextColor => _preferencesSO.ErrorLogTextColor;

        public static Color NormalLogBackgroundColor => _preferencesSO.NormalLogBackgroundColor;
        public static Color HighlightedLogBackroundColor => _preferencesSO.HighlightedLogBackroundColor;

        public static Color HighlightedCommandColor => _preferencesSO.HighlightedCommandColor;
        public static Color PressedCommandColor => _preferencesSO.PressedCommandColor;

        #endregion

        static ConsolePreferences()
        {
            if (_preferencesSO == null)
                _preferencesSO = Resources.Load<ConsolePreferencesSO>(FileName);
            if (_preferencesSO == null)
                _preferencesSO = ScriptableObject.CreateInstance<ConsolePreferencesSO>();
        }

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