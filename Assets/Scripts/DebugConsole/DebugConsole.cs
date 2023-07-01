using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace DebugConsole
{
    public class DebugConsole : MonoBehaviour
    {
        public static DebugConsole Instance { get; private set; }

        [SerializeField]
        private DebugConsoleSettings _settings;

        public bool Cheats { get; private set; } = false;
        public bool Shown { get; private set; } = false;


        public Dictionary<string, DebugCommand> CommandDictionary { get; private set; }
        public DebugConsoleDrawer ConsoleDrawer { get; private set; }

        public enum AccessLevel { None, High };
        public AccessLevel currentAccessLevel;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                ConsoleDrawer = new DebugConsoleDrawer(_settings);

                LinkCommands();
                DontDestroyOnLoad(this);

                LogPrivate("Initialized debug Console.");

            }
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
            ConsoleDrawer.commandPosted += ExecuteCommand;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
            ConsoleDrawer.commandPosted -= ExecuteCommand;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                Shown = !Shown;

                if (Shown)
                {
                    ConsoleDrawer.Reset();
                }
            }
        }
        private void OnGUI()
        {
            if (Shown && ConsoleDrawer != null)
            {
                ConsoleDrawer.Draw();
            }
        }

        private void LinkCommands()
        {
            CommandDictionary = new Dictionary<string, DebugCommand>();
            Link(new CmdClear());
        }

        private void Link(DebugCommand command)
        {
            if (!CommandDictionary.ContainsKey(command.Title))
            {
                CommandDictionary.Add(command.Title, command);

            }
        }

        //Handles logs from Unity
        private void HandleLog(string message, string stackTrace, LogType type)
        {
            Instance.ConsoleDrawer.CreateEntry(message.ToString(), type);
        }

        private void LogPrivate(object message, LogType type = LogType.Log)
        {
            ConsoleDrawer.CreateEntry(message.ToString(), type);
        }

        /// <summary>
        /// Logs a message to the in game console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public static void Log(object message, LogType type = LogType.Log)
        {
            Instance.ConsoleDrawer.CreateEntry(message.ToString(), type);
        }

        /// <summary>
        /// Clears the in game console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public static void ClearConsole()
        {
            Instance.ConsoleDrawer.ClearEntries();
        }

        private void ExecuteCommand(string entry)
        {
            string[] parsedEntry = entry.Split(' ');

            DebugCommand command;
            if (CommandDictionary.TryGetValue(parsedEntry[0], out command))
            {
                command.Execute(parsedEntry.Skip(1).ToArray());
            }
            else
            {
                LogPrivate("'" + string.Join(' ', parsedEntry) + "'" + " is not recognized as a command.", LogType.Error);
            }
        }
    }

    [System.Serializable]
    public class ConsoleEntry
    {
        public string time;
        public string text;
        public string trace;

        public LogType type;
        public ConsoleEntry(string entry, LogType type, string trace = "")
        {
            time = DateTime.Now.ToString();
            this.text = entry;
            this.type = type;
            this.trace = trace;
        }

        public string GetEntry(bool showTime, bool showType, bool showTrace)
        {
            string entry = "";

            if (showTime)
            {
                entry += "[" + time + "]" + " ";
            }

            if (showType)
            {
                entry += "[" + type.ToString() + "]" + " ";
            }

            entry += text;

            if (showTrace && !string.IsNullOrEmpty(trace))
            {
                entry += ("\n" + trace);
            }

            return entry;
        }
    }
}

