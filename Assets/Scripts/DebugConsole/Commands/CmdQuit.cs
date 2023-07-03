using UnityEngine;

namespace DebugConsole
{
    public class CmdQuit : DebugCommand
    {
        public override string Title { get; protected set; } = "quit";
        public override string Description { get; protected set; } = "Quits the game.";
        public override string[] Args { get; protected set; }

        public override DebugConsole.AccessLevel RequiredLevel { get; protected set; } = DebugConsole.AccessLevel.None;

        public override void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                DebugConsole.Log(InvalidArgMessage, LogType.Error);
            }

            if (Application.isEditor)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif 
            }
            else
            {
                //Do some stuff
                Application.Quit();
            }
        }
    }
}
