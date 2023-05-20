using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
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
                    await _sqlConnection.OpenAsync();
                    var result = await command.ExecuteNonQueryAsync();

                    return result;
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
                    await _sqlConnection.OpenAsync();
                    var result = await command.ExecuteReaderAsync();

                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task<object?> ExecuteScalarAsync(string query, IEnumerable<SqlParameter>? parameters = null)
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
                    await _sqlConnection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
            

        public async Task<bool> IsUserValid(string userName, string oldPassword)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM tbl_User WHERE userName = @userName AND Password = @oldPassword";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userName", userName);
                    command.Parameters.AddWithValue("@oldPassword", oldPassword);

                     await connection.OpenAsync();
                    int count =  (int)await command.ExecuteScalarAsync();
                    await connection.CloseAsync();
                    await connection.DisposeAsync();

                    return count > 0;
                }
            }
        }

        public async Task BulkUserUpload(User user)
        {

            // Save the records to the database
            using (var connection = new SqlConnection(_connectionString)) // Replace with your database connection string
            {
                await connection.OpenAsync();

                    using (var command = new SqlCommand("dbo.usp_UpsertUserDetails", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@userNumber", user.userNo);
                        command.Parameters.AddWithValue("@employmentCode", user.employmentCode);
                        command.Parameters.AddWithValue("@firstName", user.fName);
                        command.Parameters.AddWithValue("@lastName", user.lName);
                        command.Parameters.AddWithValue("@middleName", user.mName);
                        command.Parameters.AddWithValue("@contact", user.contact);
                        command.Parameters.AddWithValue("@email", user.email);
                        command.Parameters.AddWithValue("@userName", user.username);
                        command.Parameters.AddWithValue("@status", user.status);
                        command.Parameters.AddWithValue("@department", user.departmentName);
                        command.Parameters.AddWithValue("@address", user.address);
                        command.Parameters.AddWithValue("@hiredDate", user.hiredDate ?? (object)DBNull.Value);

                        await command.ExecuteNonQueryAsync();
                    }
                
            }
        }
    }
}
