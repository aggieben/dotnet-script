namespace BitThicket.DotNet.ScriptTool
{
    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            var options = Options.Default;

            string scriptPath = null;
            IReadOnlyList<string> scriptArgs = null;

            ArgumentSyntax.Parse(args, syntax => 
            {
                syntax.DefineOption("l|loglevel", ref options.LogLevel, ParseLogLevel, "log level, one of [trace, debug, information, warning, error, critical, none]");

                syntax.DefineParameter("scriptPath", ref scriptPath, "script to run.  (required)");
                syntax.DefineParameterList("scriptArguments", ref scriptArgs, "arguments to be passed through to the script");
            });        

            var loggerFactory = new LoggerFactory()                
                .AddConsole(options.LogLevel)
                // TODO: add TraceSource logger here
                ;
            var logger = loggerFactory.CreateLogger("dotnet-script");

            var assPath = Path.GetDirectoryName(
                Assembly.GetEntryAssembly()
                        .GetReferencedAssemblies()
                        .Select(a => Assembly.Load(a).Location)
                        .First());

            logger.LogDebug($"Assembly path: {assPath}");

            var scriptOptions = ScriptOptions.Default
                .WithReferences(
                    typeof(System.Object).GetTypeInfo().Assembly,                                           // mscorlib
                    typeof(System.Runtime.InteropServices.RuntimeInformation).GetTypeInfo().Assembly,       // System.Runtime.?
                    typeof(System.Collections.Generic.IEnumerable<>).GetTypeInfo().Assembly,                // ?
                    typeof(System.Console).GetTypeInfo().Assembly,                                          // System.Console
                    typeof(System.IO.File).GetTypeInfo().Assembly,                                          // System.IO.FileSystem
                    typeof(System.Linq.Enumerable).GetTypeInfo().Assembly,                                   // System.Linq
                    typeof(ILogger).GetTypeInfo().Assembly
                )
                .WithImports("Microsoft.Extensions.Logging")
                .WithMetadataResolver(ScriptMetadataResolver.Default.WithSearchPaths(assPath));
            
            var scriptText = File.ReadAllText(scriptPath);
            var script = CSharpScript.Create(scriptText, scriptOptions, typeof(Globals));

            try
            {
                script.RunAsync(new Globals
                { 
                    Args = scriptArgs.ToArray(),
                    Logger = loggerFactory.CreateLogger(Path.GetFileName(scriptPath)),
                    RuntimeDir = assPath
                })
                .GetAwaiter().GetResult();
            }
            catch (CompilationErrorException cee)
            {
                Console.Error.WriteLine("Script error: {0}", cee);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Execution error: {0}", e);
            }
        }

        private static LogLevel ParseLogLevel(string val)
        {
            LogLevel level = LogLevel.Information;
            Enum.TryParse(val, true, out level);
            return level;
        }
    }
}
