using UnityEngine;

namespace DebugConsole
{
    public class CmdHelp : DebugCommand
    {
        public override string Title { get; protected set; } = "quit";
        public override string Description { get; protected set; } = "Quits the game.";
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
