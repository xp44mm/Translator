namespace Translator8.Kernel

open Microsoft.EntityFrameworkCore
open Translator8.Scaffold
open System

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Threading.Tasks
open System.Threading.Tasks
open System.Windows
open System.Reactive.Concurrency

open Microsoft.EntityFrameworkCore
open Translator8.Scaffold


///操作数据库的类型
type WordRepository() =            
    let _words = WordDictionary()

    member _.Words = _words

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

    member _.getObservableWords () =
        use db = new VocabularyDbContext()
        db.Word.AsNoTracking().ToArrayAsync().ToObservable().SelectMany(fun arr -> arr |> Seq.ofArray)

    member repo.updateDatabase(eng,chn) =
        let word = new Word(English = eng, Chinese = chn)

        if String.IsNullOrWhiteSpace(chn) then
            repo.delete(word.English)
        else
            repo.update(word)

        true

    member repo.updateDictionary(eng,chn) =
        if String.IsNullOrWhiteSpace(chn) then
            repo.Words.delete(eng)
        else
            repo.Words.update(eng,chn)
