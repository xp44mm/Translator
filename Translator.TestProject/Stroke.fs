namespace Translator.TestProject

open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open Microsoft.Data.Sqlite

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open Translator8.Scaffold

open System
open System.IO
open System.Text

open FSharp.Idioms
open FSharp.Idioms.Literal


type StrokeTest(output: ITestOutputHelper) =

    [<Fact
    //(Skip = "only once needed")
    >]
    member _.``提取strokeJsonPath``() =
        let strokeJsonPath = @"D:\Application Data\崔安琪\中文词典\stroke.json"

        let json =
            File.ReadAllText(strokeJsonPath, Encoding.UTF8)
            |> FSharp.RfcJson.JsonCompiler.compile

        let rows = [
            for a in json.elements do
            let capital = a.elements.Head.stringText
            //output.WriteLine $"{capital}"
            for b in a.elements.Tail do
            let letters = b.elements.Head.stringText
            //output.WriteLine $"{letters}"
            for c in b.elements.Tail do
            let d = c.stringText.Split '|'
            let voice = d.[0]
            for hanzi in d.[1..] do
            yield {|
                capital = capital
                letters = letters
                voice = voice
                hanzi = hanzi
            |}
        ]
        let content =
            [
                "capital\tletters\tvoice\thanzi"
                for row in rows do
                $"{row.capital}\t{row.letters}\t{row.voice}\t{row.hanzi}"
            ]
        let path = strokeJsonPath.Split('.').[0] + ".tsv"
        File.WriteAllLines(path, content, Encoding.UTF8)
        output.WriteLine path

    [<Fact
    //(Skip = "only once needed")
    >]
    member _.``通用规范汉字表一级字表``() =
        let srcPath = @"D:\Application Data\崔安琪\中文词典\通用规范汉字表一级字表.txt"
        let h3500 =
            File.ReadAllText(srcPath, Encoding.UTF8).Split('\n').[1].ToCharArray()
            |> Set.ofArray

        output.WriteLine (stringify h3500.Count)
        //output.WriteLine (stringify h3500)
