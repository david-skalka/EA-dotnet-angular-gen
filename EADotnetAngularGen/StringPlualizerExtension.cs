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