using System;
using System.Text.Json;

namespace Core.Arango.Serialization.Json
{
    /// <summary>
    ///     System.Json.Text PascalCase Naming Policy for Arango
    /// </summary>
    public class ArangoJsonCamelCasePolicy : JsonNamingPolicy
    {
        /// <inheritdoc />
        public override string ConvertName(string name)
        {
            return name switch
            {
                "Id" => "_id",
                "Key" => "_key",
                "Revision" => "_rev",
                "From" => "_from",
                "To" => "_to",
                _ => Fix(name)
            };
        }

        /// <summary>
        /// Convert a string to PascalCase unless it is empty or already starts with UpperCase
        /// </summary>
        private static string Fix(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
                return name;

#if NETSTANDARD2_0
            var newString = new Span<char>();
            name.AsSpan().CopyTo(newString);
            FixCasing(newString);
            return newString.ToString();
#else
            return string.Create(name.Length, name, (chars, n) =>
            {
                n.AsSpan().CopyTo(chars);
                FixCasing(chars);
            });
#endif
        }

        private static void FixCasing(Span<char> chars)
        {
            for (var i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i])) break;

                var hasNext = i + 1 < chars.Length;

                // Stop when next char is already lowercase.
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    // If the next char is a space, lowercase current char before exiting.
                    if (chars[i + 1] == ' ') chars[i] = char.ToLowerInvariant(chars[i]);

                    break;
                }

                chars[i] = char.ToLowerInvariant(chars[i]);
            }
        }
    }
}