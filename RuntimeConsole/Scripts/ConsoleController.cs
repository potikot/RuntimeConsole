namespace PotikotTools.RuntimeConsole
{
    public class ConsoleController
    {
        private ConsoleView _consoleView;
        private CommandsView _commandsView;
        private ConsoleInput _input;

        public bool IsEnabled { get; private set; }

        public ConsoleController(ConsoleView consoleView, CommandsView commandsView, ConsoleInput input, bool enable = true)
        {
            _consoleView = consoleView;
            _commandsView = commandsView;
            _input = input;

            _input.OnToggleKeyPressed += OnToggleEnabled;

            if (enable)
                Enable();
        }

        ~ConsoleController()
        {
            _input.OnToggleKeyPressed -= OnToggleEnabled;

            Disable();
        }

        public void Enable()
        {
            if (IsEnabled) return;
            IsEnabled = true;

            //_input.Enable();
            _consoleView.Show();
            _commandsView.Show();
        }

        public void Disable()
        {
            if (!IsEnabled) return;
            IsEnabled = false;

            //_input.Disable();
            _consoleView.Hide();
            _commandsView.Hide();
        }

        public void Log(LogType logType, object message)
        {
            _consoleView.AddLog(new LogData(message.ToString(), logType));
        }

        private void OnToggleEnabled()
        {
            if (IsEnabled)
                Disable();
            else
                Enable();
        }
    }
}