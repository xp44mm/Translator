namespace Translator8.Kernel

open Microsoft.EntityFrameworkCore
open Translator8.Scaffold

open System
open System.Windows
open System.Threading.Tasks

open System.Reactive
open System.Reactive.Concurrency
open System.Reactive.Linq
open System.Reactive.Threading.Tasks

///操作数据库的类型
type WordRepository(connectionString:string) =
    let _words = WordDictionary()

    member _.Words = _words

    member _.update(word: Word) =
        use db = new VocabularyDbContext(connectionString)

        match db.Find<Word>(word.English) with
        | null -> db.Add(word) |> ignore
        | found -> found.Chinese <- word.Chinese

        db.SaveChanges() |> ignore

    member _.delete(english: string) =
        use db = new VocabularyDbContext(connectionString)
        db.Remove(new Word(English = english)) |> ignore

        db.SaveChanges() |> ignore

    member _.getObservableWords() =
        use db = new VocabularyDbContext(connectionString)

        db.Word
            .AsNoTracking() //
            .ToArrayAsync() //
            .ToObservable() //
            .SelectMany(fun arr -> arr |> Seq.ofArray)

    member repo.updateDatabase(eng, chn) =
        let word = new Word(English = eng, Chinese = chn)

        if String.IsNullOrWhiteSpace(chn) then
            repo.delete (word.English)
        else
            repo.update (word)


    member repo.updateDictionary(eng, chn) =
        if String.IsNullOrWhiteSpace(chn) then
            repo.Words.delete (eng)
        else
            repo.Words.update (eng, chn)
