using UnityEngine;

namespace DebugConsole
{
    [System.Serializable]
    public class CmdClear : DebugCommand
    {
        public override string Title { get; protected set; } = "/clear";
        public override string Description { get; protected set; } = "Clears the debug console.";
        public override DebugConsole.AccessLevel RequiredLevel { get; protected set; } = DebugConsole.AccessLevel.None;
        public override string[] Args { get; protected set; } = 
            new string[] {"NONE"};

        public override void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                DebugConsole.Log(InvalidArgMessage, LogType.Error);
            }
            else
            {
                DebugConsole.ClearConsole();
            }
        }
    }
}
