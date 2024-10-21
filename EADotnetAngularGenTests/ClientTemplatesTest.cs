using System;
using System.IO;
using System.Linq;
using EA;
using EADotnetAngularGen;
using EADotnetAngularGen.Templates.Client;
using NUnit.Framework;

namespace EADotnetAngularGenTests
{
    public class ClientTemplatesTest
    {
        private Element[] _diagram;

        private readonly Info _info = new Info() { ProjectName = "Sample", SeedCount = 10 };

        private readonly Repository _repository = new Repository();


        [OneTimeSetUp]
        public void Setup()
        {
            _repository.OpenFile(Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory,
                @"..\..\..\Data\SampleModel.qea")));

            _diagram = _repository.Models.Cast<Package>().Single(x => x.Name == "Model").Packages.Cast<Package>()
                .Single(x => x.Name == "MainPackage").Elements.Cast<Element>().ToArray();


        }


        // one time tear down
        [OneTimeTearDown]
        public void TearDown()
        {
            _repository.CloseFile();
            _repository.Exit();
        }


        [Test]
        public void EditComponentTest()
        {
            var content = new EditComponent { Model = _diagram.Single(x => x.Name == "Product") }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void EditTemplateTest()
        {
            var content = new EditTemplate { Model = _diagram.Single(x => x.Name == "Product") }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void EditComponentSpecTest()
        {
            var content = new EditComponentSpec { Model = _diagram.Single(x => x.Name == "Comment"), Entities = _diagram }
                .TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void ListComponentTest()
        {
            var content = new ListComponent { Model = _diagram.Single(x => x.Name == "Product") }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void ListComponentSpecTest()
        {
            var content = new ListComponentSpec { Model = _diagram.Single(x => x.Name == "Comment") }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void ListTemplateTest()
        {
            var content = new ListTemplate { Model = _diagram.Single(x => x.Name == "Comment") }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void AppRoutes()
        {
            var content = new AppRoutes { Entities = _diagram }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void AppTemplate()
        {
            var content = new AppTemplate { Entities = _diagram, Info = _info }.TransformText();
            Console.WriteLine(content);
        }

        [Test]
        public void AppComponentSpec()
        {
            var content = new AppComponentSpec { Info = _info }.TransformText();
            Console.WriteLine(content);
        }
    }
}