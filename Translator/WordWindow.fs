namespace Translator
open Translator.Kernel

open System
open System.Windows.Shapes
open System.Windows.Media

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Observable.Aliases

open MahApps.Metro.Controls
open System.Windows


type WordWindowXaml = FsXaml.XAML<"WordWindow.xaml">

type WordWindow () as this =
    inherit WordWindowXaml ()

    do 
        this.btnNeat.Click.Add(fun _ ->
            this.tbEnglish.Text <- WordOperators.neatEnglish(this.tbEnglish.Text)
            this.tbChinese.Text <- WordOperators.neatChinese(this.tbChinese.Text)

            //MessageBox.Show(this,"hello world","Neat")
            //|> ignore
        )
        this.btnUpdateDatabase.Click.Add(fun _ ->
            let eng = WordOperators.neatEnglish(this.tbEnglish.Text)
            let chn = WordOperators.neatChinese(this.tbChinese.Text)

            if String.IsNullOrEmpty chn then
                Singleton.Words.delete(eng)
            else
                Singleton.Words.update(eng, chn)

            //修改永久数据库
            this.DialogResult <- WordOperators.updateDatabase(Singleton.repo, eng, chn);
            this.Close()
        )
    member this.InitialText (ls:string list) =
        match ls with
        | [en;cn] ->
            this.tbEnglish.Text <- en
            this.tbChinese.Text <- cn
        | _ -> ()
