# Dapper-Tools
Repo for code that I usually end up writing over again and again when working on multiple different kinds of games so I decided to create a custom Unity package to make life easier. Feel free to make use of this yourself and I am happy to consider pull requests whilst also fixing reported issues.

# Installing with Unity Package Manager

Make sure you have your Api Compatibility Level as .NET 4.x (Edit/Project Settings/Player/Configuration)
![Api Compatibility](https://github.com/DapperDino/Dapper-Tools/blob/master/ApiCompatibility.png)

To install this project as a [Git dependency](https://docs.unity3d.com/Manual/upm-git.html) using the Unity Package Manager,
add the following line to your project's `manifest.json`:

```
"com.dapperdino.dappertools": "https://github.com/DapperDino/Dapper-Tools.git"
```

You will need to have Git installed and available in your system's PATH.

If you are using [Assembly Definitions](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html) in your project, you will need to add `DapperTools` and/or `DapperToolsEditor` as Assembly Definition References.
