module Translator9.TranslatorWindow

open Translator8.Kernel

open System
open System.Collections.Generic
open System.Diagnostics

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Subjects
open System.Reactive.Concurrency
open System.Reactive.Disposables

open System.Threading

open System.Windows
open System.Windows.Controls
open System.Windows.Input
open System.Windows.Media
open System.Windows.Threading

open FSharp.Idioms

open MahApps.Metro.Controls
open Translator9.ViewModel
open System.Speech.Synthesis
open FSharp.ReactiveWpf
open System.Reflection

let assy = Assembly.GetExecutingAssembly()

let updateSentences
    (sentencesListBox: ListBox)
    (phrasesStackPanel: StackPanel)
    (phraseIndex: Subject<int>)
    (sentences: Token[][])
    (dbVersion: int)
    =
    // 更新 sentencesListBox
    sentencesListBox.Items.Clear()

    for tokens in sentences do
        let view = tokens |> Tokens.text |> SentanceTextBlock.create
        sentencesListBox.Items.Add(view) |> ignore

    // 更新中英文对照详图
    phrasesStackPanel.Children.Clear()

    sentences
    |> Seq.iter (fun tokens ->
        let wpanel =
            WrapPanel(
                VerticalAlignment = VerticalAlignment.Stretch,
                Visibility = Visibility.Collapsed,
                Background = Brushes.Transparent,
                DataContext = {
                    sentence = tokens
                    dbVersion = dbVersion - 1
                }
            )

        PhrasesWrapPanel.addCurrentPhraseEvent wpanel phraseIndex
        phrasesStackPanel.Children.Add(wpanel) |> ignore
    )

let create (repository: WordRepository, wordsAdded: Subject<unit>) =
    let window = XamlLoader.loadXaml assy "Translator9.TranslatorWindow.xaml" :?> MetroWindow

    let btnPaste = window.FindName("btnPaste") :?> Button
    let btnUpdateDict = window.FindName("btnUpdateDict") :?> Button
    let sentencesListBox = window.FindName("lstSentences") :?> ListBox
    let phrasesStackPanel = window.FindName("phrases") :?> StackPanel

    let disposables = new CompositeDisposable()

    let phraseIndex = new Subject<int>()
    let dbVersion = new Subject<int>()

    // 异步读取数据库，等待读取后再工作
    wordsAdded
        .ObserveOn(SynchronizationContext.Current)
        .Subscribe(
            (fun () -> ()), // on Next
            (fun () ->
                btnPaste.IsEnabled <- true
                dbVersion.OnNext 0
            ) // on Complete
        )
    |> disposables.Add

    (btnPaste.Click :> IObservable<_>)
        .Select(fun _ ->
            if Clipboard.ContainsText(TextDataFormat.UnicodeText) then
                try
                    Clipboard.GetText(TextDataFormat.UnicodeText)
                with ex ->
                    ex.Message
            else
                "Clipboard does not contain text!"
        )
        .DistinctUntilChanged()
        .Select(fun text -> Sentence.from text)
        .Do(fun _ ->
            //更新视图前，确保索引安全
            phraseIndex.OnNext -1
            sentencesListBox.SelectedIndex <- -1
        )
        .WithLatestFrom(dbVersion)
        .Subscribe(fun struct (sentences, dbVersion) ->
            updateSentences sentencesListBox phrasesStackPanel phraseIndex sentences dbVersion

            // 选择零
            if not sentencesListBox.Items.IsEmpty then
                sentencesListBox.SelectedIndex <- 0
        )
    |> disposables.Add

    //列表框选择事件
    let currentSentenceIndex =
        (sentencesListBox.SelectionChanged :> IObservable<_>)
            .Select(fun (e: SelectionChangedEventArgs) ->
                let listBox = e.OriginalSource :?> ListBox
                listBox.SelectedIndex
            )
            .DistinctUntilChanged()

    // 切换句子短语根元素的可见性
    currentSentenceIndex
        .StartWith(-1)
        .Do(fun _ -> phraseIndex.OnNext -1)
        .Buffer(2, 1)
        .Select(fun ls -> ls.[0], ls.[1])
        .Subscribe(fun (i, j) ->
            if i > -1 then
                phrasesStackPanel.Children.[i].Visibility <- Visibility.Collapsed

            if j > -1 then
                phrasesStackPanel.Children.[j].Visibility <- Visibility.Visible
        )
    |> disposables.Add

    let currentWrapPanel =
        currentSentenceIndex.Select(fun i ->
            if 0 <= i && i < phrasesStackPanel.Children.Count then
                phrasesStackPanel.Children.[i] :?> WrapPanel
                |> Some
            else
                None
        )

    currentWrapPanel
        .Where(fun wp -> wp.IsSome)
        .WithLatestFrom(dbVersion)
        .Subscribe(fun struct (wrapPanel, dbVersion) ->
            PhrasesWrapPanel.updateCurrentPhrasesView wrapPanel.Value repository.Words dbVersion
        )
    |> disposables.Add

    let currentBorder =
        currentWrapPanel.SelectMany(fun wrapPanel ->
            match wrapPanel with
            | None -> Observable.Return None
            | Some w -> phraseIndex.Select(fun i -> w.Children |> Seq.cast<Border> |> Seq.tryItem i)
        )

    phraseIndex
        .Buffer(2, 1)
        .WithLatestFrom(currentWrapPanel)
        .Subscribe(fun struct (ls, wrapPanel) ->
            match wrapPanel with
            | None -> ()
            | Some wrapPanel -> PhrasesWrapPanel.switchPhrase wrapPanel ls.[0] ls.[1]
        )
    |> disposables.Add

    (btnUpdateDict.Click :> IObservable<_>)
        .WithLatestFrom(
            //补充多个可观察值
            Observable.CombineLatest(dbVersion, currentSentenceIndex, currentBorder, fun v i b -> v, i, b)
        )
        .Subscribe(fun struct (_, (v, i, b)) ->
            let dlg = WordWindow.create (repository)
            dlg.Owner <- window

            match b with
            | Some b ->
                let e, c = PhraseBorder.getData b
                WordWindow.init dlg e c
            | _ -> ()

            match dlg.ShowDialog() |> Option.ofNullable with
            | Some true ->
                // 版本号+1
                dbVersion.OnNext(v + 1)

                // 重新激活当前语句
                sentencesListBox.SelectedIndex <- -1
                sentencesListBox.SelectedIndex <- i
            | _ -> ()

            ()
        )
    |> disposables.Add

    window.Closing.Add(fun _ -> disposables.Dispose())
    window

let speech (window: Window) =
    let listBox = window.FindName("lstSentences") :?> ListBox
    let btnSpeech = window.FindName("btnSpeech") :?> Button

    let synth = new SpeechSynthesizer()
    synth.SelectVoice("Microsoft David")
    synth.Rate <- -2

    let sub =
        (btnSpeech.Click :> IObservable<_>)
            .Select(fun _ ->
                let textBlock = listBox.SelectedItem :?> TextBlock
                textBlock.Text
            )
            .Subscribe(fun word -> synth.Speak(word))

    window.Closing.Add(fun _ ->
        sub.Dispose()
        synth.Dispose()
    )
