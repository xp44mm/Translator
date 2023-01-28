namespace Translator

open System
open System.Windows.Shapes
open System.Windows.Media

//open System.Reactive
//open System.Reactive.Linq
//open System.Reactive.Observable.Aliases

open MahApps.Metro.Controls

type SentanceUserControlXaml = FsXaml.XAML<"SentanceUserControl.xaml">

type SentanceUserControl () =
    inherit SentanceUserControlXaml ()

