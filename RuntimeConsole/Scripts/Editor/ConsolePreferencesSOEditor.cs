using UnityEditor;
using UnityEngine.UIElements;

namespace PotikotTools.RuntimeConsole.Editor
{
    [CustomEditor(typeof(ConsolePreferencesSO))]
    public class ConsolePreferencesSOEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI() => new();
    }
}