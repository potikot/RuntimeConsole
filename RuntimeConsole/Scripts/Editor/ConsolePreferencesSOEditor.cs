using UnityEditor;
using UnityEngine.UIElements;

namespace PotikotTools.RuntimeConsole
{
    [CustomEditor(typeof(ConsolePreferencesSO))]
    public class ConsolePreferencesSOEditor : Editor
    {
        public override VisualElement CreateInspectorGUI() => new();
    }
}