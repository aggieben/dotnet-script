namespace BitThicket.DotNet.ScriptTool
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Print Help Here");
                return;
            }

            var scriptText = File.ReadAllText(args[0]);

            var scriptOptions = ScriptOptions.Default
                .WithReferences(
                    typeof(System.Object).GetTypeInfo().Assembly,                                           // mscorlib
                    typeof(System.Runtime.InteropServices.RuntimeInformation).GetTypeInfo().Assembly,       // System.Runtime.?
                    typeof(System.Collections.Generic.IEnumerable<>).GetTypeInfo().Assembly,                // ?
                    typeof(System.Console).GetTypeInfo().Assembly,                                          // System.Console
                    typeof(System.Diagnostics.Trace).GetTypeInfo().Assembly,                                // System.Diagnostics.TraceSource
                    typeof(System.IO.File).GetTypeInfo().Assembly,                                          // System.IO.FileSystem
                    typeof(System.Linq.Enumerable).GetTypeInfo().Assembly,                                  // System.Linq
                    typeof(Newtonsoft.Json.JsonConvert).GetTypeInfo().Assembly                              // Newtonsoft.Json.NetCore
                );

            var script = CSharpScript.Create(scriptText, scriptOptions, typeof(Globals));

            try
            {
                var scriptArgs = new string[args.Length-1];
                Array.Copy(args, 1, scriptArgs, 0, args.Length-1);

                script.RunAsync(new Globals
                { 
                    Args = scriptArgs
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
    }
}
