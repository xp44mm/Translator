module Translator.App

open System
open FsXaml

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Threading.Tasks
open System.Threading.Tasks
open System.Windows
open System.Reactive.Concurrency
open Translator.Kernel
open Microsoft.EntityFrameworkCore
open Translator.ef

type App = XAML<"App.xaml">


[<STAThread; EntryPoint>]
let main argv =
    System.Threading.SynchronizationContext.SetSynchronizationContext(
        System.Windows.Threading.DispatcherSynchronizationContext())

    let app = App()
    let startup = TranslatorWindow ()

    use _ = Observable.FromAsync(fun () ->
            task {
                use db = new TranslateContext()
                let! words = db.Words.AsNoTracking().ToArrayAsync()
                for word in words do
                    Singleton.Words.Add(word.English, word.Chinese)
            })
                .SubscribeOn(System.Reactive.Concurrency.TaskPoolScheduler.Default)
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(fun () -> 
                    startup.btnPaste.IsEnabled <- true
                )

    app.Run(startup) // Returns application's exit code.
