namespace Cuisl
open System
open System.Reflection
open System.Runtime.InteropServices
open Microsoft.Win32
[<AbstractClass>]
type ComClass() =
    inherit Object()
    static member private GetSubKeyName(tp:Type, subKeyName:string) =
        String.Format("CLSID\\{{{0}}}\\{1}", tp.GUID.ToString().ToUpper(), subKeyName)
   //解决在某些机器的Excel提示找不到mscoree.dll的问题
   //这里在注册表中将该dll的路径注册进去，
   //当使用regasm注册该类库为com组件时会调用该方法
    [<ComRegisterFunction>]
    static member RegisterFunction (tp: Type ) =
        Registry.ClassesRoot.CreateSubKey(ComClass.GetSubKeyName(tp, "Programmable")) |> ignore
        let key = Registry.ClassesRoot.OpenSubKey(ComClass.GetSubKeyName(tp, "InprocServer32"), true)
        key.SetValue("", System.Environment.SystemDirectory + @"\mscoree.dll", RegistryValueKind.String)
    [<ComUnregisterFunction>]
    static member UnregisterFunction(tp:Type )=
        Registry.ClassesRoot.DeleteSubKey(ComClass.GetSubKeyName(tp, "Programmable"), false)
