module Translator9.PhrasesWrapPanel

open System

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Subjects
open System.Reactive.Disposables

open System.Windows
open System.Windows.Controls
open Translator9.ViewModel
open System.Windows.Input
open Translator8.Kernel
open System.Collections.Generic


// 闭包的附属数据由输入参数暴漏，如果是不变量，如一个不变的函数，请在外面单独定义一个函数。
// 不必使用链式调用，链式调用在js中常见，那是因为它没有类型，有类型，还是 a.b()单独一句一个功能更可读。
let addCurrentPhraseEvent (phrasesWrapPanel:WrapPanel) (currentPhraseIndex: Subject<int>) =
    let disposables = new CompositeDisposable()
    (phrasesWrapPanel.MouseDown :> IObservable<_>)
        .Select(fun (args: MouseButtonEventArgs) -> args.Source :?> FrameworkElement)
        .Select(fun clickedElement ->
            let borders =
                phrasesWrapPanel.Children
                |> Seq.cast<Border>
                |> Seq.toArray

            let textBlocks1 =
                borders
                |> Array.map (fun border -> PhraseBorder.getTextBlock1 border)

            let textBlocks2 =
                borders
                |> Array.map (fun border -> PhraseBorder.getTextBlock2 border)

            match clickedElement with
            | :? WrapPanel as b when b = phrasesWrapPanel -> Some -1
            | :? ComboBox -> None
            | :? Border as b -> borders |> Array.findIndex ((=) b) |> Some
            | :? TextBlock as tb ->
                match tb.Tag.ToString() with
                | "1" -> textBlocks1 |> Array.findIndex ((=) tb) |> Some
                | "2" -> textBlocks2 |> Array.findIndex ((=) tb) |> Some
                | _ -> failwith "never"
            | _ -> failwith $"{clickedElement.GetType().Name}"
        )

        .Subscribe(fun maybe ->
            match maybe with
            | Some i -> currentPhraseIndex.OnNext i
            | _ -> ()
        )
    |> disposables.Add

    // 输入参数的清理由用户负责
    phrasesWrapPanel.Unloaded.Add(fun _ -> disposables.Dispose())

let switchPhrase (phrasesWrapPanel: WrapPanel) (previous: int) (current: int) =
    let borders =
        phrasesWrapPanel.Children
        |> Seq.cast<Border>
        |> Seq.toArray

    if previous > -1 then
        borders.[previous] |> PhraseBorder.showTextBlock

    if current > -1 then
        borders.[current] |> PhraseBorder.showComboBox

/// 数据库版本变化或者首次粘贴句子时候第一次生成句子的Phrase数组
let updateCurrentPhrasesView (wrapPanel: WrapPanel) (words: Dictionary<string, string>) (dbVersion: int) =
    let viewModel = wrapPanel.DataContext :?> SentencePhraseViewModel

    if viewModel.dbVersion < dbVersion then
        let olds =
            wrapPanel.Children
            |> Seq.cast<FrameworkElement>
            |> Seq.map (PhraseBorder.readSelection)
            |> Map.ofSeq

        let phrases = Sentence.toPhrasesViewModel words olds viewModel.sentence

        let phraseBorders =
            phrases
            |> List.map (fun (phrase, items, selected) ->
                let view = PhraseBorder.create phrase items selected
                view.DataContext <- phrase
                view
            )

        wrapPanel.Children.Clear()

        for border in phraseBorders do
            wrapPanel.Children.Add(border) |> ignore

        wrapPanel.DataContext <- { viewModel with dbVersion = dbVersion }

