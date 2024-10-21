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
        private Element[] _diagram;

        private Info _info = new Info() { ProjectName= "Sample", SeedCount=10};

        private readonly Repository _repository = new Repository();

        [OneTimeSetUp]
        public void Setup()
        {
            _repository.OpenFile(Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory,
                @"..\..\..\Data\SampleModel.qea")));

            _diagram = _repository.Models.Cast<Package>().Single(x => x.Name == "Model").Packages.Cast<Package>()
                .Single(x => x.Name == "MainPackage").Elements.Cast<Element>().ToArray();

            _info = new Info() { ProjectName= "TestProject",  SeedCount=10 };
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            _repository.CloseFile();
            _repository.Exit();
        }


        [Test]
        public void ControllerTest()
        {
            var content =
                new Controller { Model = _diagram.Single(x => x.Name == "Comment"), Info = _info }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void CustomWebApplicationFactory()
        {
            var content = new CustomWebApplicationFactory { Info = _info }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void EfModelTest()
        {
            var content = new EfModel { Model = _diagram.Single(x => x.Name == "Comment"), Info = _info }.TransformText();
            Console.WriteLine(content);
        }

        [Test]
        public void SeederInterfaceTest()
        {
            var content = new ISeeder { Info = _info }.TransformText();
            Console.WriteLine(content);
        }

        [Test]
        public void ProgramTest()
        {
            var content = new Program { Info = _info }.TransformText();
            Console.WriteLine(content);
        }

        [Test]
        public void SeederTest()
        {
            var content = new Seeder { Entities = _diagram, Info = _info }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void TestTest()
        {
            var content = new Test { Model = _diagram.Single(x => x.Name == "Comment"), Info = _info }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void ObjectInitializer()
        {
            var model = _diagram.Single(x => x.Name == "Comment");
            var content = new ObjectInitializer(model.Name,
                model.Attributes.Cast<Attribute>().Where(x => x.IsTypePrimitive())
                    .ToDictionary(x => x.Name, x => x.GetFakeValue())).ToText();
            Console.WriteLine(content);
        }
    }
}