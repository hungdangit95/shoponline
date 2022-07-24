using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace ShopOnlineApp.Data.Dapper
{
    public class SessionFactory
    {
        private static IDbConnection conn;
        private static IDbConnection CreateConnection(DatabaseType dataType = DatabaseType.Mssql, string connStrKey = "")
        {
            try
            {
                // string connectionString = GetConnectionString(connStrKey);
                string connectionString = Connection.SqlConn;

                switch (dataType)
                {
                    case DatabaseType.Oracle:
                        // conn = new OracleConnection(connectionString);
                        break;
                    case DatabaseType.Mssql:
                        conn = new SqlConnection(connectionString);
                        break;
                    //case DatabaseType.Postgre:
                    //    conn = new PostgreConnection(connectionString);
                    //    break;

                    default:
                        conn = new SqlConnection(connectionString);
                        break;
                }

                return conn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string GetConnectionString(string connStrKey)
        {
            return Connection.OracleConn;
        }

        public static IDbSession CreateSession(DatabaseType databaseType, string key)
        {
            IDbConnection conn = CreateConnection(databaseType, key);
            IDbSession session = new DbSession(conn);
            return session;
        }
    }
}
