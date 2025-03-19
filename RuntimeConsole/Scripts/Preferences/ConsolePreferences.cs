using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public static class ConsolePreferences
    {
        private static ConsolePreferencesSO _preferencesSO;

        #region Properties

        public static bool InitializeOnStart
        {
            get => _preferencesSO.InitializeOnStart;
            set => _preferencesSO.InitializeOnStart = value;
        }

        public static KeyCode ToggleKey
        {
            get => _preferencesSO.ToggleKey;
            set => _preferencesSO.ToggleKey = value;
        }

        public static Color NormalLogTextColor
        {
            get => _preferencesSO.NormalLogTextColor;
            set => _preferencesSO.NormalLogTextColor = value;
        }
        public static Color WarningLogTextColor
        {
            get => _preferencesSO.WarningLogTextColor;
            set => _preferencesSO.WarningLogTextColor = value;
        }
        public static Color ErrorLogTextColor
        {
            get => _preferencesSO.ErrorLogTextColor;
            set => _preferencesSO.ErrorLogTextColor = value;
        }

        public static Color NormalLogBackgroundColor
        {
            get => _preferencesSO.NormalLogBackgroundColor;
            set => _preferencesSO.NormalLogBackgroundColor = value;
        }
        public static Color HighlightedLogBackgroundColor
        {
            get => _preferencesSO.HighlightedLogBackgroundColor;
            set => _preferencesSO.HighlightedLogBackgroundColor = value;
        }

        public static Color HighlightedCommandColor
        {
            get => _preferencesSO.HighlightedCommandColor;
            set => _preferencesSO.HighlightedCommandColor = value;
        }
        public static Color PressedCommandColor
        {
            get => _preferencesSO.PressedCommandColor;
            set => _preferencesSO.PressedCommandColor = value;
        }

        #endregion

        static ConsolePreferences()
        {
            _preferencesSO = Resources.Load<ConsolePreferencesSO>("Console Preferences");

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

        public static ConsolePreferencesSO GetPreferences()
        {
            return _preferencesSO;
        }

#if UNITY_EDITOR

        public static void Save()
        {
            UnityEditor.EditorUtility.SetDirty(_preferencesSO);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        public static void Reset()
        {
            _preferencesSO.Reset();
            Save();
        }

        #endif
    }
}