using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Application.Dapper.Provider
{
    public class SessionFactory
    {
        //private static IDbConnection CreateConnection(DatabaseType dataType = DatabaseType.Mssql, string connStrKey = "")
        //{
        //    try
        //    {
        //        IDbConnection conn;

        //        // string connectionString = GetConnectionString(connStrKey);
        //        string connectionString = Connection.OracleConn;

        //        switch (dataType)
        //        {
        //            case DatabaseType.Oracle:
        //                conn = new OracleConnection(connectionString);
        //                break;
        //            case DatabaseType.Mssql:
        //                conn = new SqlConnection(connectionString);
        //                break;
        //            //case DatabaseType.Mysql:
        //            //    conn = new MySqlConnection(connectionString);
        //            //    break;

        //            default:
        //                conn = new OracleConnection(connectionString);
        //                break;
        //        }

        //        conn.Open();

        //        return conn;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        //private static string GetConnectionString(string connStrKey)
        //{
        //    return Connection.OracleConn;
        //}

        //public static IDbSession CreateSession(DatabaseType databaseType, string key)
        //{
        //    IDbConnection conn = CreateConnection(databaseType, key);
        //    IDbSession session = new DbSession(conn);
        //    return session;
        //}
    }
}
