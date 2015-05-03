// include Fake lib

#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.ReleaseNotesHelper
open Fake.AssemblyInfoFile
open Fake.Git

// Properties

// Scaffold Properties
let srcDir = "./src/"
let buildDir = "./build/"
let testDir = "./tests/"
let libDir = "./lib/"
let binDir = "./bin/"
let docDir = "./docs/"

// Filesets Properties
let appReferences = !! "src/app/**/*.csproj"
let testReferences = !! "src/tests/**/*.csproj"

// Information about the project are used
//  - for version and project name in generated AssemblyInfo file
//  - by the generated NuGet package

// The name of the project
// (used by attributes in AssemblyInfo, name of a NuGet package)
let project = "NetConsole.WebApi"

// Short summary of the project
// (used as description in AssemblyInfo and as a short summary for NuGet package)
let summary = "NetConsole.WebApi is a package that it provides a way to access commands by using a sort of RPC-like API interaction."

// Read additional information from the release notes document
let release = LoadReleaseNotes "RELEASE_NOTES.md"

Target "Clean" (fun _ ->
        CleanDirs [buildDir; testDir; binDir; docDir]
)

// Generate assembly info files with the right version & up-to-date information
Target "AssemblyInfo" (fun _ ->
    let getAssemblyInfoAttributes projectName =
        [ Attribute.Title (projectName)
          Attribute.Product project
          Attribute.Description summary
          Attribute.Version release.AssemblyVersion
          Attribute.FileVersion release.AssemblyVersion ]

    let getProjectDetails projectPath =
        let projectName = System.IO.Path.GetFileNameWithoutExtension(projectPath)
        ( projectPath,
          projectName,
          System.IO.Path.GetDirectoryName(projectPath),
          (getAssemblyInfoAttributes projectName)
        )

    !! "src/**/*.??proj"
    |> Seq.map getProjectDetails
    |> Seq.iter (fun (projFileName, projectName, folderName, attributes) ->
            CreateCSharpAssemblyInfo ((folderName @@ "Properties") @@ "AssemblyInfo.cs") attributes
        )
)

Target "BuildApp" (fun _ ->
    MSBuildRelease "" "Rebuild" appReferences
            |> Log "AppBuild-Output: "
    MSBuildRelease buildDir "Rebuild" appReferences
            |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    MSBuildDebug testDir "Rebuild" testReferences
            |> Log "TestBuild-Output: "
)

Target "Test" (fun _ ->
        !! (testDir + "*Tests*.dll")
            |> NUnit (fun p ->
                {
                    p with
                        DisableShadowCopy = true;
                        OutputFile = testDir + "TestResults.xml"
                }
            )
)

Target "BuildPackage" (fun _ ->
    Paket.Pack(fun p ->
        { p with
            OutputPath = binDir
            Version = release.NugetVersion
            ReleaseNotes = toLines release.Notes})
)

Target "BuildZip" (fun _ ->
        !! (buildDir + "/**/*.*")
            -- "*.zip"
            |> Zip buildDir (binDir + project + "." +  release.NugetVersion + ".zip" )
)

Target "ReleasePackage" (fun _ ->
    Paket.Push(fun p ->
        { p with
            WorkingDir = binDir
        })
)

Target "Master" (fun _ ->
    trace "Build completed"
)

// Default target
Target "Develop" (fun _ ->
        trace "Build completed"
)

"Clean"
    ==> "AssemblyInfo"
    ==> "BuildApp"
    ==> "BuildTest"
    ==> "Test"
    ==> "BuildZip"
    ==> "BuildPackage"
    ==> "Develop"

"Clean"
    ==> "BuildApp"
    ==> "BuildTest"
    ==> "Test"
    ==> "BuildZip"
    ==> "BuildPackage"
    ==> "Master"

// start build
RunTargetOrDefault "Develop"
