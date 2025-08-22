module Translator9.Config

open System
open System.IO
open System.Text

let configPath =
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json")

let getConnectionString () =
    try
        if File.Exists(configPath) then
            let json =
                File.ReadAllText(configPath, Encoding.UTF8)
                |> UnquotedJson.Json.parse

            json.["ConnectionStrings"].["VocabularyDb"].stringText


        else
            failwith $"File.Exists(configPath)={configPath};违反"
    with ex ->
        failwith "appsettings.json.[ConnectionStrings].[VocabularyDb]"
