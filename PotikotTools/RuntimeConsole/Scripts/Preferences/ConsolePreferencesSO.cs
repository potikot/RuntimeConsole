using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public class ConsolePreferencesSO : ScriptableObject
    {
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
            ToggleKey = KeyCode.Tab;

            NormalLogTextColor = Color.white;
            WarningLogTextColor = new(1f, 0.5f, 0f);
            ErrorLogTextColor = Color.red;

            NormalLogBackgroundColor = Color.clear;
            HighlightedLogBackroundColor = new(0f, 0f, 1f, 0.75f);

            HighlightedCommandColor = new(1f, 0.5f, 0f);
            PressedCommandColor = new(1f, 0.5f, 0f, 0.75f);
        }
    }
}