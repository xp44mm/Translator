module Translator.Kernel.Translation

open System.Collections.Generic
open System.Linq
open System.Text.RegularExpressions

//保存当前选择
let keepSelection(phraseItems:PhraseItem[]) =
    ///因为是不可变的，所以，这里区分大小写也没有关系
    phraseItems
    |> Array.filter(fun pi -> pi.Chinese <> null)
    |> Array.map(fun pi ->(pi.Phrase.Position,pi.Phrase.Text), pi.Chinese)

//词典更新，重新翻译句子
let Update(words:Dictionary<string,string>,tokens:Token[],olds:((int*string)*string)[]) =
    //英语单词集合
    let engs = words.Keys.ToArray()

    //重新生成短语
    let newPhrases = PhrasePartition.phrases(engs, tokens)

    let olds = olds |> Map.ofArray
    [|
        for phrase in newPhrases do
            //设置新短语的中文候选项
            let chineseCandidates =
                if words.ContainsKey phrase.Text then
                    Regex.Split(words.[phrase.Text], @"\r?\n")
                else
                    [||]

            //中文词义当前项
            let chinese =
                if Array.isEmpty chineseCandidates then
                    null
                elif olds.ContainsKey(phrase.Position,phrase.Text) then
                    let previusSelected = olds.[phrase.Position,phrase.Text]

                    chineseCandidates 
                    |> Array.tryFind((=) previusSelected)
                    |> Option.defaultValue chineseCandidates.[0]
                else chineseCandidates.[0]

            //设置完毕，添加到集合
            yield new PhraseItem(Phrase = phrase,
                                ChineseCandidates = chineseCandidates,
                                Chinese = chinese)
    |]
