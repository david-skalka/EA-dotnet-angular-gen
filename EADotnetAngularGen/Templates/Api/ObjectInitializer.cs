using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EADotnetAngularGen.Templates.Api
{
    public class ObjectInitializer
    {
        private readonly Dictionary<Type, Func<object, string>> _valueFormaters =
            new Dictionary<Type, Func<object, string>>
            {
                { typeof(string), value => "\"" + (string)value + "\"" },
                { typeof(int), value => ((int)value).ToString() },
                { typeof(bool), value => (bool)value ? "true" : "false" },
                { typeof(decimal), value => ((decimal)value).ToString(new CultureInfo("en-US")) + "m" }
            };


        private readonly string _name;
        private readonly Dictionary<string, object> _values;
        private readonly bool _simple;

        public ObjectInitializer(string name, Dictionary<string, object> values, bool simple)
        {
            this._name = name;
            this._values = values;
            this._simple = simple;
        }

        public string ToText()
        {
            return  (_simple ? "new()" : string.Format("new {0}()", _name)) + " { " + string.Join(", ",
                _values.Select(x => x.Key + "= " + _valueFormaters[x.Value.GetType()](x.Value))) + " }";
        }
    }
}