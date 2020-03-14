using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Linq.Internal;

namespace Core.Arango.Linq
{
    public class ArangoProvider : IQueryProvider
    {
        private readonly ArangoContext _arango;
        private readonly ArangoHandle _handle;
        private readonly string _collection;

        public ArangoProvider(ArangoContext arango, ArangoHandle handle, string collection)
        {
            _arango = arango;
            _handle = handle;
            _collection = collection;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = expression.Type.GetElementType();
            try
            {
                return               
                    (IQueryable)Activator.CreateInstance(typeof(ArangoQueryableContext<>).
                        MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new ArangoQueryableContext<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            return default;
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var isEnumerable = (typeof(TResult).Name == "IEnumerable`1");

            var types = expression.Type.GenericTypeArguments;

            var writer = new AqlCodeWriter(expression);

            var query = writer.ToString();
            var bindVars = writer.BindVars;
            var type = writer.SelectType ?? typeof(TResult);

            //_arango.QueryAsync<JObject>()

            return default;
        }
    }
}