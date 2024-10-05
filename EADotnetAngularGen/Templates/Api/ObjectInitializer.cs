using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EADotnetAngularGen.Templates.Api
{
    public class ObjectInitializer
    {


        public string name;
        public Dictionary<string, object> values;


        public ObjectInitializer(string name, Dictionary<string, object> values)
        {
            this.name = name;
            this.values = values;
        }

        Dictionary<Type, Func<object, string>> _valueFormaters = new Dictionary<Type, Func<object, string>>() {
            { typeof(string), (value) => "\"" + ((string)value) + "\"" },
            { typeof(int), (value) => ((int)value).ToString() },
            { typeof(bool), (value) => ((bool)value) ? "true" : "false"},
            { typeof(decimal), (value) => ((decimal)value).ToString(new CultureInfo("en-US"))+ "m" } };

        public string ToText()
        {
            return "new " + name + "() { " + string.Join(", ", values.Select(x => x.Key + "= " + _valueFormaters[x.Value.GetType()](x.Value))) + " }";
        }

    }


}
