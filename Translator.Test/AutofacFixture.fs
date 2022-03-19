namespace Translate.test

open Xunit
open Autofac

open Translator.Kernel
open Translator.ef

type AutofacFixture() =
    let repo = new WordRepo();

    let builder = 
        let builder = new ContainerBuilder()
        builder.RegisterInstance(TestFileHelpers.options)
        |> ignore

        builder.RegisterType<TranslateContext>()
        |> ignore

        builder.RegisterInstance(repo).As<WordRepo>()
        |> ignore

        builder.RegisterType<TranslatorViewModel>()
        |> ignore

        //builder.RegisterType<WordViewModel>()
        //|> ignore

        builder

    let container = builder.Build()

    member this.Container = container

[<CollectionDefinition("AutofacCollection")>]
type AutofacCollection() =
    interface ICollectionFixture<AutofacFixture>
