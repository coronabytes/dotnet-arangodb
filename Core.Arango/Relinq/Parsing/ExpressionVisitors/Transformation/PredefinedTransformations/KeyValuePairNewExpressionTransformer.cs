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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Arango.Relinq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations
{
    /// <summary>
    ///     Detects <see cref="NewExpression" /> nodes for <see cref="KeyValuePair{TKey,TValue}" /> and adds
    ///     <see cref="MemberInfo" /> metadata to those nodes.
    ///     This allows LINQ providers to match member access and constructor arguments more easily.
    /// </summary>
    internal class KeyValuePairNewExpressionTransformer : MemberAddingNewExpressionTransformerBase
    {
        protected override MemberInfo[] GetMembers(ConstructorInfo constructorInfo,
            ReadOnlyCollection<Expression> arguments)
        {
            return new[]
            {
                GetMemberForNewExpression(constructorInfo.DeclaringType, "Key"),
                GetMemberForNewExpression(constructorInfo.DeclaringType, "Value")
            };
        }

        protected override bool CanAddMembers(Type instantiatedType, ReadOnlyCollection<Expression> arguments)
        {
            return instantiatedType.Name == typeof(KeyValuePair<,>).Name
                   && instantiatedType.Namespace == typeof(KeyValuePair<,>).Namespace
                   && arguments.Count == 2;
        }
    }
}