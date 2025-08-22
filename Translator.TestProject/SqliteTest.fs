namespace Translator.TestProject

open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open Microsoft.Data.Sqlite

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal
open FSharp.xUnit
open Translator8.Scaffold

type SqliteTest(output: ITestOutputHelper) =
    let formatChinese (text: string) =
        let formated =
            Regex.Split(text.Trim(), @"\r\n|\r|\n")
            |> Seq.map (fun x -> x.Trim())
            |> Seq.filter (String.IsNullOrEmpty >> not)
            |> String.concat "<br>"

        if formated = text then None else Some formated

    [<Fact(Skip = "only once needed")>]
    member _.``替换回车``() =
        let dbPath = @"D:\Application Data\vocabulary.db"
        use dbContext = new VocabularyDbContext($"Data Source={dbPath}")

        dbContext.Word
        |> Seq.iter (fun word ->
            match formatChinese word.Chinese with
            | Some nw -> word.Chinese <- nw
            | None -> ()
        )

        let changes = dbContext.SaveChanges()
        output.WriteLine($"成功更新了{changes}条记录")

    [<Fact(Skip = "only once needed")>]
    member _.``预览替换回车``() =
        let dbPath = "D:\\Application Data\\vocabulary.db"
        use dbContext = new VocabularyDbContext($"Data Source={dbPath}")

        let content =
            dbContext.Word
            |> Seq.map (fun word ->
                let c =
                    match formatChinese word.Chinese with
                    | Some f -> f
                    | None -> word.Chinese

                word.English, c
            )
            |> Seq.map (fun (e, c) -> $"{e}\t{c}")
            |> String.concat "\r\n"

        //let voc = File.ReadAllLines(fromPath, Encoding.UTF8)
        let folder = @"D:\Application Data\GitHub\xp44mm\Translator\"
        let path = Path.Combine(folder, "voc.tsv")
        File.WriteAllText(path, content)
        output.WriteLine(path)
