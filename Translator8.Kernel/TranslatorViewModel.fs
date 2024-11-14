namespace Translator8.Kernel

open Translator8.Kernel

type TranslatorViewModel() =
    inherit NotifyPropertyChanged()

    let mutable sentances: SentanceViewModel[] = [||]

    member this.Sentances
        with get() = sentances
        and set value =
            this.Update(value,&sentances)
