using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace nc_attendance_app_api.DAL
{
    public class DataAccessService : IDataAccessService, IDisposable
    {
        private readonly string? _connectionString;
        private readonly SqlConnection _sqlConnection;

        public DataAccessService(ConnectionStrings connectionStrings)
        {
            _connectionString = connectionStrings.DefaultConnection;
            _sqlConnection = new SqlConnection(_connectionString);

        }
        public void Dispose()
        {
            _sqlConnection.Dispose();
        }

        public async Task<int> ExecuteNonQueryAsync(string query, IEnumerable<SqlParameter>? parameters = null)
        {
            using (var command = new SqlCommand(query, _sqlConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                if (parameters != null && parameters.Any())
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                try
                {
                    _sqlConnection.Open();
                    return await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<IDataReader> ExecuteReaderAsync(string query, IEnumerable<SqlParameter>? parameters = null)
        {
            using (var command = new SqlCommand(query, _sqlConnection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                if (parameters != null && parameters.Any())
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);

                    }
                }

                try
                {
                    _sqlConnection.Open();
                    return await command.ExecuteReaderAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public Task<object?> ExecuteScalarAsync(string query, IEnumerable<SqlParameter>? parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                if (parameters != null && parameters.Any())
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }

                try
                {
                    connection.Open();
                    return command.ExecuteScalarAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
