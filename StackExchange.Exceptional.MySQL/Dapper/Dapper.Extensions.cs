﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace StackExchange.Exceptional.Dapper
{
    internal static class DapperExtensions
    {
        public static int Execute(this DbConnection conn, string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (conn.EnsureOpen())
            {
                return SqlMapper.Execute(conn, sql, param as object, transaction, commandTimeout: commandTimeout);
            }
        }

        public static IEnumerable<T> Query<T>(this DbConnection conn, string sql, dynamic param = null, bool buffered = true, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            using (conn.EnsureOpen())
            {
                return SqlMapper.Query<T>(conn, sql, param as object, transaction, buffered, commandTimeout);
            }
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this DbConnection conn, string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            using (conn.EnsureOpen())
            {
                return SqlMapper.Query<TReturn>(conn, sql, param as object, transaction, buffered, commandTimeout);
            }
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this DbConnection conn, string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            using (conn.EnsureOpen())
            {
                return SqlMapper.Query<TReturn>(conn, sql, param as object, transaction, buffered, commandTimeout);
            }
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this DbConnection conn, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            using (conn.EnsureOpen())
            {
                return SqlMapper.Query<TReturn>(conn, sql, param as object, transaction, buffered, commandTimeout);
            }
        }

        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this DbConnection conn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            using (conn.EnsureOpen())
            {
                return SqlMapper.Query<TReturn>(conn, sql, param as object, transaction, buffered, commandTimeout);
            }
        }

        public static IEnumerable<dynamic> Query(this DbConnection conn, string sql, dynamic param = null, bool buffered = true, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            using (conn.EnsureOpen())
            {
                return SqlMapper.Query(conn, sql, param as object, transaction, buffered, commandTimeout);
            }
        }
        
        public static IDisposable EnsureOpen(this DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            switch (connection.State)
            {
                case ConnectionState.Open:
                    return null;
                case ConnectionState.Closed:
                    connection.Open();
                    try
                    {
                        return new ConnectionCloser(connection);
                    }
                    catch
                    {
                        try { connection.Close(); }
                        catch { } // we're already trying to handle, kthxbye
                        throw;
                    }

                default:
                    throw new InvalidOperationException("Cannot use EnsureOpen when connection is " + connection.State);
            }
        }

        private class ConnectionCloser : IDisposable
        {
            DbConnection connection;
            public ConnectionCloser(DbConnection connection)
            {
                this.connection = connection;
            }
            public void Dispose()
            {
                var cn = connection;
                connection = null;
                if (cn != null)
                {
                    try { cn.Close(); }
                    catch { }//throwing from Dispose() is so lame
                }
            }
        }
    }
}
