namespace Translator
open Translator.Kernel

open System
open System.Windows.Shapes
open System.Windows.Media
open System.Windows

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Observable.Aliases

open MahApps.Metro.Controls


type TranslatorWindowXaml = FsXaml.XAML<"TranslatorWindow.xaml">

type TranslatorWindow () as this =
    inherit TranslatorWindowXaml ()

    let vm = TranslatorViewModel()

    let translateSelectedSetance (force:bool) =
        match this.lstSentances.SelectedItem with
        | null -> ()
        | :? SentanceViewModel as sentance ->
            if not sentance.Updated || force then
                let olds = Translation.keepSelection(sentance.PhraseItems)
                sentance.PhraseItems <- Translation.Update(Singleton.Words, sentance.Tokens, olds)
        | x -> failwith $"{x.GetType()}"

    do 
        this.DataContext <- vm

        this.btnPaste.Click.Add(fun _ ->
            let text =
                if Clipboard.ContainsText(TextDataFormat.UnicodeText) then
                    try
                        Clipboard.GetText(TextDataFormat.UnicodeText)
                    with e -> e.Message
                else
                    "Clipboard does not contain text!"
            let essay = Essay(text)
            vm.Sentances <-
                essay.Sentances
                |> Array.map(fun x -> SentanceViewModel(x.Tokens, x.Text))

            if not this.lstSentances.Items.IsEmpty then
                this.lstSentances.SelectedIndex <- 0
    
        )

        this.btnUpdateDict.Click.Add(fun _ ->
            let dlg = WordWindow()
            dlg.Owner <- this
            dlg.InitialText((this.sentanceControl:?>SentanceUserControl).SelectedPhrase())

            if dlg.ShowDialog().Value then
                for sent in vm.Sentances do
                    sent.Updated <- false
                translateSelectedSetance false
            
        )

        this.lstSentances.SelectionChanged.Add(fun _ ->
            translateSelectedSetance false
        )
