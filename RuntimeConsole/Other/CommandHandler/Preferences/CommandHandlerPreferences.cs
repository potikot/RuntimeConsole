using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PotikotTools.Commands
{
    public static class CommandHandlerPreferences
    {
        public const BindingFlags ReflectionBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        private static CommandHandlerPreferencesSO _preferencesSO;

        public static bool ExcludeFromSearchAssemblies
        {
            get => _preferencesSO.ExcludeDefaultAssemblies;
            set => _preferencesSO.ExcludeDefaultAssemblies = value;
        }
        
        public static List<string> ExcludedFromSearchAssemblyPrefixes => _preferencesSO.ExcludedAssemblyPrefixes;
        public static List<string> CommandAttributeUsingAssemblies => _preferencesSO.CommandAttributeUsingAssemblies;

        static CommandHandlerPreferences()
        {
            _preferencesSO = Resources.Load<CommandHandlerPreferencesSO>("Command Handler Preferences");

            if (_preferencesSO == null)
                _preferencesSO = ScriptableObject.CreateInstance<CommandHandlerPreferencesSO>();
        }

        public static CommandHandlerPreferencesSO GetPreferences()
        {
            return _preferencesSO;
        }

        #if UNITY_EDITOR

        public static void Save()
        {
            EditorUtility.SetDirty(_preferencesSO);
            AssetDatabase.SaveAssetIfDirty(_preferencesSO);
        }

        public static void Reset()
        {
            _preferencesSO.Reset();
            Save();
        }
        
        #endif
    }
}