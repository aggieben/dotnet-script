namespace ConsoleApplication
{
    using System.IO;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Microsoft.CodeAnalysis.Scripting;
    public class Program
    {
        public static void Main(string[] args)
        {
            var scriptText = File.ReadAllText(args[0]);

            var scriptOptions = ScriptOptions.Default.AddImports(
                "System"
            );

            var script = CSharpScript.Create(scriptText, scriptOptions);

            script.RunAsync().GetAwaiter().GetResult();
        }
    }

    
}
