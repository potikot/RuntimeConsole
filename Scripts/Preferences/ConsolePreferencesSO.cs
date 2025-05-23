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
        public Color HighlightedLogBackgroundColor = new(0f, 0f, 1f, 0.75f);

        public Color HighlightedCommandColor = new(1f, 0.5f, 0f);
        public Color PressedCommandColor = new(1f, 0.5f, 0f, 0.75f);

        public void Reset()
        {
            InitializeOnStart = true;

            ToggleKey = KeyCode.Tab;

            NormalLogTextColor = Color.white;
            WarningLogTextColor = new Color(1f, 0.5f, 0f);
            ErrorLogTextColor = Color.red;

            NormalLogBackgroundColor = Color.clear;
            HighlightedLogBackgroundColor = new Color(0f, 0f, 1f, 0.75f);

            HighlightedCommandColor = new Color(1f, 0.5f, 0f);
            PressedCommandColor = new Color(1f, 0.5f, 0f, 0.75f);
        }

        public void CopyFrom(ConsolePreferencesSO source)
        {
            InitializeOnStart = source.InitializeOnStart;

            ToggleKey = source.ToggleKey;

            NormalLogTextColor = source.NormalLogTextColor;
            WarningLogTextColor = source.WarningLogTextColor;
            ErrorLogTextColor = source.ErrorLogTextColor;

            NormalLogBackgroundColor = source.NormalLogBackgroundColor;
            HighlightedLogBackgroundColor = source.HighlightedLogBackgroundColor;

            HighlightedCommandColor = source.HighlightedCommandColor;
            PressedCommandColor = source.PressedCommandColor;
        }

        public ConsolePreferencesSO CreateCopy()
        {
            return Instantiate(this);
        }
    }
}