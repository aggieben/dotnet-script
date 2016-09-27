using Microsoft.Extensions.Logging;

namespace BitThicket.DotNet.ScriptTool
{
    public class Globals
    {
        public string[] Args { get; internal set; }
        public ILogger Logger { get; internal set; }
        public string RuntimeDir { get; internal set; }
    }
}