using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoBogus;
using CaseExtensions;
using EA;
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
                    return Math.Round(AutoFaker.Generate<decimal>(), 6);
                default:
                    throw new NotImplementedException();
            }
        }
        

        }



    public static class ElementExtensions
    {
        public static string JsObjectInitializer(this Element _, Dictionary<string, object> values)
        {

            var valueFormaters =
                new Dictionary<Type, Func<object, string>>
                {
                    { typeof(string), value => "\"" + (string)value + "\"" },
                    { typeof(int), value => ((int)value).ToString() },
                    { typeof(bool), value => (bool)value ? "true" : "false" },
                    { typeof(decimal), value => ((decimal)value).ToString(new CultureInfo("en-US")) }
                };


            return "{ " + string.Join(", ",
                values.Select(x => x.Key.ToCamelCase() + ":  " + valueFormaters[x.Value.GetType()](x.Value))) + " }";
        }
        
        
        
        
        public static string CsharpObjectInitializer(this Element element, Dictionary<string, object> values, bool simple)
        {

        var valueFormaters =
                new Dictionary<Type, Func<object, string>>
                {
                    { typeof(string), value => "\"" + (string)value + "\"" },
                    { typeof(int), value => ((int)value).ToString() },
                    { typeof(bool), value => (bool)value ? "true" : "false" },
                    { typeof(decimal), value => ((decimal)value).ToString(new CultureInfo("en-US")) + "m" }
                };


        return  (simple ? "new()" : $"new {element.Name}()") + " { " + string.Join(", ",
            values.Select(x => x.Key + "= " + valueFormaters[x.Value.GetType()](x.Value))) + " }";
        }
    }

}