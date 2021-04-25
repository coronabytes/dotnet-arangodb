using System.Collections.Generic;
using System.Linq;
using Core.Arango.Linq.Interface;
using Core.Arango.Relinq;

namespace Core.Arango.Linq.Query
{
    internal class ArangoQueryExecuter : IQueryExecutor
    {
        private readonly IArangoLinq db;

        public ArangoQueryExecuter(IArangoLinq db)
        {
            this.db = db;
        }

        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            return ExecuteCollection<T>(queryModel).Single();
        }

        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            return returnDefaultWhenEmpty
                ? ExecuteCollection<T>(queryModel).SingleOrDefault()
                : ExecuteCollection<T>(queryModel).Single();
        }

        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            var visitor = new ArangoModelVisitor(db);
            visitor.VisitQueryModel(queryModel);
            visitor.QueryData.Query = visitor.QueryText.ToString();

            return db.ExecAsync<T>(visitor.QueryData.Query, visitor.QueryData.BindVars).Result.AsEnumerable();
        }
    }
}