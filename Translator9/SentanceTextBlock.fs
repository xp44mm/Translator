module Translator9.SentanceTextBlock

open System.Windows.Controls
open FSharp.ReactiveWpf
open System.Reflection

let create(text:string) =
    let assy = Assembly.GetExecutingAssembly()

    let textBlock = XamlLoader.loadXaml assy "Translator9.SentanceTextBlock.xaml" :?> TextBlock
    textBlock.Text <- text
    textBlock
