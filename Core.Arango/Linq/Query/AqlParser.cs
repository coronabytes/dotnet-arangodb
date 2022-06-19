using System.Linq;
using System.Reflection;
using Core.Arango.Linq.Interface;
using Core.Arango.Linq.Query.Clause;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.ExpressionVisitors.Transformation;
using Core.Arango.Relinq.Parsing.Structure;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;
using Core.Arango.Relinq.Parsing.Structure.NodeTypeProviders;
using GroupByExpressionNode = Core.Arango.Linq.Query.Clause.GroupByExpressionNode;
using SkipExpressionNode = Core.Arango.Linq.Query.Clause.SkipExpressionNode;
using TakeExpressionNode = Core.Arango.Linq.Query.Clause.TakeExpressionNode;

namespace Core.Arango.Linq.Query
{
    internal class AqlParser
    {
        internal static readonly MethodInfo[] OrderBySupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => Queryable.OrderBy<object, object>(null, null)),
            LinqUtility.GetSupportedMethod(() => Enumerable.OrderBy<object, object>(null, null)),
            LinqUtility.GetSupportedMethod(() => ArangoQueryableExtensions.Sort<object, object>(null, null))
        };

        internal static readonly MethodInfo[] OrderByDescendingSupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => Queryable.OrderByDescending<object, object>(null, null)),
            LinqUtility.GetSupportedMethod(() => Enumerable.OrderByDescending<object, object>(null, null)),
            LinqUtility.GetSupportedMethod(() => ArangoQueryableExtensions.SortDescending<object, object>(null, null))
        };

        public static readonly MethodInfo[] SelectManySupportedMethods =
        {
            LinqUtility.GetSupportedMethod(
                () => Queryable.SelectMany<object, object[], object>(null, o => null, null)),
            LinqUtility.GetSupportedMethod(
                () => Enumerable.SelectMany<object, object[], object>(null, o => null, null)),
            LinqUtility.GetSupportedMethod(
                () => Queryable.SelectMany<object, object[]>(null, o => null)),
            LinqUtility.GetSupportedMethod(
                () => Enumerable.SelectMany<object, object[]>(null, o => null)),
            LinqUtility.GetSupportedMethod(
                () => ArangoQueryableExtensions.For<object, object>(null, o => null))
        };

        private readonly IArangoLinq db;

        public AqlParser(IArangoLinq db)
        {
            this.db = db;
        }

        private IQueryParser CreateQueryParser()
        {
            var customNodeTypeRegistry = new MethodInfoBasedNodeTypeRegistry();

            //customNodeTypeRegistry.Register(new[] { typeof(EntitySet<>).GetMethod("Contains") }, typeof(ContainsExpressionNode));
            customNodeTypeRegistry.Register(GroupByExpressionNode.GetSupportedMethods, typeof(GroupByExpressionNode));
            customNodeTypeRegistry.Register(FilterExpressionNode.GetSupportedMethods, typeof(FilterExpressionNode));
            customNodeTypeRegistry.Register(LetSelectExpressionNode.SupportedMethods, typeof(LetSelectExpressionNode));
            customNodeTypeRegistry.Register(LetLambdaExpressionNode.SupportedMethods, typeof(LetLambdaExpressionNode));
            customNodeTypeRegistry.Register(TakeExpressionNode.SupportedMethods, typeof(TakeExpressionNode));
            customNodeTypeRegistry.Register(SkipExpressionNode.SupportedMethods, typeof(SkipExpressionNode));
            customNodeTypeRegistry.Register(OrderBySupportedMethods, typeof(OrderByExpressionNode));
            customNodeTypeRegistry.Register(OrderByDescendingSupportedMethods, typeof(OrderByDescendingExpressionNode));
            customNodeTypeRegistry.Register(SelectManySupportedMethods, typeof(SelectManyExpressionNode));
            customNodeTypeRegistry.Register(RemoveExpressionNode.SupportedMethods, typeof(RemoveExpressionNode));
            customNodeTypeRegistry.Register(InsertExpressionNode.SupportedMethods, typeof(InsertExpressionNode));
            customNodeTypeRegistry.Register(UpdateReplaceExpressionNode.SupportedMethods, typeof(UpdateReplaceExpressionNode));
            customNodeTypeRegistry.Register(PartialUpdateExpressionNode.SupportedMethods, typeof(PartialUpdateExpressionNode));
            customNodeTypeRegistry.Register(UpsertExpressionNode.SupportedMethods, typeof(UpsertExpressionNode));
            customNodeTypeRegistry.Register(SelectModificationExpressionNode.SupportedMethods,
                typeof(SelectModificationExpressionNode));
            customNodeTypeRegistry.Register(InModificationExpressionNode.SupportedMethods,
                typeof(InModificationExpressionNode));
            customNodeTypeRegistry.Register(IgnoreModificationSelectExpressionNode.SupportedMethods,
                typeof(IgnoreModificationSelectExpressionNode));
            customNodeTypeRegistry.Register(TraversalExpressionNode.SupportedMethods, typeof(TraversalExpressionNode));
            customNodeTypeRegistry.Register(TraversalDepthExpressionNode.SupportedMethods,
                typeof(TraversalDepthExpressionNode));
            customNodeTypeRegistry.Register(TraversalDirectionExpressionNode.SupportedMethods,
                typeof(TraversalDirectionExpressionNode));
            customNodeTypeRegistry.Register(TraversalGraphNameExpressionNode.SupportedMethods,
                typeof(TraversalGraphNameExpressionNode));
            customNodeTypeRegistry.Register(TraversalEdgeExpressionNode.SupportedMethods,
                typeof(TraversalEdgeExpressionNode));
            customNodeTypeRegistry.Register(TraversalOptionsExpressionNode.SupportedMethods,
                typeof(TraversalOptionsExpressionNode));
            customNodeTypeRegistry.Register(ShortestPathExpressionNode.SupportedMethods,
                typeof(ShortestPathExpressionNode));
            customNodeTypeRegistry.Register(OptionsExpressionNode.SupportedMethods,
                typeof(OptionsExpressionNode));

            var nodeTypeProvider = ExpressionTreeParser.CreateDefaultNodeTypeProvider();
            nodeTypeProvider.InnerProviders.Insert(0, customNodeTypeRegistry);

            var transformerRegistry = ExpressionTransformerRegistry.CreateDefault();
            var processor = ExpressionTreeParser.CreateDefaultProcessor(transformerRegistry);
            var expressionTreeParser = new ExpressionTreeParser(nodeTypeProvider, processor);
            return new QueryParser(expressionTreeParser);
        }

        private IQueryExecutor CreateQueryExecuter()
        {
            return new ArangoQueryExecuter(db);
        }

        internal ArangoQueryable<T> CreateQueryable<T>()
        {
            var queryParser = CreateQueryParser();
            var queryExecuter = CreateQueryExecuter();

            return new ArangoQueryable<T>(queryParser, queryExecuter, db);
        }
    }
}