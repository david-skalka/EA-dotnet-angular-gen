using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pluralize.NET;
namespace EADotnetAngularGen
{
    public static class StringPluralizerExtension
    {
        public static string Pluralize(this string str)
        {
            return new Pluralizer().Pluralize(str);
        }
    }
}
