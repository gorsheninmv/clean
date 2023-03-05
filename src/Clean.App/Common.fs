namespace Clean.App

[<AutoOpen>]
module Monads =

  let private bind x f =
    match x with
    | Some success -> f success
    | None _ -> None

  type MaybeBuilder() =
    member this.Zero() = None
    member this.Bind(x, f) = bind x f
    member this.Return(x) = Some x

  type MaybeBuilder2() =

    member this.Bind(x, f) =
      match x with
      | None -> None
      | Some a -> f a

    member this.Return(x) = Some x

  let maybe = MaybeBuilder()
