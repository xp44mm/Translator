namespace Translator9.ViewModel

open Translator8.Kernel
open System.Reactive.Subjects

type SentencePhraseViewModel = {
    sentence: Token []
    //index: int
    dbVersion: int
}
