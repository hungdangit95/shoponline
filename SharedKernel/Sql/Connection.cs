using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel
{
    public static class Connection
    {
        private static IConfiguration Configuration { get; set; }
        private static IConfiguration _keyConfig;
        private static string _oracleConn;
        private static string _sqlConn;
        private static string _redisConn;
        public static void InitSetting()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public static string RedisConn
        {
            get
            {
                if (string.IsNullOrEmpty(_redisConn))
                {
                    InitSetting();
                    _redisConn = Configuration.GetConnectionString("RedisConn");
                }
                return _redisConn;
            }
        }


        public static string SqlConn
        {
            get
            {
                if (string.IsNullOrEmpty(_sqlConn))
                {
                    InitSetting();
                    _sqlConn = Configuration.GetConnectionString("SqlConnnectionString");
                }
                return _oracleConn;
            }
        }

        public static string OracleConn
        {
            get
            {
                if (string.IsNullOrEmpty(_oracleConn))
                {
                    InitSetting();
                    _oracleConn = Configuration.GetConnectionString("OracleConnectionString");
                }
                return _oracleConn;
            }
        }

        public static IConfiguration AppSettings
        {
            get
            {
                if (_keyConfig == null)
                {
                    InitSetting();
                    _keyConfig = Configuration.GetSection("AppSettings");
                }

                return _keyConfig;
            }
        }

        /// <summary>
        /// Gets an open READ UNCOMMITTED connection using the specified connection string, optionally timing out on the initial connect.
        /// </summary>
        /// <param name="connectionString">The connection string to use for the connection.</param>
        /// <param name="connectionTimeoutMs">(Optional) Milliseconds to wait to connect.</param>
        /// <returns>A READ UNCOMMITTED connection to the specified connection string.</returns>
        /// <exception cref="Exception">Throws if a connection isn't able to be made.</exception>
        /// <exception cref="TimeoutException">Throws if a connection can't be made in <paramref name="connectionTimeoutMs"/>.</exception>
        public static DbConnection GetOpen(string connectionString, int? connectionTimeoutMs = null)
        {

            var conn = new SqlConnection(connectionString);
            conn.Open();

            void setReadUncommitted(DbConnection c)
            {
                using (var cmd = c.CreateCommand())
                {
                    cmd.CommandText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";
                    cmd.ExecuteNonQuery();
                }
            }

            if (connectionTimeoutMs.GetValueOrDefault(0) == 0)
            {
                conn.OpenAsync();
                setReadUncommitted(conn);
            }
            else
            {
                // In the case of remote monitoring, the timeout will be at the NIC level, not responding to traffic,
                // in that scenario, connection timeouts don't really do much, because they're never reached, the timeout happens
                // before their timer starts.  Because of that, we need to spin up our own overall timeout

                try
                {
                    conn.Open();
                }
                catch (SqlException e)
                {
                    var csb = new SqlConnectionStringBuilder(connectionString);
                    var sqlException = $"Error opening connection to {csb.InitialCatalog} at {csb.DataSource} timeout was: {connectionTimeoutMs.ToString()} ms";
                    throw new Exception(sqlException, e);

                }


                setReadUncommitted(conn);
                if (conn.State == ConnectionState.Connecting)
                {
                    var b = new SqlConnectionStringBuilder { ConnectionString = connectionString };

                    throw new TimeoutException($"Timeout expired connecting to {b.InitialCatalog} on {b.DataSource} on in the alloted {connectionTimeoutMs.ToString()} ms");
                }
            }
            return conn;
        }

        /// <summary>
        /// Gets an open READ UNCOMMITTED connection using the specified connection string, optionally timing out on the initial connect.
        /// </summary>
        /// <param name="connectionString">The connection string to use for the connection.</param>
        /// <param name="connectionTimeoutMs">(Optional) Milliseconds to wait to connect.</param>
        /// <returns>A READ UNCOMMITTED connection to the specified connection string.</returns>
        /// <exception cref="Exception">Throws if a connection isn't able to be made.</exception>
        /// <exception cref="TimeoutException">Throws if a connection can't be made in <paramref name="connectionTimeoutMs"/>.</exception>
        public static async Task<DbConnection> GetOpenAsync(string connectionString, int? connectionTimeoutMs = null)
        {
            var conn = new SqlConnection(connectionString);

            if (connectionTimeoutMs.GetValueOrDefault(0) == 0)
            {
                await conn.OpenAsync().ConfigureAwait(false);
                await conn.SetReadUncommittedAsync().ConfigureAwait(false);
            }
            else
            {
                // In the case of remote monitoring, the timeout will be at the NIC level, not responding to traffic,
                // in that scenario, connection timeouts don't really do much, because they're never reached, the timeout happens
                // before their timer starts.  Because of that, we need to spin up our own overall timeout
                using (var tokenSource = new CancellationTokenSource())
                {
                    tokenSource.CancelAfter(connectionTimeoutMs.Value);
                    try
                    {
                        await conn.OpenAsync(tokenSource.Token).ConfigureAwait(false); // Throwing Null Refs
                        await conn.SetReadUncommittedAsync().ConfigureAwait(false);
                    }
                    catch (TaskCanceledException e)
                    {
                        conn.Close();
                        var csb = new SqlConnectionStringBuilder(connectionString);
                        var sqlException = $"Error opening connection to {csb.InitialCatalog} at {csb.DataSource}, timeout out at {connectionTimeoutMs.ToString()} ms";
                        throw new Exception(sqlException, e);
                    }
                    catch (SqlException e)
                    {
                        conn.Close();
                        var csb = new SqlConnectionStringBuilder(connectionString);
                        var sqlException = $"Error opening connection to {csb.InitialCatalog} at {csb.DataSource}: {e.Message}";
                        throw new Exception(sqlException, e);
                    }
                    if (conn.State == ConnectionState.Connecting)
                    {
                        tokenSource.Cancel();
                        var b = new SqlConnectionStringBuilder { ConnectionString = connectionString };
                        throw new TimeoutException($"Timeout expired connecting to {b.InitialCatalog} on {b.DataSource} on in the alloted {connectionTimeoutMs.Value.ToString()} ms");
                    }
                }
            }
            return conn;
        }
        public static async Task<int> SetReadUncommittedAsync(this DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            return 1;
        }

    }
}
