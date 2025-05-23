using System.Collections.Generic;
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
            if (ConsolePreferences.InitializeOnStart)
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
                    CommandHandler.Register("help", "Shows all available commands", LogCommands);

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

        public static void Enable() => Controller.Enable();

        [Command("disable", "Disables console view", false)]
        public static void Disable() => Controller.Disable();

        [Command("log", "Log info message in console", false)]
        public static void Log(object message)
        {
            BaseLog(LogType.Log, message);
        }

        [Command("warning", "Log warning message in console", false)]
        public static void LogWarning(object message)
        {
            BaseLog(LogType.Warning, message);
        }

        [Command("error", "Log error message in console", false)]
        public static void LogError(object message)
        {
            BaseLog(LogType.Error, message);
        }

        public static void LogCommands()
        {
            Log("Command List:");
            IReadOnlyList<ICommandInfo> commandInfos = CommandHandler.Commands;

            foreach (ICommandInfo commandInfo in commandInfos)
            {
                if (string.IsNullOrEmpty(commandInfo.Description))
                    Log($"{commandInfo.HintText}");
                else
                    Log($"{commandInfo.HintText}\n{commandInfo.Description}");
            }
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