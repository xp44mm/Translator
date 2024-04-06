module Translate.test.TestFileHelpers


open Microsoft.Extensions.Configuration
open Microsoft.EntityFrameworkCore
open Translator.Scaffold

//open System.Text.RegularExpressions
//open System.IO
//open Microsoft.Extensions.PlatformAbstractions;
//let GetSolutionDirectory() =
//    let host = new ApplicationEnvironment();
//    let pathToManipulate = host.ApplicationBasePath;

//    let rgx = Regex(@"^(.+)\\[^\\]+\\bin\\.*$",RegexOptions.IgnoreCase)
//    rgx.Replace(pathToManipulate,"$1")

//let path = Directory.GetCurrentDirectory()

//let GetConfiguration() =
    
//    let builder = ConfigurationBuilder()
//                    .SetBasePath(__SOURCE_DIRECTORY__)
//                    .AddJsonFile("appsettings.json", false, false)
//                    //.AddEnvironmentVariables()

//    builder.Build()

//let getConnectionString() = 
//    let config = GetConfiguration()   
//    config.GetConnectionString("DefaultConnection")

//let options:DbContextOptions<TranslateContext> = 
//    let builder = new DbContextOptionsBuilder<TranslateContext>()
//    let connectionString = getConnectionString()
//    builder.UseSqlServer(connectionString) |> ignore
//    builder.Options

    

