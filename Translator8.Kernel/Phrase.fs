namespace Translator8.Kernel

open System
open System.Collections.Generic
open System.Text.RegularExpressions

///若干个单词(token)组成一个短语
type Phrase(tokens: Token[]) =
    member _.Tokens = tokens
    member _.Position = tokens.[0].Position

    member _.Text = Tokens.text tokens

    ///自定义相等，位置相等，文本相等，即相等
    override this.Equals(thatobj) =
        match thatobj with
        | :? Phrase as that ->
            this.Position = that.Position
            && this.Text.Equals(that.Text, StringComparison.OrdinalIgnoreCase)
        | _ -> false

    override x.GetHashCode() =
        hash (x.Position, x.Text.ToLowerInvariant())

    interface IComparable with
        member x.CompareTo yobj =
            match yobj with
            | :? Phrase as y ->
                let x = (x.Position, x.Text.ToLowerInvariant())
                let y = (y.Position, y.Text.ToLowerInvariant())
                compare x y
            | _ -> invalidArg "yobj" "cannot compare values of different types"

    interface System.IComparable<Phrase> with
        member x.CompareTo y =
            let x = (x.Position, x.Text.ToLowerInvariant())
            let y = (y.Position, y.Text.ToLowerInvariant())
            compare x y
