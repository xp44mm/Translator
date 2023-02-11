module Translator.Singleton

open System
open System.Collections.Generic
open System.Text
open Translator.Kernel

// 数据库源的内存表示
let Words = new WordDictionary()
// 操作数据库的方法
let repo = new WordRepo()
