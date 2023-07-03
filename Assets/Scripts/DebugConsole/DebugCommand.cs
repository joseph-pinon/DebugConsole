using System.Collections;
using System.Collections.Generic;

namespace DebugConsole
{
    [System.Serializable]
    public abstract class DebugCommand
    {
        public abstract string Title { get; protected set; }
        public abstract string Description { get; protected set; }      
        public abstract string[] Args { get; protected set; }
        public virtual string Help
        {
            get
            {
                string message = Title + ": " + Description + "\nArguments: " + string.Join(" ", Args);
                return message;
            }                   
        }
            
        protected string InvalidArgMessage { get { return "Invalid command arguments. " + Help; } } 
        public abstract DebugConsole.AccessLevel RequiredLevel { get; protected set; }
        public abstract void Execute(string[] args);
    }
}
