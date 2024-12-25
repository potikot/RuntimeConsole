using System;

namespace PotikotTools
{
    public class ContextMenuItem
    {
        public readonly string Name;

        private readonly Action _action;

        public ContextMenuItem(string name, Action action)
        {
            Name = name;
            _action = action;
        }

        public void Execute()
        {
            _action.Invoke();
        }
    }
}
