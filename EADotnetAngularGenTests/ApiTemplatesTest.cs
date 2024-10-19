using System;
using System.IO;
using System.Linq;
using EA;
using EADotnetAngularGen;
using EADotnetAngularGen.Templates.Api;
using NUnit.Framework;
using Attribute = EA.Attribute;
using Test = EADotnetAngularGen.Templates.Api.Test;

namespace EADotnetAngularGenTests
{
    public class ApiTemplatesTest
    {
        private Element[] diagram;

        private Info info = new Info("Sample", 10);

        private readonly Repository repository = new Repository();

        [OneTimeSetUp]
        public void Setup()
        {
            repository.OpenFile(Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory,
                @"..\..\..\Data\SampleModel.qea")));

            diagram = repository.Models.Cast<Package>().Single(x => x.Name == "Model").Packages.Cast<Package>()
                .Single(x => x.Name == "MainPackage").Elements.Cast<Element>().ToArray();

            info = new Info("TestProject", 10);
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            repository.CloseFile();
            repository.Exit();
        }


        [Test]
        public void ControllerTest()
        {
            var content =
                new Controller { Model = diagram.Single(x => x.Name == "Comment"), Info = info }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void CustomWebApplicationFactory()
        {
            var content = new CustomWebApplicationFactory { Info = info }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void EfModelTest()
        {
            var content = new EfModel { Model = diagram.Single(x => x.Name == "Comment"), Info = info }.TransformText();
            Console.WriteLine(content);
        }

        [Test]
        public void ISeederTest()
        {
            var content = new ISeeder { Info = info }.TransformText();
            Console.WriteLine(content);
        }

        [Test]
        public void ProgramTest()
        {
            var content = new Program { Info = info }.TransformText();
            Console.WriteLine(content);
        }

        [Test]
        public void SeederTest()
        {
            var content = new Seeder { Entities = diagram, Info = info }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void TestTest()
        {
            var content = new Test { Model = diagram.Single(x => x.Name == "Comment"), Info = info }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void ObjectInitializer()
        {
            var model = diagram.Single(x => x.Name == "Comment");
            var content = new ObjectInitializer(model.Name,
                model.Attributes.Cast<Attribute>().Where(x => x.IsTypePrimitive())
                    .ToDictionary(x => x.Name, x => x.GetFakeValue())).ToText();
            Console.WriteLine(content);
        }
    }
}