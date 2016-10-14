// ===================================================================================================
// Front of House (FOH) Build Script
// For more info and docs: http://cakebuild.net
// ===================================================================================================
#addin "Cake.Powershell"
#addin "nuget:?package=Cake.Watch"

// ===================================================================================================
// Command line parameters (second parameter in the Argument call is the default if none are specified)
// ===================================================================================================

// The starting target to run
var target = Argument("target", "Build");

// Version number arguments
var majver = Argument("MajVer", "0");
var minver = Argument("MinVer", "0");
var patch = Argument("Patch", "0");
var revno = Argument("RevNo", "0");

// Build configuration properties
var configuration = Argument("configuration", "Release");

// The directory to output the release build artifacts to.
var outdir = Argument("OutputDirectory", "./bld");

// ====================================================================================================
// Tasks
// ====================================================================================================

Task("Parameters")
    .Does(() => {
        Information("Target: " + target);
        Information("Major Version: " + majver);
        Information("Minor Version: " + minver);
        Information("Patch Number: " + patch);
        Information("Revision Number: " + revno);
        Information("Configuration: " + configuration);
        Information("Output Directory: " + outdir);
    });

// ================================================================
// Clean (Combining done.)
// ================================================================

Task("Clean")
    .IsDependentOn("Parameters")
    .Does(() => {
        MSBuild(@"./BroccoliTaskRunner.sln", new MSBuildSettings()
            .WithTarget("Clean"));
        EnsureDirectoryExists(outdir);
        CleanDirectories(outdir);
    });

Task("Set-Assembly-Versions")
    .Does(() => {
        if(configuration == "Release"){
            StartPowershellFile("./assemblyInfo.ps1", args => {
                    args.Append("fileFilter", @"*AssemblyInfo.cs");
                    args.Append("lpath", ".");
                    args.Append("buildnum", string.Format("{0}.{1}.{2}.{3}", majver, minver, patch, revno));
                });
        }
    });

// ================================================================
// Build 
// ================================================================

Task("Restore-NuGet-Packages")
  .IsDependentOn("Clean")
  .Does(() => {
    Information("Restoring NuGet packages for all projects.");
    NuGetRestore(@"./BroccoliTaskRunner.sln");
});

Task("Build")
    .IsDependentOn("Set-Assembly-Versions")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        var msbuildSettings = new MSBuildSettings() {
            Verbosity = Verbosity.Minimal,
            Configuration = configuration
        };
        MSBuild(@"./BroccoliTaskRunner.sln", msbuildSettings);
    });




// Default task that covers running from the command line without the -Target argument.
Task("Default")
    .IsDependentOn("Build");

// Startup
RunTarget(target);