﻿using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Repository.Implementation
{
    public class PgSQLContext : IDisposable, IBaseContext
    {
        private readonly NpgsqlConnection _dbConn;
        private string _connectionString = "";

        public PgSQLContext(string connectionString)
        {
            _connectionString = connectionString;
            _dbConn = new NpgsqlConnection(_connectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public void Delete(string sql, object parameters = null)
        {
            using (_dbConn)
            {
                Open();
                _dbConn.Execute(sql, parameters);
            }
        }

        public int Insert<T>(string sql, object poco)
        {
            using (_dbConn)
            {
                Open();
                return _dbConn.ExecuteScalar<int>(sql, (T)poco);
            }
        }

        public void InsertBulk<T>(string sql, object listPoco)
        {
            using (_dbConn)
            {
                Open();
                using (NpgsqlTransaction trans = _dbConn.BeginTransaction())
                {
                    _dbConn.Execute(sql, listPoco, transaction: trans);
                    trans.Commit();
                }
            }
        }

        public T Select<T>(string sql, object parameters = null) where T : new()
        {
            using (_dbConn)
            {
                Open();
                var o = _dbConn.Query<T>(sql, parameters).FirstOrDefault();
                if (o != null)
                    return o;

                return new T();
            }
        }

        public List<T> SelectList<T>(string sql, object parameters = null)
        {
            using (_dbConn)
            {
                Open();
                return _dbConn.Query<T>(sql, parameters).ToList();
            }
        }

        public void Update<T>(string sql, object poco)
        {
            using (_dbConn)
            {
                Open();
                _dbConn.Execute(sql, (T)poco);
            }
        }

        public void ExecuteNonQuery(string sql)
        {
            using (_dbConn)
            {
                Open();
                using (NpgsqlCommand command = new NpgsqlCommand(sql, _dbConn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        #region helpers
        public void Dispose()
        {
            _dbConn.Close();
            _dbConn.Dispose();
        }
        public void Open()
        {
            if (_dbConn.State == ConnectionState.Closed)
                _dbConn.Open();
        }
        #endregion
    }
}
