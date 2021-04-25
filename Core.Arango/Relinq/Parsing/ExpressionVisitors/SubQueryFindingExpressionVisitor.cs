// Copyright (c) rubicon IT GmbH, www.rubicon.eu
//
// See the NOTICE file distributed with this work for additional information
// regarding copyright ownership.  rubicon licenses this file to you under 
// the Apache License, Version 2.0 (the "License"); you may not use this 
// file except in compliance with the License.  You may obtain a copy of the 
// License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the 
// License for the specific language governing permissions and limitations
// under the License.
// 

using System.Linq.Expressions;
using Core.Arango.Relinq.Clauses.Expressions;
using Core.Arango.Relinq.Parsing.Structure;
using Core.Arango.Relinq.Parsing.Structure.ExpressionTreeProcessors;
using Remotion.Utilities;

namespace Core.Arango.Relinq.Parsing.ExpressionVisitors
{
    /// <summary>
    ///     Preprocesses an expression tree for parsing. The preprocessing involves detection of sub-queries and VB-specific
    ///     expressions.
    /// </summary>
    internal sealed class SubQueryFindingExpressionVisitor : RelinqExpressionVisitor
    {
        public static Expression Process(Expression expressionTree, INodeTypeProvider nodeTypeProvider)
        {
            ArgumentUtility.CheckNotNull("expressionTree", expressionTree);
            ArgumentUtility.CheckNotNull("nodeTypeProvider", nodeTypeProvider);

            var visitor = new SubQueryFindingExpressionVisitor(nodeTypeProvider);
            return visitor.Visit(expressionTree);
        }

        private readonly INodeTypeProvider _nodeTypeProvider;
        private readonly ExpressionTreeParser _expressionTreeParser;
        private readonly QueryParser _queryParser;

        private SubQueryFindingExpressionVisitor(INodeTypeProvider nodeTypeProvider)
        {
            ArgumentUtility.CheckNotNull("nodeTypeProvider", nodeTypeProvider);

            _nodeTypeProvider = nodeTypeProvider;
            _expressionTreeParser = new ExpressionTreeParser(_nodeTypeProvider, new NullExpressionTreeProcessor());
            _queryParser = new QueryParser(_expressionTreeParser);
        }

        public override Expression Visit(Expression expression)
        {
            var potentialQueryOperatorExpression = _expressionTreeParser.GetQueryOperatorExpression(expression);
            if (potentialQueryOperatorExpression != null &&
                _nodeTypeProvider.IsRegistered(potentialQueryOperatorExpression.Method))
                return CreateSubQueryNode(potentialQueryOperatorExpression);
            return base.Visit(expression);
        }

#if NET_3_5
    protected override Expression VisitRelinqUnknownNonExtension (Expression expression)
    {
      //ignore
      return expression;
    }
#endif

        private SubQueryExpression CreateSubQueryNode(MethodCallExpression methodCallExpression)
        {
            var queryModel = _queryParser.GetParsedQuery(methodCallExpression);
            return new SubQueryExpression(queryModel);
        }
    }
}