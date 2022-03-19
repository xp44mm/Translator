namespace Translator.Kernel

open System
open System.Threading.Tasks

open Microsoft.EntityFrameworkCore

open Translator.ef

///操作数据库的类型
type WordRepo() = // as this
            
    //let mutable words: Word[] = [||]
    //do Task.Run(fun()->lock words (fun()->words<-this.getWords())) |> ignore
    //member this.Words
    //    with get() = words
    //    and set value = words <- value

    static member getWordsAsync() =
        use db = new TranslateContext()
        db.Words.AsNoTracking()
            .ToArrayAsync()

    ///仅测试使用
    member this.getWords() =
        use db = new TranslateContext()
        db.Words.AsNoTracking()
        |>Seq.toArray

    member this.update(word: Word) =
        use db = new TranslateContext()
        match db.Find<Word>(word.English) with
        | null -> db.Add(word) |> ignore
        | found -> found.Chinese <- word.Chinese

        db.SaveChanges() |> ignore

    member this.delete (english:string) =
        use db = new TranslateContext()
        db.Remove(new Word(English=english)) |> ignore

        db.SaveChanges() |> ignore
