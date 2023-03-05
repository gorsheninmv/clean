namespace Clean.App

[<AutoOpen>]
module Clean =
  open System
  open System.IO
  open System.IO.Abstractions

  let inline combine path1 path2 = Path.Combine(path1, path2)
  let inline (</>) path1 path2 = combine path1 path2

  let private pickOldFiles (files: seq<FileInfo>) =
    let now = DateTime.Now.ToUniversalTime()
    files |> Seq.filter (fun file -> file.LastWriteTimeUtc.AddDays(30) < now)

  let private removeOldFiles (files: seq<FileInfo>) =
    files |> Seq.iter (fun file -> file.Delete())

  let private getDirInfo path =
    if Directory.Exists(path) then
      let dirInfo = DirectoryInfo(path)
      Some dirInfo
    else
      None

  let clean (fs: IFileSystem) =
    let path = "D:" </> "tmp"

    maybe {
      let! dirInfo = getDirInfo path
      let dirInfo = DirectoryInfo(path)
      let opts = EnumerationOptions()
      opts.RecurseSubdirectories <- true
      dirInfo.GetFiles("*.*", opts) |> removeOldFiles
    }
    |> ignore

  let fs = FileSystem()
  clean fs
