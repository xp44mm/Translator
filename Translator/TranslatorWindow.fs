namespace Translator

open System
open System.Windows.Shapes
open System.Windows.Media

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Observable.Aliases

open MahApps.Metro.Controls

type TranslatorWindowXaml = FsXaml.XAML<"TranslatorWindow.xaml">

type TranslatorWindow () as this =
    inherit TranslatorWindowXaml ()
    do this.btnUpdateDict.Click.Add(fun _ ->
    //((App)Application.Current).repo
        let dlg = new WordWindow()
        dlg.Owner <- this

        //dlg.InitialText(this.sentanceControl.SelectedPhrase())

        if dlg.ShowDialog().Value then
            ()
        //    let vm = unbox<TranslatorViewModel>this.DataContext
        //    for sent in vm.Sentances do
        //        sent.Updated <- false
        //    this.translateSelectedSetance()
    
    
    )
