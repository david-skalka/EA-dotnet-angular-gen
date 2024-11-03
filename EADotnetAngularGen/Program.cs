using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CaseExtensions;
using CommandLine;
using EA;
using EADotnetAngularGen.Templates.Api;
using EADotnetAngularGen.Templates.Client;
using JetBrains.Annotations;
using Medallion.Collections;
using Newtonsoft.Json.Linq;
using Sharprompt;
using Test = EADotnetAngularGen.Templates.Api.Test;

namespace EADotnetAngularGen
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Default)]
    internal class Program
    {
        private static IEnumerable<string> GetDependencies(IEnumerable<Element> diagram, string name)
        {
            var retD = diagram.Single(x => x.Name == name).Attributes.Cast<Attribute>().Where(x => !x.IsTypePrimitive())
                .Select(x => x.Type);

            return retD;
        }


        private static void Generate(Element[] elements, Info info, string outputDir, bool overwrite, string partsFilter)
        {
            var elementsSorted = elements.Select(x => x.Name)
                .OrderTopologicallyBy(name => GetDependencies(elements, name))
                .Select(x => elements.Single(y => y.Name == x)).ToArray();


            var entities = elementsSorted.Where(x => x.Stereotype == "Entity").ToArray();


            var testProjectPath = Path.Combine(outputDir, info.ProjectName + "IntegrationTest");

            var clientProjectPath = Path.Combine(outputDir, info.ProjectName + "Client");


            var pipeline = new Dictionary<string, IGeneratorCommand>
            {
                {
                    "initialize-solution", new MultiCommand(new ShellGeneratorCommand("dotnet", "new sln -n " + info.ProjectName + " -o " + outputDir + '"',
                        null), new ShellGeneratorCommand("dotnet",
                        "new webapi -f net8.0 -n " + info.ProjectName + " -o " +
                        Path.Combine(outputDir, info.ProjectName), null), new MkdirGeneratorCommand(Path.Combine(outputDir, info.ProjectName, "Models")), new MkdirGeneratorCommand(Path.Combine(outputDir, info.ProjectName, "Controllers")), new ShellGeneratorCommand("dotnet", "add package Microsoft.EntityFrameworkCore --version 8.0.6",
                        Path.Combine(outputDir, info.ProjectName)), new ShellGeneratorCommand("dotnet",
                        "add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.6",
                        Path.Combine(outputDir, info.ProjectName)), new T4GeneratorCommand(new Templates.Api.Program { Info = info },
                        Path.Combine(outputDir, info.ProjectName, "Program.cs"), true), new ShellGeneratorCommand("dotnet",
                        "new nunit -f net8.0 -n " + info.ProjectName + "IntegrationTest -o \"" + testProjectPath,
                        null), new MkdirGeneratorCommand(Path.Combine(testProjectPath, "Seeders")), new ShellGeneratorCommand("dotnet",
                        "add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.6", testProjectPath), new ShellGeneratorCommand("dotnet",
                        "add package JetBrains.Annotations --version 2024.2.0", testProjectPath), new ShellGeneratorCommand("dotnet", "add reference ../" + info.ProjectName, testProjectPath), new T4GeneratorCommand(new ISeeder { Info = info }, Path.Combine(testProjectPath, "ISeeder.cs"),
                        true), new T4GeneratorCommand(new CustomWebApplicationFactory { Info = info },
                        Path.Combine(testProjectPath, "CustomWebApplicationFactory.cs"), true), new ShellGeneratorCommand("dotnet",
                        "dotnet sln " + info.ProjectName + ".sln add " + info.ProjectName + " " + testProjectPath,
                        outputDir))
                },


                {
                    "initialize-angular", new MultiCommand(new ShellGeneratorCommand("npx",
                        "@angular/cli@18.0.7 new " + info.ProjectName + "Client --style scss --ssr false",
                        outputDir), new JsonCommand(Path.Combine(clientProjectPath, "angular.json"), des =>
                    {
                        ((JObject)des).Add("cli", JToken.FromObject(new { analytics = false }));
                        var testOptions = ((JObject)des)["projects"]?[info.ProjectName + "Client"]?["architect"]?["test"]?["options"] as JObject;
                        testOptions?.Add("codeCoverage", true);
                        testOptions?.Add("codeCoverageExclude", new JArray(new object[] { "src/app/api/**" }));

                        return des;
                    }), new JsonCommand(Path.Combine(clientProjectPath, "angular.json"), des =>
                    {
                        ((JObject)des.projects[info.ProjectName + "Client"].architect.serve).Add("options",
                            JToken.FromObject(new { proxyConfig = "proxy.conf.json" }));
                        return des;
                    }), new JsonCommand(Path.Combine(clientProjectPath, "package.json"), des =>
                    {

                        (((JObject)des)?.GetValue("scripts") as JObject)?.GetValue("start")?.Replace(new JValue("ng serve --ssl"));

                            

                        return des;
                    }), new T4GeneratorCommand(new ProxyConf(), Path.Combine(clientProjectPath, "proxy.conf.json"), true), new T4GeneratorCommand(new KarmaCiConf(), Path.Combine(clientProjectPath, "karma-ci.conf.js"), true), new T4GeneratorCommand(new AppConfig(),
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", "app.config.ts"), true), new ShellGeneratorCommand("npx",
                        "@angular/cli@18.0.7 add @angular/material --skip-confirmation --defaults",
                        clientProjectPath), new ShellGeneratorCommand("npm", "i @openapitools/openapi-generator-cli@2.13.13 -D",
                        clientProjectPath), new JsonCommand(Path.Combine(clientProjectPath, "package.json"), des =>
                        {
                            des.scripts["update-api"] =
                                "npx --yes concurrently -k -s first -n \"API,CLI\" -c \"magenta,blue\"  \"cd..\\" +
                                info.ProjectName +
                                "\\ && dotnet run --environment Development --urls https://localhost:7064;http://localhost:5195\" \"npx --yes wait-on http-get://127.0.0.1:5195/swagger/v1/swagger.json && openapi-generator-cli generate -i http://127.0.0.1:5195/swagger/v1/swagger.json -g typescript-angular --additional-properties=withInterfaces=true -o ./src/app/api\"";
                            return des;
                        }
                    ), new ShellGeneratorCommand("npx",
                        "@angular/cli@18.0.7 add @angular-eslint/schematics --skip-confirmation",
                        clientProjectPath), new T4GeneratorCommand(new EsLintConfig(), Path.Combine(clientProjectPath, "eslint.config.js"), true))
                },
                {
                    "db-context",
                    new T4GeneratorCommand(new DbContext { Info = info, Entities = entities },
                            Path.Combine(outputDir, info.ProjectName, "ApplicationDbContext.cs"), overwrite)
                },
                {
                    "seeder",
                    new T4GeneratorCommand(new Seeder { Entities = entities, Info = info },
                            Path.Combine(outputDir, info.ProjectName + "IntegrationTest", "Seeders",
                                "DefaultSeeder.cs"), overwrite)
                },
                {
                    "app-component", new MultiCommand(new T4GeneratorCommand(new AppComponent { Info = info },
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", "app.component.ts"),
                        overwrite), new T4GeneratorCommand(new AppTemplate { Entities = entities, Info = info },
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", "app.component.html"),
                        overwrite), new T4GeneratorCommand(new AppComponentSpec { Info = info },
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", "app.component.spec.ts"),
                        overwrite))
                },
                {
                    "app-routes",
                    new T4GeneratorCommand(new AppRoutes { Entities = entities },
                            Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", "app.routes.ts"),
                            overwrite)
                }
            };


            foreach (var entity in entities)
                pipeline.Add($"entity-{entity.Name.ToKebabCase()}",
                    new MultiCommand
                    (new T4GeneratorCommand(new EfModel { Model = entity, Info = info },
                        Path.Combine(outputDir, info.ProjectName, "Models", entity.Name + ".cs"), overwrite), new T4GeneratorCommand(new Controller { Model = entity, Info = info },
                        Path.Combine(outputDir, info.ProjectName, "Controllers", entity.Name + "Controller.cs"),
                        overwrite), new T4GeneratorCommand(new Test { Model = entity, Info = info },
                        Path.Combine(outputDir, info.ProjectName + "IntegrationTest", entity.Name + "Test.cs"),
                        overwrite), new MkdirGeneratorCommand(Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                        entity.Name.ToKebabCase() + "-edit")), new T4GeneratorCommand(new EditComponent { Model = entity },
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                            entity.Name.ToKebabCase() + "-edit", entity.Name.ToKebabCase() + "-edit.component.ts"),
                        overwrite), new T4GeneratorCommand(new EditTemplate { Model = entity },
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                            entity.Name.ToKebabCase() + "-edit",
                            entity.Name.ToKebabCase() + "-edit.component.html"), overwrite), new T4GeneratorCommand(new EditComponentSpec { Model = entity, Entities = entities },
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                            entity.Name.ToKebabCase() + "-edit",
                            entity.Name.ToKebabCase() + "-edit.component.spec.ts"), overwrite), new T4GeneratorCommand(new ListScss(),
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                            entity.Name.ToKebabCase() + "-edit",
                            entity.Name.ToKebabCase() + "-edit.component.scss"), overwrite), new MkdirGeneratorCommand(Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                        entity.Name.ToKebabCase() + "-list")), new T4GeneratorCommand(new ListComponent { Model = entity },
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                            entity.Name.ToKebabCase() + "-list", entity.Name.ToKebabCase() + "-list.component.ts"),
                        overwrite), new T4GeneratorCommand(new ListTemplate { Model = entity },
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                            entity.Name.ToKebabCase() + "-list",
                            entity.Name.ToKebabCase() + "-list.component.html"), overwrite), new T4GeneratorCommand(new ListComponentSpec { Model = entity },
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                            entity.Name.ToKebabCase() + "-list",
                            entity.Name.ToKebabCase() + "-list.component.spec.ts"), overwrite), new T4GeneratorCommand(new ListScss(),
                        Path.Combine(outputDir, info.ProjectName + "Client", "src", "app",
                            entity.Name.ToKebabCase() + "-list",
                            entity.Name.ToKebabCase() + "-list.component.scss"), overwrite)));

            var selectedParts = partsFilter!=null ? pipeline.Where(x=>Regex.Match(x.Key, partsFilter).Success).Select(x => x.Key) : Prompt.MultiSelect("Select parts", pipeline.Select(x => x.Key));


            foreach (var part in selectedParts) pipeline[part].Execute();
        }


        private static int Main(string[] args)
        {

          

            return Parser.Default.ParseArguments<RunPipeline>(args)
                .MapResult(options =>
                {
                    var outputDir = Path.Combine(Directory.GetCurrentDirectory(), options.OutputDir);


                    var repository = new Repository();

                    var file = Path.GetFullPath(options.File);

                    repository.OpenFile(file);

                    var elements = repository.Models.Cast<Package>().Single(x => x.Name == options.Package).Packages
                        .Cast<Package>().Single(x => x.Name == options.SubPackage).Elements.Cast<Element>().ToArray();
                    try
                    {
                        Generate(elements, new Info() { ProjectName= options.ProjectName, SeedCount = options.SeedCount }, outputDir,
                            options.Overwrite, options.PartsFilter);
                    }
                    finally
                    {
                        repository.CloseFile();
                        repository.Exit();
                    }


                    return 0;
                }, error => 1);
        }
    }


    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Default)]
    [Verb("run-pipeline")]
    internal class RunPipeline
    {
        [Option('d', "output-dir", Required = true)]
        public string OutputDir { get; set; } = string.Empty;

        [Option('f', "file", Required = true)] public string File { get; set; } = string.Empty;

        [Option('n', "project-name", Required = true)]
        public string ProjectName { get; set; } = string.Empty;

        [Option('o', "overwrite", Default = false)]
        public bool Overwrite { get; set; } = false;


        [Option('s', "seed-count", Default = 10)]
        public int SeedCount { get; set; } = 10;

        [Option('p', "parts")]
        public string PartsFilter { get; set; } = null;


        [Option("package", Default = "Model")] public string Package { get; set; } = string.Empty;

        [Option("sub-package", Default = "MainPackage")]
        public string SubPackage { get; set; } = string.Empty;
    }
}