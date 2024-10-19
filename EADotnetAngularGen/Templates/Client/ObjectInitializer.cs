using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CaseExtensions;

namespace EADotnetAngularGen.Templates.Client
{
    public class ObjectInitializer
    {
        private readonly Dictionary<Type, Func<object, string>> _valueFormaters =
            new Dictionary<Type, Func<object, string>>
            {
                { typeof(string), value => "\"" + (string)value + "\"" },
                { typeof(int), value => ((int)value).ToString() },
                { typeof(bool), value => (bool)value ? "true" : "false" },
                { typeof(decimal), value => ((decimal)value).ToString(new CultureInfo("en-US")) }
            };


        public Dictionary<string, object> values;


        public ObjectInitializer(Dictionary<string, object> values)
        {
            this.values = values;
        }

        public string ToText()
        {
            return "{ " + string.Join(", ",
                values.Select(x => x.Key.ToCamelCase() + ":  " + _valueFormaters[x.Value.GetType()](x.Value))) + " }";
        }
    }
}