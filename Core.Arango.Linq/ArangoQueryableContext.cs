using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Arango.Linq
{
    public class ArangoQueryableContext<T> : IOrderedQueryable<T>
    {
        public ArangoQueryableContext(ArangoContext arango, ArangoHandle handle, string collection)
        {
            Provider = new ArangoProvider(arango, handle, collection);
            Expression = Expression.Constant(this);
        }

        internal ArangoQueryableContext(IQueryProvider provider, Expression expression)
        {
            Provider = provider;
            Expression = expression;
        }


        public IEnumerator<T> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Provider.Execute<IEnumerable>(Expression).GetEnumerator();
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; }
        public IQueryProvider Provider { get; }
    }
}