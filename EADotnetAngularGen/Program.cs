using CaseExtensions;
using CommandLine;
using EA;
using EADotnetAngularGen.Templates.Api;
using EADotnetAngularGen.Templates.Client;
using Medallion.Collections;
using Newtonsoft.Json.Linq;
using Sharprompt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EADotnetAngularGen
{
    internal class Program
    {

        static IEnumerable<string> GetDependencies(IEnumerable<Element> diagram, string name)
        {
            var retD = diagram.Single(x => x.Name == name).Attributes.Cast<EA.Attribute>().Where(x => !x.IsTypePrimitive()).Select(x => x.Type);

            return retD;
        }



        static void Generate(Element[] elements, Info info, string outputDir, bool overwrite)
        {

            Element[] elementsSorted = elements.Select(x => x.Name).OrderTopologicallyBy(name => GetDependencies(elements, name)).Select(x => elements.Single(y => y.Name == x)).ToArray();


            Element[] entities = elementsSorted.Where(x => x.Stereotype == "Entity").ToArray();



            var testProjectPath = Path.Combine(outputDir, info.ProjectName + "IntegrationTest");

            var clientProjectPath = Path.Combine(outputDir, info.ProjectName + "Client");


            var pipeline = new Dictionary<string, IGeneratorCommand[]> {
                          { "initialize-solution",  new IGeneratorCommand[]{
                            new ShellGeneratorCommand("dotnet", "new sln -n " + info.ProjectName + " -o " + outputDir + '"', null) ,
                            new ShellGeneratorCommand("dotnet", "new webapi -f net8.0 -n " + info.ProjectName + " -o " + Path.Combine(outputDir, info.ProjectName), null),
                            new MkdirGeneratorCommand(Path.Combine(outputDir, info.ProjectName, "Models")),
                            new MkdirGeneratorCommand(Path.Combine(outputDir, info.ProjectName, "Controllers")),
                            new ShellGeneratorCommand("dotnet", "add package Microsoft.EntityFrameworkCore --version 8.0.6", Path.Combine(outputDir, info.ProjectName)),
                            new ShellGeneratorCommand("dotnet", "add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.6", Path.Combine(outputDir, info.ProjectName)),
                            new T4GeneratorCommand(new Templates.Api.Program() { Info  = info }, Path.Combine(outputDir, info.ProjectName, "Program.cs"), true),
                            new ShellGeneratorCommand("dotnet", "new nunit -f net8.0 -n " + info.ProjectName + "IntegrationTest -o \"" + testProjectPath, null),
                            new MkdirGeneratorCommand(Path.Combine(testProjectPath, "Seeders")),
                            new ShellGeneratorCommand("dotnet", "add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.6", testProjectPath),
                            new ShellGeneratorCommand("dotnet", "add reference ../" + info.ProjectName, testProjectPath),
                            new T4GeneratorCommand(new ISeeder() { Info=info }, Path.Combine(testProjectPath, "ISeeder.cs"), true),
                            new T4GeneratorCommand(new CustomWebApplicationFactory() { Info=info }, Path.Combine(testProjectPath, "CustomWebApplicationFactory.cs"), true),
                            new ShellGeneratorCommand("dotnet", "dotnet sln " + info.ProjectName + ".sln add " + info.ProjectName + " " + testProjectPath, outputDir)}
                          } ,


                          { "initialize-angular",  new IGeneratorCommand[]{

                            new ShellGeneratorCommand("npx", "@angular/cli@18.0.7 new " + info.ProjectName + "Client --style scss --ssr false", outputDir),
                            new JsonCommand(Path.Combine(clientProjectPath, "angular.json"), (dynamic des) => {
                                ((JObject)des).Add("cli", JToken.FromObject(new { analytics = false }));
                                return des;
                            }),
                            new JsonCommand(Path.Combine(clientProjectPath, "angular.json"), (dynamic des) => {
                                ((JObject)des.projects[info.ProjectName + "Client"].architect.serve).Add("options", JToken.FromObject(new { proxyConfig = "proxy.conf.json" }));
                                return des;
                            }),
                            new JsonCommand(Path.Combine(clientProjectPath, "package.json"), (dynamic des) => {
                                ((JObject)des)["scripts"]["start"] = "ng serve --ssl";
                                return des;
                            }),
                            new T4GeneratorCommand(new ProxyConf() { }, Path.Combine(clientProjectPath, "proxy.conf.json"), true),
                            new T4GeneratorCommand(new AppConfig(), Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", "app.config.ts"), true),
                            new ShellGeneratorCommand("npx", "@angular/cli@18.0.7 add @angular/material --skip-confirmation --defaults", clientProjectPath),
                            new ShellGeneratorCommand("npm", "i @openapitools/openapi-generator-cli@2.13.13 -D", clientProjectPath),
                            new JsonCommand(Path.Combine(clientProjectPath, "package.json"), (dynamic des) => {
                                des.scripts["update-api"] = "npx --yes concurrently -k -s first -n \"API,CLI\" -c \"magenta,blue\"  \"cd..\\" + info.ProjectName + "\\ && dotnet run --environment Development --urls https://localhost:7064;http://localhost:5195\" \"npx --yes wait-on http-get://127.0.0.1:5195/swagger/v1/swagger.json && openapi-generator-cli generate -i http://127.0.0.1:5195/swagger/v1/swagger.json -g typescript-angular --additional-properties=withInterfaces=true -o ./src/app/api\"";
                                    return des;
                                }),

                          }},
                          { "db-context", new IGeneratorCommand[]{ new T4GeneratorCommand(new DbContext() { Info=info, Entities = entities }, Path.Combine(outputDir, info.ProjectName, "ApplicationDbContext.cs"), overwrite) } } ,
                          { "seeder",  new IGeneratorCommand[]{new T4GeneratorCommand(new Seeder() { Entities = entities, Info=info}, Path.Combine(outputDir, info.ProjectName + "IntegrationTest", "Seeders", "DefaultSeeder.cs"), overwrite) }  },
                          { "app-component",  new IGeneratorCommand[]{ new T4GeneratorCommand( new AppComponent() { }, Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", "app.component.ts"), overwrite), new T4GeneratorCommand(new AppTemplate() { Entities = entities, Info = info }, Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", "app.component.html"), overwrite) }  },
                          { "app-routes",  new IGeneratorCommand[]{ new T4GeneratorCommand( new AppRoutes() { Entities = entities }, Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", "app.routes.ts"), overwrite)}  }

                         };



            foreach (var entity in entities)
            {

                pipeline.Add(string.Format("entity-{0}", entity.Name.ToKebabCase()),
                   new IGeneratorCommand[] {
                            new T4GeneratorCommand(new EfModel() { Model = entity, Info=info }, Path.Combine(outputDir, info.ProjectName, "Models", entity.Name + ".cs"), overwrite),
                            new T4GeneratorCommand(new Controller() { Model = entity, Info=info }, Path.Combine(outputDir, info.ProjectName, "Controllers", entity.Name + "Controller.cs"), overwrite),
                            new T4GeneratorCommand(new Templates.Api.Test() { Model = entity,Info=info }, Path.Combine(outputDir, info.ProjectName + "IntegrationTest", entity.Name + "Test.cs"), overwrite),
                            new MkdirGeneratorCommand(Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", entity.Name.ToKebabCase() + "-edit")),
                            new T4GeneratorCommand(new EditComponent() { Model = entity }, Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", entity.Name.ToKebabCase() + "-edit", entity.Name.ToKebabCase() + "-edit.component.ts"), overwrite),
                            new T4GeneratorCommand(new EditTemplate() { Model = entity }, Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", entity.Name.ToKebabCase() + "-edit", entity.Name.ToKebabCase() + "-edit.component.html"), overwrite),
                            new T4GeneratorCommand(new ListScss(), Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", entity.Name.ToKebabCase() + "-edit", entity.Name.ToKebabCase() + "-edit.component.scss"), overwrite),
                            new MkdirGeneratorCommand(Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", entity.Name.ToKebabCase() + "-list")),
                            new T4GeneratorCommand(new ListComponent() { Model = entity }, Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", entity.Name.ToKebabCase() + "-list", entity.Name.ToKebabCase() + "-list.component.ts"), overwrite),
                            new T4GeneratorCommand(new ListTemplate() { Model = entity }, Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", entity.Name.ToKebabCase() + "-list", entity.Name.ToKebabCase() + "-list.component.html"), overwrite),
                            new T4GeneratorCommand(new ListScss(), Path.Combine(outputDir, info.ProjectName + "Client", "src", "app", entity.Name.ToKebabCase() + "-list", entity.Name.ToKebabCase() + "-list.component.scss"), overwrite),

            });

            }

            var selectedParts = Prompt.MultiSelect("Select parts", pipeline.Select(x => x.Key));



            foreach (var part in selectedParts)
            {
                pipeline[part].ToList().ForEach(x => x.Execute());
            }




        }





        static int Main(string[] args)
        {

            return Parser.Default.ParseArguments<RunPipeline>(args)
                    .MapResult(options =>
                    {

                        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), options.OutputDir);




                        Repository repository = new Repository();

                        var file = Path.GetFullPath(options.File);

                        repository.OpenFile(file);

                        var elements = repository.Models.Cast<Package>().Single(x => x.Name == options.Package).Packages.Cast<Package>().Single(x => x.Name == options.SubPackage).Elements.Cast<Element>().ToArray();
                        try
                        {
                            Generate(elements, new Info(options.ProjectName, options.SeedCount), outputDir, options.Overwrite);
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






    [Verb("run-pipeline")]
    class RunPipeline
    {
        [Option('d', "output-dir", Required = true)]
        public string OutputDir { get; set; } = String.Empty;

        [Option('f', "file", Required = true)]
        public string File { get; set; } = String.Empty;

        [Option('n', "project-name", Required = true)]
        public string ProjectName { get; set; } = String.Empty;

        [Option('o', "overwrite", Default = false)]
        public bool Overwrite { get; set; } = false;


        [Option('s', "seed-count", Default = 10)]
        public int SeedCount { get; set; } = 10;


        [Option("package", Default = "Model")]
        public string Package { get; set; } = string.Empty;

        [Option("sub-package", Default = "MainPackage")]
        public string SubPackage { get; set; } = string.Empty;

    }


}



