using UnityEngine;

namespace DebugConsole
{
    public class CmdHelp : DebugCommand
    {
        public override string Title { get; protected set; } = "/help";
        public override string Description { get; protected set; } = "Shows information about use and arguments for a command";
        public override DebugConsole.AccessLevel RequiredLevel { get; protected set; } = DebugConsole.AccessLevel.None;
        public override string[] Args { get; protected set; } = new string[] {"[commandName]"};
        public override void Execute(string[] args)
        {
            if (args.Length > 1)
            {
                DebugConsole.Log(InvalidArgMessage, LogType.Error);
            }
            if (args.Length == 0)
            {
                DebugConsole.Log(GetMessage());
            }
        }

        private string GetFullMessage()
        {

        }

    }
}
