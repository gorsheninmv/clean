namespace WeatherBot.Tests

open NUnit
open NUnit.Framework
open FsUnit

module Unit =

  [<Test>]
  let ``check if unit tests works should pass`` () = 2 + 2 |> should equal 4
