namespace Translator.Kernel

type Sentance(tokens:Token[]) =
    member this.Tokens = tokens
    member _.Text =
        [for tk in tokens -> tk.Lexeme]
        |> String.concat ""
