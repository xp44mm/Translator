module Translator.App

open System
open FsXaml

type App = XAML<"App.xaml">

[<STAThread; EntryPoint>]
let main argv =
    System.Threading.SynchronizationContext.SetSynchronizationContext(
        System.Windows.Threading.DispatcherSynchronizationContext())

    let app = App()
    let startup = TranslatorWindow ()
    app.Run(startup) // Returns application's exit code.
