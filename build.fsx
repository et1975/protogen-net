// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.ReleaseNotesHelper

// Properties
// build output folder
let build_out = "build_out" 
// Read additional information from the release notes document
let release = LoadReleaseNotes "RELEASE_NOTES.md"

// Targets
Target "Clean" (fun _ ->
    CleanDir build_out
)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target "NuGet" (fun _ ->
    Paket.Pack(fun p -> 
        { p with
            OutputPath = build_out
            TemplateFile = "paket.template"
            Version = release.NugetVersion
            ReleaseNotes = toLines release.Notes})
)

Target "PublishNuget" (fun _ ->
    Paket.Push(fun p -> 
        { p with
            WorkingDir = build_out })
)

"Clean"
  ==> "NuGet"


// start build
RunTargetOrDefault "NuGet"