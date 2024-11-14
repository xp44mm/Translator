module  Translator8.Kernel.AppOps

open System

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Threading.Tasks
open System.Threading.Tasks
open System.Windows
open System.Reactive.Concurrency

open Microsoft.EntityFrameworkCore
open Translator8.Scaffold


let getWordsStream () =
    use db = new VocabularyDbContext()

    db.Word.AsNoTracking().ToArrayAsync().ToObservable().SelectMany(fun arr -> arr |> Seq.ofArray)
