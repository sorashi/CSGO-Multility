Deploying
===

0. Be sure to have all NuGet packages specified in the [`packages.config`](Multility/packages.config) file available for use. To achieve that, simply use the [`nuget restore`](https://docs.nuget.org/consume/package-restore#automatic-package-restore-in-visual-studio) command.
1. Edit the [`.nuspec` file](Multility.nuspec) to contain all EXEs, DLLs and `exe.config` needed to run
2. In Visual Studio, switch to 'SquirrelRelease' configuration and press Build. It will create a folder called `Releases` in the solution directory. The folder contains all the generated files. Note that the version is fetched from the assembly version, so be sure to change that in the `AssemblyInfo.cs` file
3. Make a GitHub release - add files from the `Releases` folder, mainly the `Setup.exe` file.Those `.nupkg` "full" and "delta" files are important for the autoupdater. Include the `.nupkg` of the most recent release and a few previous "delta" and "full" files, since the updater then can download only the difference between previous and actual version.

---

# What does exactly happen in the SquirrelRelease building process? (for future back-reference)

In the [`Multility.csproj`](Multility/Multility.csproj), you can see a few lines at the end:

```
  <Target Name="AfterBuild" Condition=" '$(Configuration)' == 'SquirrelRelease'">
    <GetAssemblyIdentity AssemblyFiles="$(TargetPath)">
      <Output TaskParameter="Assemblies" ItemName="myAssemblyInfo" />
    </GetAssemblyIdentity>
    <Exec Command="$(SolutionDir)releasify.bat &quot;$(SolutionDir)&quot; &quot;%(myAssemblyInfo.Version)&quot;"/>
  </Target>
```

These call the batch file [`releasify.bat`](releasify.bat) with two arguments:

1. The solution directory path
2. The assembly version

The batch file does a few things:

1. It finds the [Squirrel](https://github.com/Squirrel/Squirrel.Windows/) and NuGet tools executables (regardless of the current version e.g. squirrel.windows.1.3.0)
2. It builds a NuGet package from the `.nuspec` file
3. It uses [Squirrel](https://github.com/Squirrel/Squirrel.Windows/) to build the released files