using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public static class ConsolePreferences
    {
        private const string FileName = "ConsolePreferences";
        
        private static ConsolePreferencesSO _preferencesSO;

        #region Properties

        public static ConsolePreferencesSO PreferencesSO
        {
            get
            {
                if (!_preferencesSO)
                {
                    _preferencesSO = Resources.Load<ConsolePreferencesSO>(FileName);
                    if (!_preferencesSO)
                    {
                        _preferencesSO = ScriptableObject.CreateInstance<ConsolePreferencesSO>();
                        
                        #if UNITY_EDITOR
                    
                        UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                        UnityEditor.AssetDatabase.CreateAsset(_preferencesSO, $"Assets/Resources/{FileName}.asset");

                        #endif
                    }
                }

                return _preferencesSO;
            }
        }
        
        public static bool InitializeOnStart
        {
            get => PreferencesSO.InitializeOnStart;
            set => PreferencesSO.InitializeOnStart = value;
        }

        public static KeyCode ToggleKey
        {
            get => PreferencesSO.ToggleKey;
            set => PreferencesSO.ToggleKey = value;
        }

        public static Color NormalLogTextColor
        {
            get => PreferencesSO.NormalLogTextColor;
            set => PreferencesSO.NormalLogTextColor = value;
        }
        public static Color WarningLogTextColor
        {
            get => PreferencesSO.WarningLogTextColor;
            set => PreferencesSO.WarningLogTextColor = value;
        }
        public static Color ErrorLogTextColor
        {
            get => PreferencesSO.ErrorLogTextColor;
            set => PreferencesSO.ErrorLogTextColor = value;
        }

        public static Color NormalLogBackgroundColor
        {
            get => PreferencesSO.NormalLogBackgroundColor;
            set => PreferencesSO.NormalLogBackgroundColor = value;
        }
        public static Color HighlightedLogBackgroundColor
        {
            get => PreferencesSO.HighlightedLogBackgroundColor;
            set => PreferencesSO.HighlightedLogBackgroundColor = value;
        }

        public static Color HighlightedCommandColor
        {
            get => PreferencesSO.HighlightedCommandColor;
            set => PreferencesSO.HighlightedCommandColor = value;
        }
        public static Color PressedCommandColor
        {
            get => PreferencesSO.PressedCommandColor;
            set => PreferencesSO.PressedCommandColor = value;
        }

        #endregion

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

#if UNITY_EDITOR

        public static void Save()
        {
            UnityEditor.EditorUtility.SetDirty(PreferencesSO);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        public static void Reset()
        {
            PreferencesSO.Reset();
            Save();
        }

        #endif
    }
}