module FreeMonad
open System


type UserId = UserId of int
    
type CreateUserRequest = {
    Name: string
}

type UserEntity = {
    Id: UserId;
    Name: string
}

type IUserRepository =
    abstract member Create : user:CreateUserRequest -> UserId
    abstract member Get : userId:UserId -> UserEntity

// Start impl of FreeMonad

type UserRepositoryInstruction<'a> = 
| Create of (CreateUserRequest * (UserId -> 'a))
| Get of (UserId * (UserEntity -> 'a))

let private mapI f = function 
    | Create (x, next) -> Create(x, next >> f)
    | Get (x, next) -> Get(x, next >> f)

type UserRepositoryProgram<'a> =
| Free of UserRepositoryInstruction<UserRepositoryProgram<'a>>
| Pure of 'a

let rec bind f = function 
| Free x -> x |> mapI (bind f) |> Free
| Pure x -> f x

type UserRepoBuilder () =
    member this.Bind (x, f) = bind f x
    member this.Return x = Pure x
    member this.ReturnFrom x = x
    member this.Zero () = Pure ()

    // TODO Combine, For, TryFinally

// Factories
let createUser user = Free (Create (user, Pure))
let getUser userId = Free (Get (userId, Pure))


// Implementations 
let InsertUserRow (req:CreateUserRequest) = UserId 42
let Select1Row (userId:UserId) = {
    Id = UserId 42;
    Name = "Pawel Izycki"
}

// Interpreter
let rec interpret = function
| Pure x -> x
| Free (Create (x, next)) -> x |> InsertUserRow |> next |> interpret
| Free (Get (x, next)) -> x |> Select1Row |> next |> interpret

[<EntryPoint>]
let main argv =
    printfn "%A" argv
    0 // return an integer exit code
