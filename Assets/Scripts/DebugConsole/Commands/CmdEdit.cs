using UnityEngine;

namespace DebugConsole
{
    public class CmdEdit : DebugCommand
    {
        public override string Title { get; protected set; } = "edit";
        public override string Description { get; protected set; } = "Edits the console.";
        public override string Help { get; protected set; } = "";
        public override DebugConsole.AccessLevel RequiredLevel { get; protected set; } = DebugConsole.AccessLevel.None;

        public override void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                DebugConsole.Log(invalidArguments, LogType.Error);

            }
            else if (args.Length == 1)
            {
                DebugConsole.Instance.ConsoleDrawer.EditConsole(true);
            }
            else if (args.Length == 2)
            {
                DebugConsole.Instance.ConsoleDrawer.EditConsole(true);
            }
            else if (args.Length == 3)
            {
                DebugConsole.Instance.ConsoleDrawer.EditConsole(true); 
            }
            else
            {
                DebugConsole.Log(invalidArguments, LogType.Error);
            }          
        }
    }
}
