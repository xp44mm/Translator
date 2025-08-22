module Translator8.Kernel.Chinese

open System
open System.Text.RegularExpressions

let toItems (chinese:string) =
    Regex.Split(chinese.Trim(), "<br>",RegexOptions.IgnoreCase)
    |> Array.map(fun i -> i.Trim())
    |> Array.filter (String.IsNullOrEmpty >> not)
    |> Array.distinct

let from (items:string seq) =
    items
    |> Seq.distinct
    |> String.concat "<br>"
