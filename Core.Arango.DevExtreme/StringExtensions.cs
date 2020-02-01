using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Arango.DevExtreme
{
    internal static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }  
        
        public static string FirstCharOfPropertiesToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default:
                {
                    string expression = @"[\.]{1}([a-z])";
                    char[] charArray = input.ToCharArray();
                    foreach (Match match in Regex.Matches(input, expression,RegexOptions.Singleline))
                    {
                        charArray[match.Groups[1].Index] = char.ToUpper(charArray[match.Groups[1].Index]);
                    }
                    string output = FirstCharToUpper(new string(charArray));
                    return output;
                };
            }
        }
    }
}