using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace PotikotTools.Commands
{
    public static class CommandHandlerPreferences
    {
        public const BindingFlags ReflectionBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        private const string FileName = "CommandHandlerPreferences";
        
        private static CommandHandlerPreferencesSO _preferencesSO;

        public static CommandHandlerPreferencesSO PreferencesSO
        {
            get
            {
                if (!_preferencesSO)
                {
                    _preferencesSO = Resources.Load<CommandHandlerPreferencesSO>(FileName);
                    if (!_preferencesSO)
                    {
                        _preferencesSO = ScriptableObject.CreateInstance<CommandHandlerPreferencesSO>();
                        
                        #if UNITY_EDITOR

                        UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
                        UnityEditor.AssetDatabase.CreateAsset(_preferencesSO, $"Assets/Resources/{FileName}.asset");
                        
                        #endif
                    }
                }
                
                return _preferencesSO;
            }
        }
        
        public static bool ExcludeFromSearchAssemblies
        {
            get => PreferencesSO.ExcludeDefaultAssemblies;
            set => PreferencesSO.ExcludeDefaultAssemblies = value;
        }
        
        public static List<string> ExcludedFromSearchAssemblyPrefixes => PreferencesSO.ExcludedAssemblyPrefixes;
        public static List<string> CommandAttributeUsingAssemblies => PreferencesSO.CommandAttributeUsingAssemblies;

        #if UNITY_EDITOR

        public static void Save()
        {
            UnityEditor.EditorUtility.SetDirty(PreferencesSO);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(PreferencesSO);
        }

        public static void Reset()
        {
            PreferencesSO.Reset();
            Save();
        }
        
        #endif
    }
}