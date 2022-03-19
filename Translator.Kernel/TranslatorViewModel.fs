namespace Translator.Kernel

open Translator.Kernel

type TranslatorViewModel() =
    inherit NotifyPropertyChanged()

    let mutable sentances: SentanceViewModel[] = [||]

    member this.Sentances
        with get() = sentances
        and set value =
            this.Update(value,&sentances)
