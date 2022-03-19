module Translator.Kernel.PhrasePartition

open System.Text.RegularExpressions
open System

///模糊查询
let search (engs:string Set) (phrase:string) =
    let rgx = new Regex("^"+Regex.Escape(phrase), RegexOptions.IgnoreCase)
    engs
    |> Set.filter(fun eng -> rgx.IsMatch(eng))

///根据逆序的代表phrase的临时Tokens生成短语
let phraseText (revTokens:Token list) =
    revTokens
    |> List.map(fun tk -> tk.Lexeme)
    |> List.reduce(fun s1 s2 -> s2 + s1)

///尝试使短语最长，此时，第n-1个token可以在词典中查到项目，并且在第n个查不到项目
///pTokens代表phrase Tokens，代表一个短语。pTokens不断从tokens读取token组成短语
let rec lengthen (engs:string Set) (pTokens:Token list) (tokens:Token list) =
    //如果在词典中查不到短语, tokens到达末尾，或者被强制打断
    if engs.IsEmpty || tokens.IsEmpty || tokens.Head.IsStarted then
        pTokens,tokens
    else
        let pTokens,tokens = (tokens.Head:: pTokens), tokens.Tail
        let engs = pTokens |> phraseText |> search engs
        lengthen engs pTokens tokens

///根据给出的英语词典中的Key，就是英语短语，对tokens划分成短语。
let phrases (engs:string[], tokens:Token []) =
    let engs = Set.ofArray engs

    ///获取tokens开头的第一个短语
    let rec firstPhrase (accTokens:Token list) (tokens:Token list) =
        match tokens with
        | [] -> accTokens |> List.rev,[]
        | head :: tail when tail.IsEmpty || tail.Head.IsStarted
            -> (head::accTokens)|> List.rev,tail
        | head :: tail ->
            //用tokens的第一个单词模糊查询词典
            let engs = search engs head.Lexeme
            //精确匹配
            let rec shorten (pTokens:Token list,tokens:Token list) =
                match pTokens with
                | [_] | [] ->
                    //已经回退到最后，返回这个状态
                    pTokens,tokens
                | head::tail ->
                    let phrase = pTokens |> phraseText
                    let found =
                        engs |> Set.exists(fun eng -> eng.Equals(phrase,StringComparison.OrdinalIgnoreCase))
                    //如果在词典中发现短语，就返回这个状态。
                    if found then
                        pTokens |> List.rev,tokens
                    else
                        //退回一个token，递归
                        let tokens,pTokens = (head::tokens), tail
                        shorten (pTokens, tokens)

            let accTokens,tokens =
                lengthen engs [head] tail
                |> shorten
            accTokens,tokens

    let rec allPhrases (accPhrases:Token list list) (tokens:Token list) =
        match tokens with
        | [] -> accPhrases |> List.rev
        //丢弃开头的空格
        | head::tail when head.Lexeme = " " -> allPhrases accPhrases tail
        | _ ->
            let phrase,tokens = firstPhrase [] tokens
            allPhrases (phrase::accPhrases) tokens

    allPhrases [] <| List.ofArray tokens
    |> List.map(fun tokens -> new Phrase(tokens|> List.toArray))
