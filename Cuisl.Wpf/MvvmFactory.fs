module Cuisl.MvvmFactory

open System
open System.Windows.Input
open System.ComponentModel.DataAnnotations

let getCommand(exec) =
    let event = new Event<EventHandler, EventArgs>()
    { new ICommand with
            member x.Execute(obj) = exec obj
            member x.CanExecute(obj) = true
            [<CLIEvent>]
            member x.CanExecuteChanged = event.Publish
    }

let ValidateProperty(this,propName,value) =
    let vc = new ValidationContext(this, null, null)
    vc.MemberName <- propName
    Validator.ValidateProperty(value, vc)

let getErrorMessage<'a>(this:'a,propName) =
    let vc = new ValidationContext(this, null, null)
    vc.MemberName <- propName
    let res = new ResizeArray<_>()
    Validator.TryValidateProperty(typeof<'a>.GetProperty(propName).GetValue(this, null), vc, res) |> ignore
    if res.Count > 0 then
        [
            for vr in res do
                yield vr.ErrorMessage
        ]|> String.concat Environment.NewLine
    else
        String.Empty