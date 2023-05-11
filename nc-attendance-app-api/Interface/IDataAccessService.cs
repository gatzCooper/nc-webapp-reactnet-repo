using System.Data;
using System.Data.SqlClient;

namespace nc_attendance_app_api.Interface
{
    public interface IDataAccessService
    {
        Task<int> ExecuteNonQueryAsync(string query, IEnumerable<SqlParameter>? parameters = null);
        Task<IDataReader> ExecuteReaderAsync(string query, IEnumerable<SqlParameter>? parameters = null);
        Task<object?> ExecuteScalarAsync(string query, IEnumerable<SqlParameter>? parameters = null);
        Task<bool> IsUserValid(string userName, string oldPassword);
    }
}
