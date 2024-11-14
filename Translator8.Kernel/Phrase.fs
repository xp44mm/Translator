namespace Translator8.Kernel
open System

///若干个单词(token)组成一个短语
type Phrase(tokens:Token[]) =
    member _.Tokens = tokens
    member _.Position = tokens.[0].Position
    member _.Text = 
        tokens
        |> Array.map(fun tk -> tk.Lexeme) 
        |> String.concat ""

    new() = new Phrase([||])

    ///以下是自定义相等，自定义比较:位置相等，文本相等，即使相等
    override this.Equals(thatobj) =
        match thatobj with
        | :? Phrase as that ->
            this.Position = that.Position &&
            this.Text.Equals(that.Text,StringComparison.OrdinalIgnoreCase)
        | _ -> false

    override x.GetHashCode() = hash (x.Position,x.Text.ToLower())

    interface System.IComparable with
      member x.CompareTo yobj =
          match yobj with
          | :? Phrase as y -> compare (x.Position,x.Text.ToLower()) (y.Position,y.Text.ToLower())
          | _ -> invalidArg "yobj" "cannot compare values of different types"


