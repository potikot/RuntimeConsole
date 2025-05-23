using PotikotTools.Commands;
using UnityEngine.UI;

namespace PotikotTools.RuntimeConsole
{
    public class CommandHintsScroll : OptimizedScroll<CommandHintButtonView, ICommandInfo>
    {
        private CommandInputView _commandInputView;

        public void Init(CommandInputView commandInputView)
        {
            _commandInputView = commandInputView;
        }

        protected override void OnSetupElement()
        {
            setupElement = view =>
            {
                ColorBlock colors = view.Button.colors;
                colors.highlightedColor = ConsolePreferences.HighlightedCommandColor;
                colors.selectedColor = ConsolePreferences.HighlightedCommandColor;
                colors.pressedColor = ConsolePreferences.PressedCommandColor;

                view.Button.colors = colors;
            };
        }

        protected override void SetElementData(CommandHintButtonView view, ICommandInfo data)
        {
            view.SetData(data.HintText, () => _commandInputView.SetInput($"{data.Name} "));
        }
    }
}