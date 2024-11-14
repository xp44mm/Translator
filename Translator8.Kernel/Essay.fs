namespace Translator8.Kernel

///从剪贴板粘贴英文文本，分解为单词，划分为句子。
type Essay(input:string) = 
    let text = Token.formatInputText(input)
    let tokens = Token.split(text) |> Seq.toArray
    let sentances =
        Token.sentances(tokens)
        |> Array.map(fun tks -> Sentance(tks))

    member this.Text = text
    member this.Tokens = tokens
    member this.Sentances = sentances
