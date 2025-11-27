module Translator9.WordWindow

open MahApps.Metro.Controls
open Translator8.Kernel

open System
open System.Reactive.Subjects
open System.Windows
open System.Windows.Controls
open FSharp.ReactiveWpf
open System.Reflection

let create (repository: WordRepository) =
    let assy = Assembly.GetExecutingAssembly()
    let w = XamlLoader.loadXaml assy "Translator9.WordWindow.xaml" :?> MetroWindow
    let tbEnglish = w.FindName("tbEnglish") :?> TextBox
    let tbChinese = w.FindName("tbChinese") :?> TextBox
    let btnNeat = w.FindName("btnNeat") :?> Button
    let btnUpdateDatabase = w.FindName("btnUpdateDatabase") :?> Button

    btnNeat.Click.Add(fun _ ->
        tbEnglish.Text <- WordOperators.neatEnglish (tbEnglish.Text)
        tbChinese.Text <- WordOperators.neatChinese (tbChinese.Text)
    )

    btnUpdateDatabase.Click.Add(fun _ ->
        let eng = WordOperators.neatEnglish (tbEnglish.Text)
        let chn = WordOperators.neatChinese (tbChinese.Text)

        if String.IsNullOrEmpty(chn) then
            repository.Words.delete (eng)
        else
            repository.Words.update (eng, chn)

        //修改永久数据库
        repository.updateDatabase(eng, chn)

        w.DialogResult <- true
        w.Close()
    )
    w

let init (w:Window) e c =
    let tbEnglish = w.FindName("tbEnglish") :?> TextBox
    let tbChinese = w.FindName("tbChinese") :?> TextBox
    tbEnglish.Text <- e
    tbChinese.Text <- c
