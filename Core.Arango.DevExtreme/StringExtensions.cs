using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Arango.DevExtreme
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };
        }

        /// <summary>
        ///     foo.bar.something -> Foo.Bar.Something
        /// </summary>
        public static string FirstCharOfPropertiesToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                {
                    var expression = @"[\.]{1}([a-z])";
                    var charArray = input.ToCharArray();
                    foreach (Match match in Regex.Matches(input, expression, RegexOptions.Singleline))
                        charArray[match.Groups[1].Index] = char.ToUpper(charArray[match.Groups[1].Index]);
                    var output = FirstCharToUpper(new string(charArray));
                    return output;
                }
                    ;
            }
        }
    }
}