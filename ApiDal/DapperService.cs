using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Dal
{
    public class DapperService
    {
        public string _connectionString;

        public DapperService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<TResponse?> SingleQueryAsync<TRequest, TResponse>(string procedure, TRequest parameters)
        {
            using var db = CreateConnection();
            var result = await db.QueryFirstOrDefaultAsync<TResponse>(
                procedure,
                parameters,
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<TResponse?> SingleQueryAsync<TResponse>(string procedure)
        {
            using var db = CreateConnection();
            var result = await db.QueryFirstOrDefaultAsync<TResponse>(
                procedure,
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> ExecuteQueryAsync(string procedure)
        {
            using var db = CreateConnection();
            try
            {
                return await db.ExecuteAsync(procedure, commandType: CommandType.StoredProcedure, commandTimeout: 1200);
            }
            catch (Exception)
            {
                // Aquí puedes agregar logging si usas ILogger
                return 0;
            }
        }

        public async Task<List<TResponse>> ListQueryAsync<TRequest, TResponse>(string procedure, TRequest parameters)
        {
            using var db = CreateConnection();
            var result = await db.QueryAsync<TResponse>(
                procedure,
                parameters,
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<TResponse>> ListQueryAsync<TResponse>(string procedure)
        {
            using var db = CreateConnection();
            var result = await db.QueryAsync<TResponse>(
                procedure,
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task ExecuteQueryAsync<T>(string procedure, T parameters)
        {
            using var db = CreateConnection();
            await db.ExecuteAsync(procedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
