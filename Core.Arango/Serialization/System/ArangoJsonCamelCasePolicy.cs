using System;
using System.Text.Json;

namespace Core.Arango.Serialization.System
{
    public class ArangoJsonCamelCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name switch
            {
                "Key" => "_key",
                "Id" => "_id",
                "From" => "_from",
                "To" => "_to",
                _ => Fix(name)
            };
        }

        private static string Fix(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
                return name;

            return string.Create(name.Length, name, (chars, name) =>
            {
                name.AsSpan().CopyTo(chars);
                FixCasing(chars);
            });
        }

        private static void FixCasing(Span<char> chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);

                // Stop when next char is already lowercase.
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    // If the next char is a space, lowercase current char before exiting.
                    if (chars[i + 1] == ' ')
                    {
                        chars[i] = char.ToLowerInvariant(chars[i]);
                    }

                    break;
                }

                chars[i] = char.ToLowerInvariant(chars[i]);
            }
        }
    }
}