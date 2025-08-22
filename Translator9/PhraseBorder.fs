module Translator9.PhraseBorder

open System

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Subjects
open System.Reactive.Disposables

open System.Windows
open System.Windows.Controls
open Translator8.Kernel
open System.Collections.Generic

let getStackPanel (root: Border) = root.Child :?> StackPanel

let getTextBlock1 (root: Border) =
    let _stackPanel = root |> getStackPanel
    let _textBlock1 = _stackPanel.Children.[0] :?> TextBlock
    XamlLoader.verifyTag _textBlock1 "1"
    _textBlock1

let getComboBox (root: Border) =
    let _stackPanel = root |> getStackPanel
    _stackPanel.Children.[1] :?> ComboBox

let getTextBlock2 (root: Border) =
    let _stackPanel = root |> getStackPanel
    let _textBlock1 = _stackPanel.Children.[2] :?> TextBlock
    XamlLoader.verifyTag _textBlock1 "2"
    _stackPanel.Children.[2] :?> TextBlock


let showComboBox (border: Border) =
    let _comboBox = getComboBox border
    let _textBlock2 = getTextBlock2 border

    _comboBox.Visibility <- Visibility.Visible
    _textBlock2.Visibility <- Visibility.Collapsed

let showTextBlock (border: Border) =
    let _comboBox = getComboBox border
    let _textBlock2 = getTextBlock2 border

    _comboBox.Visibility <- Visibility.Collapsed
    _textBlock2.Visibility <- Visibility.Visible

/// 从View中读取数据
let readSelection (root: FrameworkElement) =
    let viewModel = root.DataContext :?> Phrase
    let chinese = (root :?> Border |> getComboBox).SelectedItem :?> string
    viewModel, chinese

let create (phrase: Phrase) (items: string[]) (old: string option) =

    let border = XamlLoader.loadXaml "PhraseBorder.xaml" :?> Border
    let _comboBox = getComboBox border
    let _textBlock1 = getTextBlock1 border
    let _textBlock2 = getTextBlock2 border
    let disposables = new CompositeDisposable()
    let _selectedItem = new BehaviorSubject<_>("")

    _selectedItem
        .Skip(1)
        .DistinctUntilChanged()
        .Subscribe(fun text ->
            _comboBox.SelectedItem <- text
            _textBlock2.Text <- text
        )
    |> disposables.Add

    (_comboBox.SelectionChanged :> IObservable<_>)
        .Subscribe(fun e ->
            let s = _comboBox.SelectedItem :?> string
            _selectedItem.OnNext(s)
        )
    |> disposables.Add

    border.Unloaded.Add(fun _ ->
        disposables.Dispose()
        _selectedItem.OnCompleted()
    )

    _textBlock1.Text <- phrase.Text

    _comboBox.Items.Clear()

    for cc in items do
        _comboBox.Items.Add cc |> ignore

    if _comboBox.Items.Count > 0 then
        _comboBox.SelectedItem <-
            match old with
            | Some old -> box old
            | _ -> _comboBox.Items.[0]

    border.DataContext <- phrase
    border

let getData(border:Border) =
    let _comboBox = getComboBox border
    let _textBlock1 = getTextBlock1 border
    let e = _textBlock1.Text
    let c = 
        _comboBox.Items
        |> Seq.cast<string>
        |> String.concat "\r\n"
    e,c
