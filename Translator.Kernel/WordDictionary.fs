namespace Translator.Kernel

open System
open System.Collections.Generic

type WordDictionary()   =
    inherit Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)

    /// 新增或修改词条
    member dic.update(english,chinese) =
        if dic.ContainsKey english then
            if chinese = dic.[english] then
                ()
            else
                dic.[english] <- chinese
        else
            dic.Add(english,chinese)

    /// 删除词条，不存在就什么也不做。
    member dic.delete (english:string) =
        if dic.ContainsKey english then
            dic.Remove english |> ignore
        else
            ()
