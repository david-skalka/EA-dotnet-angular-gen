using EA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EADotnetAngularGen
{
    public class Info
    {
        public string ProjectName { get; set; }

        public int SeedCount { get; set; }

        public Info(string projectName, int seedCount)
        {
            ProjectName = projectName;
            SeedCount = seedCount;
        }

    }



    
    public static class AttributeExtensions
    {
        public static bool IsNullable(this EA.Attribute attribute)
        {
            return attribute.LowerBound == "0";

        }



        public static bool IsTypePrimitive(this EA.Attribute attribute)
        {
            return new []{ "int", "String", "Decimal", "DateTime", "Boolean" }.Contains(attribute.Type);
        }


        public static string DescriptionTag(this EA.Attribute attribute)
        {
            return attribute.TaggedValues.GetByName("Description").Value;

        }
    }



}
