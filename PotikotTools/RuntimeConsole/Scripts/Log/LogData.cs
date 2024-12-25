using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public class LogData
    {
        public string Message;
        public Sprite Icon;
        public LogType Type;

        public LogData(string message, LogType type)
        {
            Message = message;
            Type = type;

            Icon = null;
        }
    }
}