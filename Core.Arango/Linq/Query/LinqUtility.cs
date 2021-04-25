using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Linq.Interface;
using Core.Arango.Linq.Query.Clause;
using Core.Arango.Linq.Utility;
using Core.Arango.Relinq.Clauses;
using Core.Arango.Relinq.Parsing.Structure.NodeTypeProviders;

namespace Core.Arango.Linq.Query
{
    internal static class LinqUtility
    {
        public static T NextBodyClause<T>(this ObservableCollection<IBodyClause> bodyClauses, int index = 0)
            where T : class
        {
            var clause = bodyClauses.Skip(index).FirstOrDefault(b => b is T);

            return clause == null ? null : clause as T;
        }

        private static MethodInfo GetMethod<T>(Expression<Func<T>> wrappedCall)
        {
            Utils.CheckNotNull("wrappedCall", wrappedCall);

            switch (wrappedCall.Body.NodeType)
            {
                case ExpressionType.Call:
                    return ((MethodCallExpression) wrappedCall.Body).Method;
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression) wrappedCall.Body;
                    var property = memberExpression.Member as PropertyInfo;
                    var method = property != null ? property.GetMethod : null;
                    if (method != null)
                        return method;
                    break;
            }

            throw new ArgumentException(
                string.Format("Cannot extract a method from the given expression '{0}'.", wrappedCall.Body),
                "wrappedCall");
        }

        public static MethodInfo GetSupportedMethod<T>(Expression<Func<T>> methodCall)
        {
            CheckNotNull("methodCall", methodCall);

            var method = GetMethod(methodCall);
            return MethodInfoBasedNodeTypeRegistry.GetRegisterableMethodDefinition(method, true);
        }

        public static string ResolveCollectionName(IArangoLinq db, Type itemType)
        {
            var collectionName = db.ResolveCollectionName(itemType);
            return AddBacktickToName(collectionName);
        }

        public static string ResolveMemberNameRaw(IArangoLinq db, MemberInfo memberInfo)
        {
            return ResolvePropertyNameRaw(db.ResolvePropertyName(memberInfo.DeclaringType, memberInfo.Name));
        }

        public static string ResolveMemberName(IArangoLinq db, MemberInfo memberInfo)
        {
            return AddBacktickToName(ResolveMemberNameRaw(db, memberInfo));
        }

        public static string ResolvePropertyNameRaw(string name)
        {
            return name.Replace("<", "").Replace(">", "");
        }

        public static string ResolvePropertyName(string name)
        {
            return AddBacktickToName(ResolvePropertyNameRaw(name));
        }

        private static string AddBacktickToName(string name)
        {
            return string.Format("`{0}`", name);
        }

        public static ArangoModelVisitor FindParentModelVisitor(ArangoModelVisitor modelVisitor)
        {
            var parentModelVisitor = modelVisitor;
            do
            {
                if (parentModelVisitor.ParnetModelVisitor == null)
                    return parentModelVisitor;

                parentModelVisitor = parentModelVisitor.ParnetModelVisitor;
            } while (true);
        }

        public static string MemberNameFromMap(string guid, string prefix, ArangoModelVisitor modelVisitor)
        {
            var mappings = FindParentModelVisitor(modelVisitor).MemberNamesMapping;

            if (mappings.ContainsKey(guid))
                return mappings[guid];

            var memberName = $"{prefix}_{mappings.Count}";
            mappings[guid] = memberName;

            return memberName;
        }

        public static T CheckNotNull<T>(string argumentName, T actualValue)
        {
            if (actualValue == null)
                throw new ArgumentNullException(argumentName);

            return actualValue;
        }

        public static string CheckNotNullOrEmpty(string argumentName, string actualValue)
        {
            CheckNotNull(argumentName, actualValue);
            if (actualValue.Length == 0)
                throw CreateArgumentEmptyException(argumentName);

            return actualValue;
        }

        public static ArgumentException CreateArgumentEmptyException(string argumentName)
        {
            return new(string.Format("Parameter '{0}' cannot be empty.", argumentName), argumentName);
        }

        public static Type CheckTypeIsAssignableFrom(string argumentName, Type actualType, Type expectedType)
        {
            CheckNotNull("expectedType", expectedType);
            if (actualType != null)
                if (!expectedType.GetTypeInfo().IsAssignableFrom(actualType.GetTypeInfo()))
                {
                    var message = string.Format(
                        "Parameter '{0}' is a '{2}', which cannot be assigned to type '{1}'.",
                        argumentName,
                        expectedType,
                        actualType);
                    throw new ArgumentException(message, argumentName);
                }

            return actualType;
        }

        public static List<GroupByClause> PriorGroupBy(ArangoModelVisitor modelVisitor)
        {
            var clauses = new List<GroupByClause>();

            FindGroupByRecursive(modelVisitor, clauses);

            return clauses;
        }

        private static void FindGroupByRecursive(ArangoModelVisitor modelVisitor, List<GroupByClause> clauses)
        {
            if (modelVisitor == null)
                return;

            var groupByClauses = modelVisitor.QueryModel.BodyClauses.Where(x => x is GroupByClause)
                .Select(x => x as GroupByClause).Where(x => x.Visited);

            foreach (var g in groupByClauses)
            {
                clauses.Add(g);
                if (!g.GroupOnLastGroup)
                    return;
            }

            FindGroupByRecursive(modelVisitor.ParnetModelVisitor, clauses);
        }
    }
}