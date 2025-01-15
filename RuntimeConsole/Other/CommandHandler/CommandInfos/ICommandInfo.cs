using System;

namespace PotikotTools.Commands
{
    public interface ICommandInfo
    {
        string Name { get; }
        object Obj { get; }
        string HintText { get; }

        public Type[] ParameterTypes { get; }

        public bool IsValid { get; }

        void Invoke(object[] parameters);
    }
}