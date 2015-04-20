// include dll's

#r @"packages/FAKE/tools/FakeLib.dll"
open Fake

// Scaffold Properties
let srcDir = "./src/"
let buildDir = "./build/"
let testDir = "./tests/"
let libDir = "./lib/"
let binDir = "./bin/"
let docDir = "./docs/"

let setupFile = "init.fsx"

// Targets
Target "CreateScaffold" (fun _ ->
        srcDir |> CreateDir
        srcDir + "app/" |> CreateDir
        srcDir + "tests/" |> CreateDir
        libDir |> CreateDir
        binDir |> CreateDir
        docDir |> CreateDir
        buildDir |> CreateDir
        testDir |> CreateDir
)

Target "Setup" (fun _ ->
    trace "Setup completed"
)

Target "TearDown" (fun _ ->
        setupFile |> DeleteFile
)

// Dependencies
"CreateScaffold"
    ==> "TearDown"
    ==> "Setup"

// start build
RunTargetOrDefault "Setup"
