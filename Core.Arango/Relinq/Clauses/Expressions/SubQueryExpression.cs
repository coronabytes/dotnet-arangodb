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

using System;
using System.Linq.Expressions;
using Core.Arango.Relinq.Parsing;
using Remotion.Utilities;

namespace Core.Arango.Relinq.Clauses.Expressions
{
    /// <summary>
    ///     Represents an <see cref="Expression" /> that holds a subquery. The subquery is held by <see cref="QueryModel" /> in
    ///     its parsed form.
    /// </summary>
    internal sealed class SubQueryExpression : Expression
    {
#if NET_3_5
    public const ExpressionType ExpressionType = (ExpressionType) 100002;
#endif

#if !NET_3_5
#endif

        public SubQueryExpression(QueryModel queryModel)
#if NET_3_5
        : base (ExpressionType, queryModel.GetOutputDataInfo().DataType)
#endif
        {
            ArgumentUtility.CheckNotNull("queryModel", queryModel);

#if !NET_3_5
            Type = queryModel.GetOutputDataInfo().DataType;
#endif
            QueryModel = queryModel;
        }

#if !NET_3_5
        public override ExpressionType NodeType => ExpressionType.Extension;

        public override Type Type { get; }
#endif

        public QueryModel QueryModel { get; }

#if !NET_3_5
        public override string ToString()
        {
            return "{" + QueryModel + "}";
        }

        protected override Expression Accept(ExpressionVisitor visitor)
        {
            ArgumentUtility.CheckNotNull("visitor", visitor);

            var relinqVisitor = visitor as RelinqExpressionVisitor;
            if (relinqVisitor == null)
                return base.Accept(visitor);

            return relinqVisitor.VisitSubQuery(this);
        }
#endif
    }
}