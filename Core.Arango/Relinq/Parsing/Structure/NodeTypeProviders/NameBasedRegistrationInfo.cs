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
using System.Reflection;
using Remotion.Utilities;

namespace Core.Arango.Relinq.Parsing.Structure.NodeTypeProviders
{
    /// <summary>
    ///     Defines a name and a filter predicate used when determining the matching expression node type by
    ///     <see cref="MethodNameBasedNodeTypeRegistry" />.
    /// </summary>
    internal sealed class NameBasedRegistrationInfo
    {
        public NameBasedRegistrationInfo(string name, Func<MethodInfo, bool> filter)
        {
            ArgumentUtility.CheckNotNullOrEmpty("name", name);
            ArgumentUtility.CheckNotNull("filter", filter);

            Name = name;
            Filter = filter;
        }

        public string Name { get; }

        public Func<MethodInfo, bool> Filter { get; }
    }
}