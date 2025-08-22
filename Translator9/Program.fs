module Translator9.Program

open System.Configuration
open System.Data
open System.Windows
open System.Threading
open System.Windows.Threading
open System.Reactive.Linq
open System.Reactive.Concurrency
open System
open System.Windows

open FSharp.Idioms

open Translator8.Kernel
open Translator8.Scaffold
open System.Reactive.Disposables
open System.Reactive.Subjects

SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext())

let app = XamlLoader.loadXaml "App.xaml" :?> Application
let repository = new WordRepository(Config.getConnectionString())
let wordsAdded = new Subject<unit>()

let mainWindow = TranslatorWindow.create (repository, wordsAdded)

TranslatorWindow.speech mainWindow

let disposables = new CompositeDisposable()

mainWindow.Show()

repository
    .getObservableWords()
    .SubscribeOn(TaskPoolScheduler.Default)
    .Subscribe(
        (fun word -> repository.Words.Add(word.English, word.Chinese)), // on next
        (fun () -> wordsAdded.OnCompleted()) // on complete
    )
|> disposables.Add

[<STAThread>]
[<EntryPoint>]
let main _ = app.Run(mainWindow)
