using System;

namespace PotikotTools.Commands
{
    /// <summary>
    /// Works only with static methods / fields
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class CommandAttribute : Attribute
    {
        public string Name;
        public bool IncludeTypeName;

        public CommandAttribute(bool includeTypeName = true)
        {
            Name = string.Empty;
            IncludeTypeName = includeTypeName;
        }

        public CommandAttribute(string name, bool includeTypeName = false)
        {
            Name = name;
            IncludeTypeName = includeTypeName;
        }
    }
}