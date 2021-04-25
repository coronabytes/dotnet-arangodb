using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Arango.Linq.Query;

namespace Core.Arango.Linq.Utility
{
    internal class ReflectionUtils
    {
        public static MemberInfo[] GetMember(Type type, string member)
        {
            return type.GetMember(member);
        }

        public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
        {
            return type.GetConstructors();
        }

        public static Type GetItemTypeOfClosedGenericIEnumerable(Type enumerableType, string argumentName)
        {
            LinqUtility.CheckNotNull("enumerableType", enumerableType);
            LinqUtility.CheckNotNullOrEmpty("argumentName", argumentName);

            Type itemType;
            if (!TryGetItemTypeOfClosedGenericIEnumerable(enumerableType, out itemType))
            {
                var message =
                    string.Format("Expected a closed generic type implementing IEnumerable<T>, but found '{0}'.",
                        enumerableType);
                throw new ArgumentException(message, argumentName);
            }

            return itemType;
        }

        private static bool TryGetItemTypeOfClosedGenericIEnumerable(Type possibleEnumerableType, out Type itemType)
        {
            LinqUtility.CheckNotNull("possibleEnumerableType", possibleEnumerableType);

            var possibleEnumerableTypeInfo = possibleEnumerableType.GetTypeInfo();

            if (possibleEnumerableTypeInfo.IsArray)
            {
                itemType = possibleEnumerableTypeInfo.GetElementType();
                return true;
            }

            if (!IsIEnumerable(possibleEnumerableTypeInfo))
            {
                itemType = null;
                return false;
            }

            if (possibleEnumerableTypeInfo.IsGenericTypeDefinition)
            {
                itemType = null;
                return false;
            }

            if (possibleEnumerableTypeInfo.IsGenericType &&
                possibleEnumerableType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                itemType = possibleEnumerableTypeInfo.GenericTypeArguments[0];
                return true;
            }

            var implementedEnumerableInterface = possibleEnumerableTypeInfo.ImplementedInterfaces
                .Select(t => t.GetTypeInfo())
                .FirstOrDefault(IsGenericIEnumerable);

            if (implementedEnumerableInterface == null)
            {
                itemType = null;
                return false;
            }

            itemType = implementedEnumerableInterface.GenericTypeArguments[0];
            return true;
        }

        private static bool IsIEnumerable(TypeInfo type)
        {
            return typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(type);
        }

        private static bool IsGenericIEnumerable(TypeInfo enumerableType)
        {
            return IsIEnumerable(enumerableType)
                   && enumerableType.IsGenericType
                   && enumerableType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }
    }
}