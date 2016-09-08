using Microsoft.Extensions.Logging;

namespace BitThicket.DotNet.ScriptTool
{
    public class Globals
    {
        public string[] Args { get; set; }
        public ILogger Logger { get; set; }
    }
}