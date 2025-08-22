module Translator8.Kernel.Tokens

let text (tokens:Token seq) =
    [for tk in tokens -> tk.Lexeme]
    |> String.concat "" // 因为空白也是一个token所以直接连接
