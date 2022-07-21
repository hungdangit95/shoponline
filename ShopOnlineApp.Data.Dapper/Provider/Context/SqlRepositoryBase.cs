using SqlKata;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineApp.Data.Dapper
{
    public class SqlRepositoryBase<T> : RepositoryBase<T> where T : class
    {
        protected sealed override DatabaseType DataType => DatabaseType.Mssql;
        protected override string ConnStrKey => string.Empty;
        protected override string TableName => typeof(T).Name;
        protected override SqlResult GetSqlResult(Query query)
        {
            var compiler = new SqlServerCompiler
            {
                UseLegacyPagination = false
            };

            SqlResult sqlResult = compiler.Compile(query);

            return sqlResult;
        }
    }
}
