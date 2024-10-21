using System;
using System.Linq;
using AutoBogus;
using Attribute = EA.Attribute;

namespace EADotnetAngularGen
{
    public class Info
    {

        public string ProjectName { get; set; }

        public int SeedCount { get; set; }
    }


    public static class AttributeExtensions
    {
        public static bool IsNullable(this Attribute attribute)
        {
            return attribute.LowerBound == "0";
        }


        public static bool IsTypePrimitive(this Attribute attribute)
        {
            return new[] { "int", "String", "Decimal", "DateTime", "Boolean" }.Contains(attribute.Type);
        }


        public static string DescriptionTag(this Attribute attribute)
        {
            return attribute.TaggedValues.GetByName("Description").Value;
        }


        public static object GetFakeValue(this Attribute attribute)
        {
            switch (attribute.Type)
            {
                case "String":
                    return AutoFaker.Generate<string>();
                case "int":
                    return AutoFaker.Generate<int>();
                case "Boolean":
                    return AutoFaker.Generate<bool>();
                case "Decimal":
                    return AutoFaker.Generate<decimal>();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}