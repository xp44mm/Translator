namespace Translator8.Kernel

open System.Windows

type SentanceViewModel(tokens: Token [], text: string) =
    inherit NotifyPropertyChanged()

    //短语项目间有空格
    let mutable phraseItems: PhraseItem [] = [||]
    let mutable transla = ""

    member _.Tokens = tokens

    member _.Text = text

    member this.PhraseItems
        with get () = phraseItems
        and set value =
            this.Update(value, &phraseItems)
            this.Updated <- true

    ///中文译文
    member this.Transla
        with get () = transla
        and set value = this.Update(value, &transla)

    member val Updated = false with get, set

    static member from(text:string) =
        Essay(text).Sentances
        |> Array.map(fun x -> 
            SentanceViewModel(x.Tokens, x.Text)
            )

