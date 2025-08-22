module Translator9.XamlLoader

open System
open System.IO
open System.Xml
open System.Windows
open System.Windows.Markup

open System.Reflection
open System.Diagnostics
open System.Windows.Media
open System.Windows.Input

/// 从嵌入式资源加载 XAML 并解析为对象
let loadXaml (fileName: string) =
    use stream =
        Assembly.GetExecutingAssembly().GetManifestResourceStream($"Translator9.{fileName}")

    use sr = new StreamReader(stream)
    let str = sr.ReadToEnd() // xaml file content
    use sr = new StringReader(str)
    let xr = XmlReader.Create(sr)
    XamlReader.Load(xr)

let verifyTag (element: FrameworkElement) expectedTag =
    let condition =
        element.Tag <> null
        && element.Tag.ToString() = expectedTag

    Debug.Assert(condition, $"error: Tag!={expectedTag}")

/// 获取所有父元素
let rec getInfiniteHierarchy (element: DependencyObject) =
    seq {
        match element with
        | null -> ()
        | _ ->
            yield element
            let parent = VisualTreeHelper.GetParent(element)

            if parent <> null then
                yield! getInfiniteHierarchy parent
            else
                ()
    }

/// 递归获取父元素直到 root
let getParentHierarchy (root: DependencyObject) (element: DependencyObject) =
    let rec loop (element: DependencyObject) =
        seq {
            match element with
            | null -> ()
            | _ ->
                yield element
                let parent = VisualTreeHelper.GetParent(element)

                if parent <> null && parent <> root then
                    yield! loop parent
                elif parent = root then //包括root
                    yield parent
        }

    loop element

let info (dependObj:DependencyObject) = 
    match dependObj with
    | :? FrameworkElement as fe -> 
        fe.GetType().Name + (if String.IsNullOrEmpty(fe.Name) then "" else $" (Name: {fe.Name})")
        
    | _ -> dependObj.GetType().Name

let getClickedElement phrases (args: MouseEventArgs) =
    match VisualTreeHelper.HitTest(phrases, args.GetPosition(phrases)) with
    | null -> None
    | hitTestResult ->
        match hitTestResult with
        | null -> None
        | _ ->
            match hitTestResult.VisualHit with
            | null -> None
            | clickedElement -> Some clickedElement

