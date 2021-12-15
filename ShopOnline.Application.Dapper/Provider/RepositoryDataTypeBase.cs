using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Application.Dapper.Provider
{
    public abstract class RepositoryDataTypeBase
    {
        //protected IDbSession DbSession => SessionFactory.CreateSession(DataType, ConnStrKey);
        protected abstract string ConnStrKey { get; }
        protected abstract DatabaseType DataType { get; }

        protected abstract string TableName { get; }

      //  protected abstract SqlResult GetSqlResult(Query query);
    }
}
