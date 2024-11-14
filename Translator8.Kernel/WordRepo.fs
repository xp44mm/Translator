namespace Translator8.Kernel

open Microsoft.EntityFrameworkCore
open Translator8.Scaffold

///操作数据库的类型
type WordRepo() =
            
    //member _.getWords() =
    //    use db = new TranslateContext()
    //    db.Words.AsNoTracking()
    //    |>Seq.toArray

    member _.update(word: Word) =
        use db = new VocabularyDbContext()
        match db.Find<Word>(word.English) with
        | null -> db.Add(word) |> ignore
        | found -> found.Chinese <- word.Chinese

        db.SaveChanges() |> ignore

    member _.delete (english:string) =
        use db = new VocabularyDbContext()
        db.Remove(new Word(English=english)) |> ignore

        db.SaveChanges() |> ignore
