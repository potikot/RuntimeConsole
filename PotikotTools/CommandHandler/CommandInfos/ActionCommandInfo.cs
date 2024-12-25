using System;

namespace PotikotTools.Commands
{
    public class ActionCommandInfo : ICommandInfo
    {
        private Action _callback;

        public string Name { get; private set; }
        public object Obj => null;
        public string HintText => Name;

        public Type[] ParameterTypes => null;

        public bool IsValid => _callback != null && !string.IsNullOrEmpty(Name);

        public ActionCommandInfo(string name, Action callback)
        {
            Name = name.Replace(' ', '_');
            _callback = callback;
        }

        public void Invoke(object[] parameters)
        {
            _callback.Invoke();
        }
    }
}