namespace Translator.Kernel

//代表英文中文对照
type PhraseItem() =
    //英文短语
    member val Phrase = new Phrase() with get, set
    member val ChineseCandidates:string[] = [||] with get, set
    member val Chinese = "" with get, set

    member this.IsStart = this.Phrase.Tokens.[0].IsStarted

