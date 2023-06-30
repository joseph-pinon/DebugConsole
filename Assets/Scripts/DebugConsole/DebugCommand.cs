using System.Collections;
using System.Collections.Generic;

namespace DebugConsole
{
    [System.Serializable]
    public abstract class DebugCommand
    {
        public abstract string Title { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }
        
        public string invalidArguments = "Invalid arguments!";

        public abstract DebugConsole.AccessLevel RequiredLevel { get; protected set; }
        public abstract void Execute(string[] args);
    }
}
