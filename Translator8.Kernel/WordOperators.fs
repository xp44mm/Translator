namespace Translator8.Kernel

open System
open System.Text.RegularExpressions

open Translator8.Scaffold

///“编辑”对话框
module WordOperators =
    let neatChinese(chn) =
        [
            for ln in Regex.Split(chn, @"\r?\n") do
                let ln = ln.Trim()
                if not <| String.IsNullOrWhiteSpace(ln) then yield ln
        ]
        |> Seq.distinct
        |> String.concat Environment.NewLine

    let neatEnglish (eng:string) = Regex.Replace(eng.Trim(), @"\s+", " ")

    //[<Obsolete("WordRepository")>]
    //let updateDatabase(repo:WordRepo,eng,chn) =
    //    let word = new Word(English = eng, Chinese = chn)

    //    if String.IsNullOrWhiteSpace(chn) then
    //        repo.delete(word.English)
    //    else
    //        repo.update(word)

    //    true

    //[<Obsolete("WordRepository")>]
    //let updateDictionary(dic:WordDictionary,eng,chn) =
    //    if String.IsNullOrWhiteSpace(chn) then
    //        dic.delete(eng)
    //    else
    //        dic.update(eng,chn)
        
