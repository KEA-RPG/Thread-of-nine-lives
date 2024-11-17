using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Backend.SecurityLogic
{
    public static class Sanitizer
    {
        private static readonly Regex ScriptTagRegex = new Regex(
            "<.*?script.*?>.*?</.*?script.*?>",
            RegexOptions.IgnoreCase | RegexOptions.Singleline,
            TimeSpan.FromSeconds(1)
        );

        private static readonly Regex HtmlTagRegex = new Regex(
            "<.*?>",
            RegexOptions.IgnoreCase,
            TimeSpan.FromSeconds(1)
        );

        public static T Sanitize<T>(T obj)
        {
            if (obj == null)
                return default;

            if (obj is string str)
            {
                return (T)(object)SanitizeString(str);
            }
            else if (obj is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    Sanitize(item);
                }
            }
            else
            {
                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    if (property.CanRead && property.CanWrite && property.PropertyType == typeof(string))
                    {
                        var value = (string)property.GetValue(obj);
                        if (value != null)
                        {
                            property.SetValue(obj, SanitizeString(value));
                        }
                    }
                    else if (property.CanRead && property.CanWrite && !property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                    {
                        var value = property.GetValue(obj);
                        if (value != null)
                        {
                            Sanitize(value);
                        }
                    }
                }
            }

            return obj;
        }

        private static string SanitizeString(string input)
        {
            // Remove any <script> tags
            var sanitized = ScriptTagRegex.Replace(input, string.Empty);

            // Optionally remove other HTML tags
            sanitized = HtmlTagRegex.Replace(sanitized, string.Empty);

            return sanitized;
        }
    }
}
