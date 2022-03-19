namespace Translator.Kernel

open Translator.Kernel

type TranslatorViewModel() =
    inherit NotifyPropertyChanged()

    let mutable sentances: SentanceViewModel[] = [||]

    member this.Sentances
        with get() = sentances
        and set value =
            this.Update(value,&sentances)




    ////修改为句子可变量？
    //member this.UpdateSentances(inp:string) =
    //    this.Sentances <- TranslatorViewModel.getSentances inp
    /////inp是粘贴的文本，将其翻译成句子视图。
    //static member getSentances(inp:string) =
    //    let input = Token.formatInputText(inp);
    //    let tokens = Token.split(input) |> Seq.toArray
    //    [|
    //        for tks in Token.sentances(tokens) do
    //            yield new SentanceViewModel(Tokens = tks,Updated = false)
    //    |]
