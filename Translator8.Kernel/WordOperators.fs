module Translator8.Kernel.WordOperators
//“编辑”对话框

open System
open System.Text.RegularExpressions

///整理中文
let neatChinese(chn) =
    [
        for ln in Regex.Split(chn, @"\r?\n") do
            let ln = ln.Trim()
            if not <| String.IsNullOrEmpty(ln) then yield ln
    ]
    |> Chinese.from

///整理英文
let neatEnglish (eng:string) = Regex.Replace(eng.Trim(), @"\s+", " ")

        
