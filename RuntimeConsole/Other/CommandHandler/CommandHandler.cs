using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace PotikotTools.Commands
{
    public static class CommandHandler
    {
        private const BindingFlags _bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        private const int _initialCommandsListCapacity = 20;

        private static List<ICommandInfo> _commands;

        public static IReadOnlyList<ICommandInfo> Commands => _commands;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            _commands = new List<ICommandInfo>(_initialCommandsListCapacity);
            SetupCommands();
        }

        private static void SetupCommands()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                AddMethods(type);
                AddFields(type);
            }
        }

        public static void Execute(string commandName, object[] parameters = null)
        {
            if (TryGet(commandName, out ICommandInfo commandInfo)
                || (commandInfo.ParameterTypes.Length > 0
                && (parameters == null || commandInfo.ParameterTypes.Length != parameters.Length)))
                return;

            commandInfo.Invoke(parameters);
        }

        /// <summary>
        /// Command in format "&lt;CommandName&gt; &lt;Arg1&gt; &lt;Arg2&gt; ..."
        /// </summary>
        public static void Execute(string inputCommand)
        {
            if (string.IsNullOrEmpty(inputCommand))
                return;

            string[] splittedCommand = inputCommand.Split(' ');
            if (!TryGet(splittedCommand[0], out ICommandInfo commandInfo))
            {
                Debug.LogError("[Console] Command not found: " + inputCommand);
                return;
            }

            if (!TryParseParameters(inputCommand, splittedCommand, commandInfo, out object[] parameters))
            {
                Debug.LogError("[Console] Incorrect input parameters");
                return;
            }

            commandInfo.Invoke(parameters);
        }

        public static void Register(ICommandInfo commandInfo)
        {
            if (!commandInfo.IsValid || Contains(commandInfo.Name))
                return;

            _commands.Add(commandInfo);
        }

        public static void Register(string commandName, Action callback)
        {
            if (callback == null || string.IsNullOrEmpty(commandName) || Contains(commandName))
                return;

            _commands.Add(new ActionCommandInfo(commandName, callback));
        }

        public static void Unregister(ICommandInfo commandInfo)
        {
            _commands.Remove(commandInfo);
        }

        public static void Unregister(string commandName)
        {
            int index = IndexOf(commandName);

            if (index != -1)
                _commands.RemoveAt(index);
        }

        private static int IndexOf(string commandName)
        {
            for (int i = 0; i < _commands.Count; i++)
                if (commandName == _commands[i].Name)
                    return i;

            return -1;
        }

        private static bool TryGet(string commandName, out ICommandInfo commandInfo)
        {
            foreach (ICommandInfo command in _commands)
            {
                if (commandName == command.Name)
                {
                    commandInfo = command;
                    return true;
                }
            }

            commandInfo = null;
            return false;
        }

        private static bool Contains(string commandName)
        {
            foreach (ICommandInfo command in _commands)
                if (command.Name == commandName)
                    return true;

            return false;
        }

        private static bool TryParseParameters(string inputCommand, string[] splittedCommand, ICommandInfo commandInfo, out object[] parameters)
        {
            Type[] parameterTypes = commandInfo.ParameterTypes;

            if (splittedCommand.Length == 1 && parameterTypes == null)
            {
                parameters = null;
                return true;
            }
            else if (splittedCommand.Length > 1 &&
                parameterTypes.Length == 1 &&
                (parameterTypes[0] == typeof(string) || parameterTypes[0] == typeof(object)))
            {
                parameters = new object[1] { inputCommand[(splittedCommand[0].Length + 1)..] };
                return true;
            }
            else if (splittedCommand.Length - 1 != parameterTypes.Length)
            {
                parameters = null;
                return false;
            }
            else if (splittedCommand.Length > 1)
            {
                parameters = new object[parameterTypes.Length];
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    parameters[i] = Converter.Convert(parameterTypes[i], splittedCommand[i + 1]);

                    if (parameters[i] == null)
                    {
                        parameters = null;
                        return false;
                    }
                }

                return true;
            }

            parameters = null;
            return true;
        }

        private static void AddMethods(Type type)
        {
            MethodInfo[] methodInfos = type.GetMethods(_bindingFlags);
            foreach (MethodInfo methodInfo in methodInfos)
            {
                CommandAttribute attribute = methodInfo.GetCustomAttribute<CommandAttribute>();

                if (attribute == null)
                    continue;

                string methodName = string.IsNullOrEmpty(attribute.Name) ? methodInfo.Name : attribute.Name;
                _commands.Add(new MethodCommandInfo(
                    attribute.IncludeTypeName ? $"{type.Name}.{methodName}" : methodName,
                    methodInfo));
            }
        }

        private static void AddFields(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(_bindingFlags);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                CommandAttribute attribute = fieldInfo.GetCustomAttribute<CommandAttribute>();
                
                if (attribute == null)
                    continue;

                string fieldName = string.IsNullOrEmpty(attribute.Name) ? fieldInfo.Name : attribute.Name;
                _commands.Add(new FieldCommandInfo(
                    attribute.IncludeTypeName ? $"{type.Name}.{fieldName}" : fieldName,
                    fieldInfo));
            }
        }
    }
}