namespace Translator.Kernel
open System.Text.RegularExpressions

//https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/character-classes-in-regular-expressions
type Token(pos:int,lex:string) =
    member this.Position = pos
    member this.Lexeme = lex
    //手工断开短语
    member val IsStarted = false with get, set

open FSharp.Idioms

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Token =

    //连续空格、连续字母、连续数字、其他单个字符
    let private trimStart inp =
        match inp with

        | "" -> None

        | Prefix @"[\p{Z}]+" (_,rest) -> Some(" ",rest)

        //缩写 abbr
        | Prefix @"(A\.D\.|A\.M\.|a\.k\.a\.|B\.C\.|e\.g\.|i\.e\.|Mr\.|Mrs\.|Ms\.|O\.K\.|p\.m\.|St\.|u\.s\.|vs\.|Fig\.)" (m, rest)
        
        //省略 apostrophe
        | Prefix @"'([sdmt]|ll|re|ve)\b" (m, rest)

        //字母与标记
        | Prefix @"[\p{L}\p{M}]+" (m, rest)

        //数字
        | Prefix @"[\p{N}\p{S}]+" (m, rest)

        //标点
        | Prefix @"[\p{P}]" (m, rest) -> Some(m, rest)

        //其他字符@"[\S-[a-zéêèï]]*[\S-[-=`\\';/.,~!@#$%^&*\[\]()_+{}|:""<>?"
        | Prefix @"\S" (m, rest) -> Some(m, rest)

        | _ -> Some(inp,"")

    ///将给定字符串分解为单词
    let split (segment:string) =
        segment
        |> Seq.unfold trimStart
        |> Seq.mapi(fun i s -> Token(i,s))

    /////根据单词的强制起始标志断开单词数组
    //let internal explode (tokens:Token []) =
    //    let len = tokens.Length
    //    [|
    //        yield 0
    //        //找到分组行的索引
    //        for r in 1 .. len - 1 do
    //            let row = tokens.[r]
    //            if row.IsStarted then yield r
    //        yield len
    //    |]
    //    |> Array.pairwise
    //    |> Array.map(fun(s,f)-> tokens.[s..f])

    ///断句
    let sentances (tokens : Token []) =
        let final2 (tk1 : Token, tk2 : Token) =
            let s1 = tk1.Lexeme
            let s2 = tk2.Lexeme
            match s1, s2 with
            | ("." | "!" | "?" | ":"), " " -> true
            | _ -> false
        //获取开头的句子
        let rec sent3 acc (tokens:Token list) =
            match tokens with
            | tk1 :: tk2 :: tk3 :: tail ->
                let s1 = tk1.Lexeme
                let s2 = tk2.Lexeme
                let s3 = tk3.Lexeme
                match s1, s2, s3 with
                | ".", ".", " " -> //省略号，不作结尾
                    sent3 (tk3::tk2::tk1::acc) tail
                | ("." | "!" | "?"), (")" | "\""), " " ->
                    let sentance = (tk3 :: tk2 :: tk1 :: acc) |> List.rev
                    sentance, tail
                | _ -> sent2 acc tokens
            | _ -> sent2 acc tokens

        and sent2 acc (tokens:Token list) =
            match tokens with
            | tk1 :: tk2 :: tail when final2 (tk1, tk2) ->
                let sentance = (tk2 :: tk1 :: acc) |> List.rev
                sentance, tail
            | tk :: tail -> sent3 (tk :: acc) tail
            | [] ->
                let sentance = acc |> List.rev
                sentance, []

        //获取所有句子
        let rec sents acc tokens =
            match tokens with
            | [] -> acc |> List.rev
            | _ ->
                let s, tokens = sent3 [] tokens
                sents (s :: acc) tokens
        sents [] (tokens |> Array.toList)
        |> List.map (List.toArray)
        |> List.toArray

    ///格式化输入，使输入符合英语语法。
    let formatInputText(input:string) =
        //替换中文标点为英文标点
        let input =
            input
                .TrimStart()
                .Replace('‘','\'').Replace('’', '\'')
                .Replace('“','"').Replace('”', '"')
                .Replace('（', '(').Replace('）', ')')
                .Replace('-', '-').Replace('–','-').Replace("­","") // 删除可选连字符
                // ﬀ / ﬁ / ﬂ / ﬃ / ﬄ 
                .Replace("ﬀ","ff")
                .Replace("ﬁ","fi")
                .Replace("ﬂ","fl")
                .Replace("ﬃ","ffi")
                .Replace("ﬄ","ffl")

        let input =
            //行末的连字符
            //Regex.Replace(input,@"([a-zA-Z])-\s*\n([a-zA-Z])","$1-$2")
            Regex.Replace(input, @"([a-zA-Z])-\s*\n([a-zA-Z])", "$1$2")

        let input =
            //所有空白符转化为单个空格
            Regex.Replace(input,@"\s+", " ")
        input
