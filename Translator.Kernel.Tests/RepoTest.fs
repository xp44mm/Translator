namespace Translate.test

open Xunit
open Xunit.Abstractions

open Translator.Kernel
open Translator.ef

//open Autofac

//[<Collection("AutofacCollection")>]
type RepoTest(output: ITestOutputHelper) = //,fixture: AutofacFixture
    //let di = fixture.Container
    let x = ()
    //[<Fact>]
    //member this.getWords() =
    //    Assert.NotNull(di)
    //    use scope = di.BeginLifetimeScope()
    //    Assert.NotNull(scope)

    //    let repo = scope.Resolve<WordRepo>()
    //    Assert.NotNull(repo)

        //let words = repo.getWords()

        //Assert.True(words.Length>0)
        //for w in words do
        //    output.WriteLine(sprintf "%O" w)
        //output.WriteLine("")

    //[<Fact>]
    //member this.insert() =
    //    let repo = new WordRepo()

    //    let word = new Word(English = "csl.translate.test.data",Chinese = "´ŞÊ¤Àû·­Òë²âÊÔÊı¾İ")
    //    repo.update(word)
    //    let w1 = 
    //        repo.getWords() 
    //        |> Array.find(fun w -> w.English = "csl.translate.test.data")
    //    Assert.Equal(w1.Chinese,"´ŞÊ¤Àû·­Òë²âÊÔÊı¾İ")

    //[<Fact>]
    //member this.update() =
    //    let repo = new WordRepo()

    //    let word = new Word(English = "csl.translate.test.data",Chinese = "´ŞÊ¤Àû.·­Òë.²âÊÔ.Êı¾İ")
    //    repo.update(word)
    //    let w1 = 
    //        repo.getWords() 
    //        |> Array.find(fun w -> w.English = "csl.translate.test.data")
    //    Assert.Equal(w1.Chinese,"´ŞÊ¤Àû.·­Òë.²âÊÔ.Êı¾İ")

    //[<Fact>]
    //member this.delete() =
    //    let repo = new WordRepo()
    //    repo.delete("csl.translate.test.data")
    //    let w1 = repo.getWords() |> Array.tryFind(fun w -> w.English = "csl.translate.test.data")
    //    Assert.Equal(w1,Option.None)
    