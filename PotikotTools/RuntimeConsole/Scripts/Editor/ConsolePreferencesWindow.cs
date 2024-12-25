using System.IO;
using UnityEditor;
using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public class ConsolePreferencesWindow : EditorWindow
    {
        private const float _spaceBetweenBlocks = 10f;

        private static ConsolePreferencesSO _preferences;

        private bool _hasUnsavedChanges;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            LoadOrCreatePreferences();
        }

        [MenuItem("Tools/PotikotTools/Console Preferences")]
        public static void Open()
        {
            GetWindow<ConsolePreferencesWindow>("Console Preferences");
        }

        private void OnEnable()
        {
            if (_preferences == null)
                _preferences = LoadOrCreatePreferences();
        }

        private void OnDisable()
        {
            if (_hasUnsavedChanges && DisplayUnsavedChangesDialog())
                SavePreferences();
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();

            DrawKeyFields();
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
            SavePreferences(_preferences);
            _hasUnsavedChanges = false;
        }

        private void ResetPreferences()
        {
            _preferences.Reset();
            SavePreferences();
        }

        #region Draw Fields

        private void DrawLabel(string text)
        {
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        }

        private void DrawKeyFields()
        {
            DrawLabel("Keys");

            _preferences.ToggleKey = (KeyCode)EditorGUILayout.EnumPopup("Open/Close Key", _preferences.ToggleKey);

            EditorGUILayout.Space(_spaceBetweenBlocks);
        }

        private void DrawLogTextColorFields()
        {
            DrawLabel("Log Text Colors");

            _preferences.NormalLogTextColor = EditorGUILayout.ColorField("Normal Color", _preferences.NormalLogTextColor);
            _preferences.WarningLogTextColor = EditorGUILayout.ColorField("Warning Color", _preferences.WarningLogTextColor);
            _preferences.ErrorLogTextColor = EditorGUILayout.ColorField("Error Color", _preferences.ErrorLogTextColor);

            EditorGUILayout.Space(_spaceBetweenBlocks);
        }

        private void DrawLogBackgroundColorFields()
        {
            DrawLabel("Log Background Colors");

            _preferences.NormalLogBackgroundColor = EditorGUILayout.ColorField("Normal Color", _preferences.NormalLogBackgroundColor);
            _preferences.HighlightedLogBackroundColor = EditorGUILayout.ColorField("Highlighted Color", _preferences.HighlightedLogBackroundColor);

            EditorGUILayout.Space(_spaceBetweenBlocks);
        }

        private void DrawCommandTextColorFields()
        {
            DrawLabel("Command Colors");

            _preferences.HighlightedCommandColor = EditorGUILayout.ColorField("Highlighted Color", _preferences.HighlightedCommandColor);
            _preferences.PressedCommandColor = EditorGUILayout.ColorField("Pressed Color", _preferences.PressedCommandColor);

            EditorGUILayout.Space(_spaceBetweenBlocks);
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

        #region Save & Load

        private static ConsolePreferencesSO LoadOrCreatePreferences()
        {
            ConsolePreferencesSO preferences = AssetDatabase.LoadAssetAtPath<ConsolePreferencesSO>(ConsolePreferences.FullPath);

            if (preferences == null)
            {
                if (!Directory.Exists(ConsolePreferences.ResourcesFolderPath))
                    Directory.CreateDirectory(ConsolePreferences.ResourcesFolderPath);

                preferences = CreateInstance<ConsolePreferencesSO>();

                AssetDatabase.CreateAsset(preferences, ConsolePreferences.FullPath);

                preferences.Reset();
                SavePreferences(preferences);
            }

            return preferences;
        }

        private static void SavePreferences(ConsolePreferencesSO preferences)
        {
            EditorUtility.SetDirty(preferences);
            AssetDatabase.SaveAssets();
        }

        #endregion
    }
}