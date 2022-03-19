namespace Translate.test

//open Xunit
//open Xunit.Abstractions
//open Microsoft.EntityFrameworkCore

//open Translator.ef


//open Autofac

//[<Collection("AutofacCollection")>]
//type TestFileHelpersTest(output: ITestOutputHelper,fixture: AutofacFixture) =
//    let di:IContainer = fixture.Container

//    [<Fact>]
//    member this.``SOURCE DIRECTORY`` () =
//        output.WriteLine(__SOURCE_DIRECTORY__)

//    [<Fact>]
//    member this.``get connection string`` () =
//        output.WriteLine(TestFileHelpers.getConnectionString())

//    [<Fact>]
//    member this.``getWords`` () =
//        use scope = di.BeginLifetimeScope()
//        use db = scope.Resolve<TranslateContext>()

//        let words = db.Words.AsNoTracking() |> Seq.toArray
//        for w in words do
//            output.WriteLine(sprintf "%A" w.English)
