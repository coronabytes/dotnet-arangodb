using System;

namespace Core.Arango.Linq.Query.Clause
{
    internal interface IModificationClause
    {
        string ItemName { get; set; }

        Type CollectionType { get; set; }

        bool IgnoreSelect { get; set; }
    }
}