This project was inspired by [ScriptCs](http://scriptcs.net) and [Roslyn](https://github.com/dotnet/roslyn).  I needed a way to execute a universal script as a part of the build process in .NET projects using the .NET Core tooling (.NET CLI).  While there are a number of scripting languages available, most require at least to be installed.

When considering alternatives to writing multiple versions of scripts in order to cover the platforms I care about (Bash on Linux, MacOSX, and PowerShell on Windows) and resorting to tricks that would coincidentally work because we're lucky, it seemed like we could take advantage of the .NET platform, because we already have it (by definition).  

The Roslyn scripting API is compatible with .NET Core, so it seemed reasonably doable to _quickly_ throw together a `dotnet-script` tool to make this usable.

This is the result.  Binary packages are available on `https://www.myget.org/F/aggieben/api/v2` (v2 API) or `https://www.myget.org/F/aggieben/api/v3/index.json` (v3 API).

I'm calling this a "Beta 1" for now, in the spirit of the .NET Core betas, which means anything could change between now and the next commit (:trollface:).

To use this tool, reference it in your `project.json` "tools" section, like this:
```json
{
  // ...blahblah - I know this isn't proper JSON
  "tools": {
    "BitThicket.DotNet.ScriptTool": "0.1.0-beta-1-*"
  }
```
This will make the command available in your project directory(-ies) as `dotnet script`.  It takes one argument, the path of a script file.  I'm using `.csx` for script files, so for me it would look like this:

`dotnet script test.csx`

I'm just using the Roslyn Scripting API, so presumably most of what it supports would be doable in a .csx script.  Mark Michaelis wrote a pretty [good description](https://msdn.microsoft.com/en-us/magazine/mt614271.aspx) of what you can do in a script here.
