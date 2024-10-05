using EA;
using EADotnetAngularGen.Templates.Client;
using EADotnetAngularGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;

namespace EADotnetAngularGenTests
{
    
    public class ClientTemplatesTest
    {

        private Element[] diagram;

        private Info info = new Info("Sample", 10);

        EA.Repository repository = new EA.Repository();


        [OneTimeSetUp]
        public void Setup()
        {
            
            repository.OpenFile(Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\Data\SampleModel.qea")));

            diagram = repository.Models.Cast<EA.Package>().Single(x => x.Name == "Model").Packages.Cast<Package>().Single(x => x.Name == "MainPackage").Elements.Cast<Element>().ToArray();

            info = new Info("TestProject", 10);
        }



        // one time tear down
        [OneTimeTearDown]
        public void TearDown()
        {
            repository.CloseFile();
            repository.Exit();
        }




        [Test]
        public void EditComponentTest()
        {
            var content = new EditComponent() { Model = diagram.Single(x => x.Name == "Comment"), }.TransformText();
            Console.WriteLine(content);
        }



        [Test]
        public void EditTemplateTest()
        {
            var content = new EditTemplate() { Model = diagram.Single(x => x.Name == "Comment"), }.TransformText();
            Console.WriteLine(content);
        }



        [Test]
        public void ListComponentTest()
        {
            var content = new ListComponent() { Model = diagram.Single(x => x.Name == "Comment"), }.TransformText();
            Console.WriteLine(content);
        }



        [Test]
        public void ListTemplateTest()
        {
            var content = new ListTemplate() { Model = diagram.Single(x => x.Name == "Comment"), }.TransformText();
            Console.WriteLine(content);
        }






        [Test]
        public void AppRoutes()
        {

            var content = new AppRoutes() { Entities = diagram }.TransformText();
            Console.WriteLine(content);
        }


        [Test]
        public void AppTemplate()
        {

            var content = new AppTemplate() { Entities = diagram, Info = new Info("Sample", 10) }.TransformText();
            Console.WriteLine(content);
        }




    }

}
