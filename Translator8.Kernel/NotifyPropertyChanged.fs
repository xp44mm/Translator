namespace Translator8.Kernel

open System
open System.ComponentModel
open System.Runtime

type NotifyPropertyChanged() =
    let propChangedEvent = new Event<PropertyChangedEventHandler,PropertyChangedEventArgs>()

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = propChangedEvent.Publish

    ///set property's backing field and notify property changing and changed:
    /// field <- value
    member this.Update<'T>(value:'T, field:byref<'T>, [<CompilerServices.CallerMemberName>]?propertyName) =
        let propertyName = defaultArg propertyName ""
        if Object.Equals(field, value) |> not then
            field <- value
            propChangedEvent.Trigger(this, PropertyChangedEventArgs(propertyName))

