namespace Translator

open System
open System.Windows.Shapes
open System.Windows.Media

open System.Reactive
open System.Reactive.Linq
open System.Reactive.Observable.Aliases

open MahApps.Metro.Controls

type WordWindowXaml = FsXaml.XAML<"WordWindow.xaml">

type WordWindow () =
    inherit WordWindowXaml ()

