using Microsoft.Extensions.Logging;

namespace BitThicket.DotNet.ScriptTool
{
    internal struct Options
    {
        public LogLevel LogLevel;

        public static Options Default { get; } = new Options()
        {
            LogLevel = LogLevel.Information
        };
    }
}