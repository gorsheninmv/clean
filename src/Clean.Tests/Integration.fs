namespace Clean.Tests

open NUnit
open NUnit.Framework
open FsUnit

open System.IO.Abstractions.TestingHelpers

open Clean.App

module Integration =

  [<Test>]
  let ``clean with empty folder should work`` () =
    let fs = MockFileSystem()
    clean fs
