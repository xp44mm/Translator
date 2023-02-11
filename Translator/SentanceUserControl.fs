namespace Translator
open Translator.Kernel

open System
open System.Windows.Shapes
open System.Windows.Media
open System.Windows.Input
open System.Windows.Controls

//open System.Reactive
//open System.Reactive.Linq
//open System.Reactive.Observable.Aliases

open MahApps.Metro.Controls

type SentanceUserControlXaml = FsXaml.XAML<"SentanceUserControl.xaml">

type SentanceUserControl () =
    inherit SentanceUserControlXaml ()

    member this.SelectedPhrase() =
        match this.lstPhrases.SelectedItem with
        | :? PhraseItem as pc ->
            let en = pc.Phrase.Text
            let zh = String.Join("\r\n", pc.ChineseCandidates)
            [ en; zh ]
        | _ -> []

    override this.Item_MouseRightButtonUp(s:obj,e:MouseButtonEventArgs) =
        let t =
            match e.Source with
            | :? TextBlock as c -> c.Text
            | :? ComboBox as c -> c.Text
            | c -> failwith $"{c.GetType()}"
        let sentance = unbox<SentanceViewModel>this.DataContext        
        sentance.Transla <- sentance.Transla + t
