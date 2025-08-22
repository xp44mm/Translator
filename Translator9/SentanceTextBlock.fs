module Translator9.SentanceTextBlock

open System.Windows.Controls

let create(text:string) =
    let textBlock = XamlLoader.loadXaml "SentanceTextBlock.xaml" :?> TextBlock
    textBlock.Text <- text
    textBlock
