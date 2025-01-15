using PotikotTools.Commands;
using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public static class Console
    {
        private static ConsoleController _controller;
        private static bool _triedCreateController;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            Controller.Disable();
        }

        public static ConsoleController Controller
        {
            get
            {
                if (!_triedCreateController && _controller == null)
                {
                    _triedCreateController = true;

                    ConsoleView consoleView = Object.FindObjectOfType<ConsoleView>();
                    CommandsView commandsView;

                    if (consoleView == null)
                    {
                        consoleView = Object.Instantiate(Resources.Load<ConsoleView>("Console"));
                        if (consoleView == null || !consoleView.TryGetComponent(out commandsView))
                            return null;
                    }
                    else if (!consoleView.TryGetComponent(out commandsView))
                    {
                        return null;
                    }

                    if (!consoleView.TryGetComponent(out ConsoleInput input))
                        input = consoleView.gameObject.AddComponent<ConsoleInput>();

                    _controller = new ConsoleController(consoleView, commandsView, input);
                    _controller.Disable();

                    Object.DontDestroyOnLoad(consoleView.gameObject);

                    Debug.Log("[Console] Controller created");
                }

                return _controller;
            }
            set
            {
                if (value == null)
                    return;

                _controller = value;
                Debug.Log("[Console] Controller changed");
            }
        }

        [Command("enable")]
        public static void Enable() => Controller.Enable();

        [Command("disable")]
        public static void Disable() => Controller.Disable();

        [Command("log")]
        public static void Log(object message)
        {
            BaseLog(LogType.Log, message.ToString());
        }

        [Command("warning")]
        public static void LogWarning(object message)
        {
            BaseLog(LogType.Warning, message.ToString());
        }

        [Command("error")]
        public static void LogError(object message)
        {
            BaseLog(LogType.Error, message.ToString());
        }

        private static void BaseLog(LogType logType, object message)
        {
            if (Controller == null)
            {
                Debug.LogError($"[Console] Controller is null. Logged message: {message}. Type: {logType}");
                return;
            }

            Controller.Log(logType, message);
        }
    }
}