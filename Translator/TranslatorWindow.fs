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
        let sentance = unbox<SentanceViewModel>this.lstSentances.SelectedItem
        if not sentance.Updated || force then
            let olds = Translation.keepSelection(sentance.PhraseItems)
            sentance.PhraseItems <- Translation.Update(Singleton.Words, sentance.Tokens, olds)

    do 
        this.DataContext <- vm

        this.btnPaste.Click.Add(fun _ ->
            let text =
                if Clipboard.ContainsText() then
                    try
                        Clipboard.GetText()
                    with e -> e.Message
                else
                    "剪贴板没有包含文本！"
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
            try
                translateSelectedSetance false
            with ex ->
                MessageBox.Show($"selection changed:{ex.Message}")
                |> ignore
       
        )
