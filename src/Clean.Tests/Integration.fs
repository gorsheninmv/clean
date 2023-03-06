namespace Clean.Tests

open NUnit
open NUnit.Framework
open FsUnit

open System
open System.Linq
open System.IO.Abstractions.TestingHelpers

open Clean.App

module Integration =

  let createMockFileData modifiedDate =
    let ret = MockFileData([||])
    ret.LastWriteTime <- modifiedDate
    ret

  let createInitialFs () =
    let oldDate = DateTimeOffset.UtcNow.AddDays(-40.0)
    let files = dict [
      ("/temp" </> "oldemptydir", createMockFileData oldDate);
      ("/temp" </> "olddirwithcontent" </> "file1.json", createMockFileData oldDate);
      ("/temp" </> "olddirwithcontent" </> "nesteddir" </> "file1.json", createMockFileData oldDate);
    ]

    let fs = MockFileSystem(files)
    fs

  [<Test>]
  let ``clean with not existed directory should work`` () =
    let fs = MockFileSystem()
    clean fs

  [<Test>]
  let ``clean old files`` () =
    let fs = createInitialFs ()
    clean fs
    fs.AllFiles.Count() |> should equal 0
