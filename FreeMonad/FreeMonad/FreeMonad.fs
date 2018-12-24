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

type UserRepositoryInstruction = 
    | 
    |


[<EntryPoint>]
let main argv =
    printfn "%A" argv
    0 // return an integer exit code