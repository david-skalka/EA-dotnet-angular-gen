using EA;
using System;
using System.Collections.Generic;
using AutoBogus;

namespace EADotnetAngularGen
{
    public static class ElementAutoFaker
    {
        public static Dictionary<string, object> GenerateFromElement(Element el)
        {
            var retD = new Dictionary<string, object>();

            foreach (EA.Attribute attr in el.Attributes)
            {
                if (attr.IsTypePrimitive())
                {
                    var fakeValue = GetFakeValue(attr.Type);
                    retD.Add(attr.Name, fakeValue);
                }
                else
                {
                    retD.Add(attr.Name + "Id", AutoFaker.Generate<int>());
                }
            }


            return retD;
        }




        static object GetFakeValue(string type)
        {


            switch (type)
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
