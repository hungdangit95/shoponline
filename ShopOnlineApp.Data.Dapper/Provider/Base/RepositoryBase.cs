using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace ShopOnlineApp.Data.Dapper
{
    public abstract class RepositoryBase<T> : RepositoryDataTypeBase where T : class
    {
        #region Common method for bussiness
        public virtual async Task<T> GetAsync(string id, bool useTransaction = false, int? commandTimeout = null)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            var result = await session.Connection.GetAsync<T>(id, transaction, commandTimeout);
            session.Dispose();

            return result;
        }

        public virtual async Task<T> GetFirstOrDefaultAsync(string sql, object param = null, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            var result = await session.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout);

            session.Dispose();

            return result;
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(string sql, object param = null, bool useTransaction = false,
            int? commandTimeout = null, CommandType? commandType = null)
        {
            if (string.IsNullOrEmpty(sql))
                return null;

            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            var result = await session.Connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);

            session.Dispose();

            return result;
        }

        public virtual async Task<int> InsertAsync(T entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            int result = await session.Connection.InsertAsync(entity, transaction, commandTimeout);

            session.Dispose();

            return result;
        }

        public virtual async Task<bool> UpdateAsync(T entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            bool result = await session.Connection.UpdateAsync(entity, transaction, commandTimeout);

            session.Dispose();

            return result;
        }

        public virtual async Task<bool> DeleteAsync(T entity, bool useTransaction = false, int? commandTimeout = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            bool result = await session.Connection.DeleteAsync(entity, transaction, commandTimeout);

            session.Dispose();

            return result;
        }

        public virtual async Task<int> ExecuteAsync(string sql, object param = null, bool useTransaction = false, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            IDbSession session = DbSession;

            IDbTransaction transaction = null;
            if (useTransaction)
            {
                session.BeginTrans();
                transaction = session.Transaction;
            }

            int result = await session.Connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);

            session.Dispose();

            return result;
        }
        #endregion

        #region Excute store produce Normal methods
        public virtual T ExcuteSingle(string schemal, string storeName, IDynamicParameters dyParam)
        {
            T result;
            IDbSession session = DbSession;
            result = session.Connection.QuerySingleOrDefault<T>($"{schemal}.{storeName}", param: dyParam, commandType: CommandType.StoredProcedure);
            session.Dispose();
            return result;
        }
        public virtual object ExcuteScalar(string schemal, string storeName, IDynamicParameters dyParam)
        {
            IDbSession session = DbSession;
            var result = session.Connection.ExecuteScalar($"{schemal}.{storeName}", param: dyParam, commandType: CommandType.StoredProcedure);
            session.Dispose();
            return result;
        }
        public virtual IEnumerable<T> ExcuteMany(string schemal, string storeName, IDynamicParameters dyParam)
        {
            List<T> result = new List<T>();
            IDbSession session = DbSession;
            var data = session.Connection.Query<T>($"{schemal}.{storeName}", param: dyParam, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                result = data.ToList();
            }
            session.Dispose();
            return result;
        }
        public virtual int ExcuteNoneQuery(string schemal, string storeName, IDynamicParameters dyParam)
        {
            IDbSession session = DbSession;
            int result = session.Connection.Execute($"{schemal}.{storeName}", param: dyParam, commandType: CommandType.StoredProcedure);
            session.Dispose();
            return result;
        }
        public virtual void ExcuteMultiple(string schemal, string storeName, IDynamicParameters dyParam, Action<GridReader> action)
        {
            IDbSession session = DbSession;
            using (var multi = session.Connection.QueryMultiple($"{schemal}.{storeName}", dyParam, null, null, CommandType.StoredProcedure))
            {
                action(multi);
            }
            session.Dispose();
        }

        #endregion

        #region  Excute store produce Async methods
        public virtual async Task<T> ExcuteSingleAsync(string schemal, string storeName, IDynamicParameters dyParam)
        {
            IDbSession session = DbSession;
            T result = await session.Connection.QuerySingleOrDefaultAsync<T>($"{schemal}.{storeName}", param: dyParam, commandType: CommandType.StoredProcedure);
            session.Dispose();
            return result;
        }
        public virtual async Task<object> ExcuteScalarAsync(string schemal, string storeName, IDynamicParameters dyParam)
        {
            IDbSession session = DbSession;
            var result = await session.Connection.ExecuteScalarAsync($"{schemal}.{storeName}", param: dyParam, commandType: CommandType.StoredProcedure);
            session.Dispose();
            return result;
        }

        public virtual async Task<IEnumerable<T>> ExcuteManyAsync(string schemal, string storeName, IDynamicParameters dyParam)
        {
            List<T> result = new List<T>();
            IDbSession session = DbSession;
            var data = await session.Connection.QueryAsync<T>($"{schemal}.{storeName}", param: dyParam, commandType: CommandType.StoredProcedure);
            if (data != null)
            {
                result = data.ToList();
            }
            session.Dispose();
            return result;
        }

        public virtual async Task<int> ExcuteNoneQueryAsync(string schemal, string storeName, IDynamicParameters dyParam)
        {
            IDbSession session = DbSession;
            int result = await session.Connection.ExecuteAsync($"{schemal}.{storeName}", param: dyParam, commandType: CommandType.StoredProcedure);
            session.Dispose();
            return result;
        }

        public virtual async Task ExcuteMultipleAsync(string schemal, string storeName, IDynamicParameters dyParam, Action<GridReader> action)
        {
            IDbSession session = DbSession;
            using (var multi = await session.Connection.QueryMultipleAsync($"{schemal}.{storeName}", dyParam, null, null, CommandType.StoredProcedure))
            {
                action(multi);
            }
            session.Dispose();
        }

        #endregion

        //#region Excute Utility
        //public async Task<object> ExcuteAsyncGeneric(string schemal, object model, string storedName, ExcuteTypeEnum excuteType,
        //    IEnumerable<string> prefixs = null, Action<Dictionary<string, object>> actionOutPutValue = null)
        //{
        //    OracleAddParameters param = await OracleMappingValue.GetParamWithValueByQuery(schemal, storedName, model);

        //    Dictionary<string, object> outPutData = new Dictionary<string, object>();
        //    dynamic data = null;

        //    switch (excuteType)
        //    {
        //        case ExcuteTypeEnum.RETURN_NONE:
        //            data = await ExcuteNoneQueryAsync(schemal, storedName, param);
        //            outPutData = GetOutPutValue(param);
        //            if (actionOutPutValue != null)
        //                actionOutPutValue(outPutData);
        //            return data;

        //        case ExcuteTypeEnum.RETURN_SCALAR:
        //            data = await ExcuteScalarAsync(schemal, storedName, param);
        //            outPutData = GetOutPutValue(param);
        //            if (actionOutPutValue != null)
        //                actionOutPutValue(outPutData);
        //            return data;

        //        case ExcuteTypeEnum.RETURN_SINGLE:
        //            data = await ExcuteSingleAsync(schemal, storedName, param);
        //            outPutData = GetOutPutValue(param);
        //            if (actionOutPutValue != null)
        //                actionOutPutValue(outPutData);
        //            return OracleUtils.ToObjectJson(data);
        //        case ExcuteTypeEnum.RETURN_PAGING:
        //            Pagination dataPaging = new Pagination();
        //            data = await ExcuteManyAsync(schemal, storedName, param);
        //            dataPaging.data = OracleUtils.ToListObjectJson(data);
        //            dataPaging.tong_so_dong = param.Get<OracleDecimal>("b_tong_so_dong").Value;
        //            outPutData = GetOutPutValue(param);
        //            if (actionOutPutValue != null)
        //                actionOutPutValue(outPutData);
        //            return dataPaging;

        //        case ExcuteTypeEnum.RETURN_LIST:
        //            data = await ExcuteManyAsync(schemal, storedName, param);
        //            outPutData = GetOutPutValue(param);
        //            if (actionOutPutValue != null)
        //                actionOutPutValue(outPutData);
        //            return OracleUtils.ToListObjectJson(data);

        //        case ExcuteTypeEnum.RETURN_MULTIPLE:
        //            Dictionary<string, object> dic = new Dictionary<string, object>();
        //            await ExcuteMultipleAsync(schemal, storedName, param, grid =>
        //            {
        //                foreach (var p in param.parameters)
        //                {
        //                    if (p.Key.ToLower().StartsWith("cur_"))
        //                    {
        //                        dic.Add(p.Key.Substring(4, p.Key.Length - 4), OracleUtils.ToObjectJson(grid.Read<object>().FirstOrDefault()));
        //                    }
        //                    if (p.Key.ToLower().StartsWith("curs_"))
        //                    {
        //                        dic.Add(p.Key.Substring(5, p.Key.Length - 5), OracleUtils.ToListObjectJson(grid.Read<object>().ToList()));
        //                    }
        //                }
        //            });
        //            dynamic res = dic;
        //            outPutData = GetOutPutValue(param);
        //            if (actionOutPutValue != null)
        //                actionOutPutValue(outPutData);
        //            return res;
        //        default:
        //            break;
        //    }
        //    return null;
        //}

        //public async Task<Dictionary<string, object>> ExcuteAsync(Dictionary<string, object> parammeterValues, string schemal, string storedName)
        //{
        //    OracleAddParameters param = await OracleMappingValue.GetParamWithValueByQuery(schemal, storedName, parammeterValues);

        //    Dictionary<string, object> outputData = new Dictionary<string, object>();
        //    await ExcuteMultipleAsync(schemal, storedName, param, grid =>
        //    {
        //        foreach (var p in param.parameters)
        //        {
        //            if (p.Key.ToLower().StartsWith("curs_"))
        //            {
        //                outputData.Add(p.Key.Substring(5, p.Key.Length - 5), OracleUtils.ToListObjectJson(grid.Read<object>().ToList()));
        //            }
        //        }
        //    });

        //    return outputData;
        //}

        //#endregion

        //#region Private method
        //private Dictionary<string, object> GetOutPutValue(OracleAddParameters param)
        //{
        //    Dictionary<string, object> res = new Dictionary<string, object>();

        //    if (param != null && param.parameters != null && param.parameters.Values != null)
        //    {
        //        var paramInfos = param.parameters.Values.
        //            Where(n => n.ParameterDirection != ParameterDirection.Input
        //                        && n.DbType != OracleDbType.RefCursor).ToList();

        //        if (paramInfos != null)
        //        {
        //            foreach (var paramInfo in paramInfos)
        //            {
        //                var propertyName = paramInfo.Name.ToLower().StartsWith("b_") ? paramInfo.Name.ToLower().Substring(2, paramInfo.Name.Length - 2) : paramInfo.Name.ToLower();
        //                switch (paramInfo.DbType)
        //                {
        //                    case OracleDbType.Decimal:
        //                        res.Add(propertyName, param.Get<OracleDecimal>(paramInfo.Name).Value);
        //                        break;
        //                    case OracleDbType.NChar:
        //                    case OracleDbType.NVarchar2:
        //                    case OracleDbType.Varchar2:
        //                    case OracleDbType.Char:
        //                        res.Add(propertyName, param.Get<OracleString>(paramInfo.Name).Value);
        //                        break;
        //                    case OracleDbType.Clob:
        //                    case OracleDbType.NClob:
        //                        res.Add(propertyName, param.Get<OracleClob>(paramInfo.Name).Value);
        //                        break;
        //                    case OracleDbType.Date:
        //                        res.Add(propertyName, param.Get<OracleDate>(paramInfo.Name).Value);
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    return res;
        //}
        // #endregion
    }
}
