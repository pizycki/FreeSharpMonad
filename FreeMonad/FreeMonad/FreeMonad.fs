module FreeMonad
open System


type UserId = 
    UserId of Guid

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
| Free
| Pure


[<EntryPoint>]
let main argv =
    printfn "%A" argv
    0 // return an integer exit code
