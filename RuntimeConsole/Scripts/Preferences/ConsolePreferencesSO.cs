using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public class ConsolePreferencesSO : ScriptableObject
    {
        public bool InitializeOnStart = true;

        public KeyCode ToggleKey = KeyCode.Tab;

        public Color NormalLogTextColor = Color.white;
        public Color WarningLogTextColor = new(1f, 0.5f, 0f);
        public Color ErrorLogTextColor = Color.red;

        public Color NormalLogBackgroundColor = Color.clear;
        public Color HighlightedLogBackroundColor = new(0f, 0f, 1f, 0.75f);

        public Color HighlightedCommandColor = new(1f, 0.5f, 0f);
        public Color PressedCommandColor = new(1f, 0.5f, 0f, 0.75f);

        public void Reset()
        {
            InitializeOnStart = true;

            ToggleKey = KeyCode.Tab;

            NormalLogTextColor = Color.white;
            WarningLogTextColor = new(1f, 0.5f, 0f);
            ErrorLogTextColor = Color.red;

            NormalLogBackgroundColor = Color.clear;
            HighlightedLogBackroundColor = new(0f, 0f, 1f, 0.75f);

            HighlightedCommandColor = new(1f, 0.5f, 0f);
            PressedCommandColor = new(1f, 0.5f, 0f, 0.75f);
        }

        public void CopyFrom(ConsolePreferencesSO preferences)
        {
            InitializeOnStart = preferences.InitializeOnStart;

            ToggleKey = preferences.ToggleKey;

            NormalLogTextColor = preferences.NormalLogTextColor;
            WarningLogTextColor = preferences.WarningLogTextColor;
            ErrorLogTextColor = preferences.ErrorLogTextColor;

            NormalLogBackgroundColor = preferences.NormalLogBackgroundColor;
            HighlightedLogBackroundColor = preferences.HighlightedLogBackroundColor;

            HighlightedCommandColor = preferences.HighlightedCommandColor;
            PressedCommandColor = preferences.PressedCommandColor;
        }

        public ConsolePreferencesSO CreateCopy()
        {
            ConsolePreferencesSO preferences = CreateInstance<ConsolePreferencesSO>();
            preferences.CopyFrom(this);

            return preferences;
        }
    }
}