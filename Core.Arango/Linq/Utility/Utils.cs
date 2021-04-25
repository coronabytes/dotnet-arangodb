using System;
using Core.Arango.Linq.Collection;

namespace Core.Arango.Linq.Utility
{
    internal class Utils
    {
        public static T CheckNotNull<T>(string argumentName, T actualValue)
        {
            if (actualValue == null)
                throw new ArgumentNullException(argumentName);

            return actualValue;
        }

        public static string EdgeDirectionToString(EdgeDirection direction)
        {
            switch (direction)
            {
                case EdgeDirection.Any:
                    return "any";
                case EdgeDirection.Inbound:
                    return "inbound";
                case EdgeDirection.Outbound:
                    return "outbound";
                default:
                    throw new InvalidOperationException(
                        $"EdgeDirection {direction} binding not found, this is a client bug");
            }
        }
    }
}