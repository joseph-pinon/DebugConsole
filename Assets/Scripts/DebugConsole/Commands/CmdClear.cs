using UnityEngine;

namespace DebugConsole
{
    [System.Serializable]
    public class CmdClear : DebugCommand
    {
        public override string Title { get; protected set; } = "/clear";
        public override string Description { get; protected set; } = "Clears the console.";
        public override string Help { get; protected set; } = "";
        public override DebugConsole.AccessLevel RequiredLevel { get; protected set; } = DebugConsole.AccessLevel.None;

        public override void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                DebugConsole.Log(invalidArguments, LogType.Error);
            }
            DebugConsole.ClearConsole();
        }
    }
}
