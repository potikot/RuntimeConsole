using UnityEditor;
using UnityEngine;

namespace PotikotTools.RuntimeConsole.Editor
{
    public class ConsolePreferencesWindow : EditorWindow
    {
        private const float SpaceBetweenBlocks = 10f;

        private static ConsolePreferencesSO _preferences;
        private static ConsolePreferencesSO _preferencesCopy;

        private bool _hasUnsavedChanges;

        [MenuItem("Tools/PotikotTools/Console Preferences")]
        public static void Open()
        {
            GetWindow<ConsolePreferencesWindow>("Console Preferences");
        }

        private void OnEnable()
        {
            if (_preferences == null)
            {
                _preferences = ConsolePreferences.GetPreferences();
                _preferencesCopy = _preferences.CreateCopy();
            }
        }

        private void OnDisable()
        {
            if (_hasUnsavedChanges)
            {
                if (DisplayUnsavedChangesDialog())
                    SavePreferences();
                else
                    _hasUnsavedChanges = false;
            }
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            DrawGeneralFields();
            DrawLogTextColorFields();
            DrawLogBackgroundColorFields();
            DrawCommandTextColorFields();

            if (EditorGUI.EndChangeCheck())
            {
                _hasUnsavedChanges = true;
            }

            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(200f));

            if (_hasUnsavedChanges && GUILayout.Button("Save"))
                SavePreferences();

            if (GUILayout.Button("Reset") && DisplayResetDialog())
                ResetPreferences();

            EditorGUILayout.EndVertical();
        }

        private void SavePreferences()
        {
            _preferences.CopyFrom(_preferencesCopy);

            EditorUtility.SetDirty(_preferences);
            AssetDatabase.SaveAssetIfDirty(_preferences);

            _hasUnsavedChanges = false;
        }

        private void ResetPreferences()
        {
            _preferencesCopy.Reset();
            SavePreferences();
        }

        #region Draw Fields

        private void DrawLabel(string text)
        {
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        }

        private void DrawGeneralFields()
        {
            DrawLabel("General");

            _preferencesCopy.InitializeOnStart = EditorGUILayout.Toggle("Initialize On Start", _preferencesCopy.InitializeOnStart);

            _preferencesCopy.ToggleKey = (KeyCode)EditorGUILayout.EnumPopup("Open/Close Key", _preferencesCopy.ToggleKey);

            EditorGUILayout.Space(SpaceBetweenBlocks);
        }

        private void DrawLogTextColorFields()
        {
            DrawLabel("Log Text Colors");

            _preferencesCopy.NormalLogTextColor = EditorGUILayout.ColorField("Normal Color", _preferencesCopy.NormalLogTextColor);
            _preferencesCopy.WarningLogTextColor = EditorGUILayout.ColorField("Warning Color", _preferencesCopy.WarningLogTextColor);
            _preferencesCopy.ErrorLogTextColor = EditorGUILayout.ColorField("Error Color", _preferencesCopy.ErrorLogTextColor);

            EditorGUILayout.Space(SpaceBetweenBlocks);
        }

        private void DrawLogBackgroundColorFields()
        {
            DrawLabel("Log Background Colors");

            _preferencesCopy.NormalLogBackgroundColor = EditorGUILayout.ColorField("Normal Color", _preferencesCopy.NormalLogBackgroundColor);
            _preferencesCopy.HighlightedLogBackgroundColor = EditorGUILayout.ColorField("Highlighted Color", _preferencesCopy.HighlightedLogBackgroundColor);

            EditorGUILayout.Space(SpaceBetweenBlocks);
        }

        private void DrawCommandTextColorFields()
        {
            DrawLabel("Command Colors");

            _preferencesCopy.HighlightedCommandColor = EditorGUILayout.ColorField("Highlighted Color", _preferencesCopy.HighlightedCommandColor);
            _preferencesCopy.PressedCommandColor = EditorGUILayout.ColorField("Pressed Color", _preferencesCopy.PressedCommandColor);

            EditorGUILayout.Space(SpaceBetweenBlocks);
        }

        #endregion

        #region Dialogs

        private static bool DisplayUnsavedChangesDialog()
        {
            return EditorUtility.DisplayDialog(
                title: "Console preferences unsaved changes",
                message: "You have unsaved changes. Do you want to save them before closing?",
                ok: "Save",
                cancel: "Discard"
            );
        }

        private static bool DisplayResetDialog()
        {
            return EditorUtility.DisplayDialog(
                title: "Reset",
                message: "This will reset all preferences",
                ok: "Ok",
                cancel: "Cancel"
            );
        }

        #endregion
    }
}