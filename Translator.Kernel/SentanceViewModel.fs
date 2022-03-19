namespace Translator.Kernel

open System
open System.Text.RegularExpressions

open Translator.ef
open Translator.Kernel

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

////找到原有词义
//member this.olds() =
//    ///因为是不可变的，所以，这里区分大小写也没有关系
//    this.PhraseItems
//    |> Array.filter(fun pi -> pi.Chinese <> null)
//    |> Array.map(fun pi ->pi.Phrase, pi.Chinese)
//    |> Map.ofArray
////词典更新，重新翻译句子
//member this.Translate(words:Word[]) =
//    //英语单词集合
//    let engs = words |> Array.map(fun w -> w.English)
//    //重新生成短语
//    let newPhrases = PhrasePartition.phrases(engs, this.Tokens)
//    let olds = this.olds()
//    let newPhraseItems =
//        [|for phrase in newPhrases do
//              //设置新短语的中文候选项
//              let chineseCandidates =
//                  [|for word in words do
//                        if word.English.Equals
//                               (phrase.Text,
//                                StringComparison.OrdinalIgnoreCase) then
//                            yield word.Chinese|]
//                  |> fun arr ->
//                      if Array.isEmpty arr then [||]
//                      else Regex.Split(arr.[0], @"\r?\n")
//              //中文词义当前项
//              let chinese =
//                  if olds.ContainsKey phrase
//                     && chineseCandidates |> Array.exists((=) olds.[phrase]) then
//                      olds.[phrase]
//                  else if Array.isEmpty chineseCandidates then null
//                  else chineseCandidates.[0]
//              //设置完毕，添加到集合
//              yield new PhraseItem(Phrase = phrase,
//                                   ChineseCandidates = chineseCandidates,
//                                   Chinese = chinese)
//        |]
//    this.PhraseItems <- newPhraseItems
