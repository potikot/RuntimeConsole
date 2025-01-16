using PotikotTools.Commands;
using System.Collections.Generic;
using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public class CommandsView : MonoBehaviour
    {
        [Header("Command Input")]
        [SerializeField] private CommandInputView _commandInputView;

        [Header("Command Hint")]
        [SerializeField] private CommandHintsScroll _scroll;

        private string _lastCommandName;

        public void Start()
        {
            _scroll.Init(_commandInputView);
        }

        public void Show()
        {
            _scroll.Show();

            _commandInputView.OnCommandChanged += OnCommandChanged;
            _commandInputView.OnCommandSubmitted += Internal_OnCommandSubmitted;

            _commandInputView.OnFocused += OnInputFocused;
            _commandInputView.OnUnfocused += OnInputUnfocused;
        }

        public void Hide()
        {
            _scroll.Hide();

            _commandInputView.OnCommandChanged -= OnCommandChanged;
            _commandInputView.OnCommandSubmitted -= Internal_OnCommandSubmitted;

            _commandInputView.OnFocused -= OnInputFocused;
            _commandInputView.OnUnfocused -= OnInputUnfocused;
        }

        public void SetupCommands(List<ICommandInfo> commands)
        {
            _scroll.SetElements(commands);
            _commandInputView.Focus();
        }

        private void OnCommandChanged(string inputCommand)
        {
            if (string.IsNullOrEmpty(inputCommand))
            {
                _lastCommandName = null;
                _scroll.Clear();
                return;
            }

            string commandName = GetCommandName(inputCommand);
            if (commandName == _lastCommandName)
                return;

            _lastCommandName = commandName;
            SetupCommands(GetCommandsByName(commandName));
        }

        private void Internal_OnCommandSubmitted(string inputCommand)
        {
            CommandHandler.Execute(inputCommand);
            _lastCommandName = null;
            _scroll.Clear();

            _commandInputView.ClearInput();
            _commandInputView.Focus();
        }

        private void OnInputFocused()
        {
            _scroll.Show();
        }

        private void OnInputUnfocused()
        {
            _scroll.Hide();
        }

        private List<ICommandInfo> GetCommandsByName(string commandName)
        {
            List<ICommandInfo> result = new();

            foreach (ICommandInfo commandInfo in CommandHandler.Commands)
                if (commandInfo.Name.StartsWith(commandName))
                    result.Add(commandInfo);

            return result;
        }

        private string GetCommandName(string command)
        {
            int spaceIndex = 0, limit = command.Length - 1;
            for (; spaceIndex < limit; spaceIndex++)
                if (command[spaceIndex] == ' ')
                    break;
            
            return command[spaceIndex] == ' ' ? command[..spaceIndex] : command[..++spaceIndex];
        }
    }
}