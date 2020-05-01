using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using static Core.Arango.Linq.Internal.FormatterNames;

#nullable enable

namespace Core.Arango.Linq.Internal.Util.Extensions
{
    internal static class StringExtensions
    {
        private static readonly char[] specialChars =
        {
            '\'', '\"', '\\', '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v'
        };

        public static bool IsNullOrWhitespace([NotNullWhen(false)] this string? s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static bool ContainsAny(this string s, params string[] testStrings)
        {
            return testStrings.Any(x => s.Contains(x));
        }

        public static void AppendTo(this string s, StringBuilder sb)
        {
            sb.Append(s);
        }

        // https://stackoverflow.com/a/14502246/111794
        private static string ToCSharpLiteral(this string input)
        {
            var literal = new StringBuilder("\"", input.Length + 2);
            foreach (var c in input)
                switch (c)
                {
                    case '\'':
                        literal.Append(@"\'");
                        break;
                    case '\"':
                        literal.Append("\\\"");
                        break;
                    case '\\':
                        literal.Append(@"\\");
                        break;
                    case '\0':
                        literal.Append(@"\0");
                        break;
                    case '\a':
                        literal.Append(@"\a");
                        break;
                    case '\b':
                        literal.Append(@"\b");
                        break;
                    case '\f':
                        literal.Append(@"\f");
                        break;
                    case '\n':
                        literal.Append(@"\n");
                        break;
                    case '\r':
                        literal.Append(@"\r");
                        break;
                    case '\t':
                        literal.Append(@"\t");
                        break;
                    case '\v':
                        literal.Append(@"\v");
                        break;
                    default:
                        if (char.GetUnicodeCategory(c) != UnicodeCategory.Control)
                        {
                            literal.Append(c);
                        }
                        else
                        {
                            literal.Append(@"\u");
                            literal.Append(((ushort) c).ToString("x4"));
                        }

                        break;
                }

            literal.Append("\"");
            return literal.ToString();
        }

        public static bool HasSpecialCharacters(this string s)
        {
            return s.IndexOfAny(specialChars) > -1;
        }

        public static string ToVerbatimString(this string s, string language)
        {
            return language switch
            {
                CSharp => s.ToCSharpLiteral(),
                VisualBasic => $"\"{s.Replace("\"", "\"\"")}\"",
                _ => throw new ArgumentException("Invalid language")
            };
        }

        public static void AppendLineTo(this string s, StringBuilder sb, int indentationLevel = 0)
        {
            s = (s ?? "").TrimEnd();
            var toAppend = new string(' ', indentationLevel * 4) + s.TrimEnd();
            sb.AppendLine(toAppend);
        }

        public static string? ToCamelCase(this string s)
        {
            if (s == null || s.Length == 0) return s;
            return char.ToLowerInvariant(s[0]) + s.Substring(1);
        }
    }
}