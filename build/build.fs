open Fake.Core
open Fake.DotNet
open Fake.DotNet.Testing
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

let src = __SOURCE_DIRECTORY__ </> ".." </> "src"
let bin = __SOURCE_DIRECTORY__ </> ".." </> "bin"
let tests = __SOURCE_DIRECTORY__ </> ".." </> "tests"

let setTestsOpts (opts: DotNet.TestOptions) = {
  opts with
    Output = Some tests;
    ResultsDirectory = Some $"{tests}/results";
    Logger = Some "html;LogFileName=result.html";
    NoRestore = true;
    NoBuild = true;
  }

let setUnitTestsFilter (opts: DotNet.TestOptions) =
  { opts with Filter = Some "FullyQualifiedName~.Tests.Unit."  }

let initTargets () =
  Target.create "Clean" (fun _ ->
      !! $"{src}/**/bin"
      ++ bin
      ++ tests
      -- $"{src}/**/obj"
      |> Shell.cleanDirs
  )
  
  Target.create "BuildApp" (fun _ ->
      !! $"{src}/Clean.App/*.*proj"
      |> Seq.iter (DotNet.build (fun opts ->
        { opts with OutputPath = Some bin }))
  )
  
  Target.create "BuildTests" (fun _ ->
      !! $"{src}/Clean.Tests/*.*proj"
      |> Seq.iter (DotNet.build (fun opts ->
        { opts with OutputPath = Some tests }))
  )
  
  Target.create "RunTests" (fun p ->
      let unitOnly = p.Context.Arguments |> Seq.contains "--unit-only"
      let optsBuilder =
        match unitOnly with
        | true -> setTestsOpts >> setUnitTestsFilter
        | false -> setTestsOpts
      !! $"{src}/Clean.Tests/*.*proj"
      |> Seq.iter (DotNet.test optsBuilder)
  )
  
  Target.create "Restore" (fun p ->
    !! $"{src}/**/*.*proj"
    |> Seq.iter (DotNet.restore id)
  )
  
  Target.create "All" ignore
  
  ("Clean" ==> "BuildApp") <=> ("Clean" ==> "BuildTests" ==> "RunTests") ==> "All"

[<EntryPoint>]
let main argv =
  argv
  |> Array.toList
  |> Context.FakeExecutionContext.Create false "build.fsx"
  |> Context.RuntimeContext.Fake
  |> Context.setExecutionContext
  initTargets () |> ignore
  Target.runOrDefaultWithArguments "All"
  0
