namespace Translator8.Kernel

open System.Collections.Generic
open System.Text.RegularExpressions

type Sentence(tokens: Token[]) =
    member this.Tokens = tokens

    member _.Text =
        [ for tk in tokens -> tk.Lexeme ]
        |> String.concat ""

module Sentence =

    let toPhrasesViewModel (words: Dictionary<string, string>) (olds: Map<Phrase, string>) (tokens: Token[]) =

        let engs =
            words.Keys
            |> Seq.map (fun e -> e.ToLowerInvariant())
            |> Set.ofSeq

        let newPhrases = PhrasePartition.phrases (engs |> Set.toArray, tokens)

        [
            for phrase in newPhrases do
                //设置新短语的中文候选项
                let chineseCandidates =
                    if words.ContainsKey phrase.Text then
                        words.[phrase.Text] |> Chinese.toItems
                    else
                        [||]

                let old =
                    if olds.ContainsKey(phrase) then
                        Some olds.[phrase]
                    else
                        None

                let head = chineseCandidates |> Seq.tryHead

                //中文词义当前项
                let chinese =
                    match old, head with
                    | Some old, Some head ->
                        chineseCandidates
                        |> Array.tryFind ((=) old)
                        |> Option.defaultValue head
                        |> Some
                    | _ -> head

                //设置完毕，添加到集合
                yield phrase, chineseCandidates, chinese
        ]

    let from (input:string) = 
        input
        |> Token.formatInputText
        |> Token.split
        |> Seq.toArray
        |> Token.sentences



