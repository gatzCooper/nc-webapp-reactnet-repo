using Microsoft.Office.Interop.Excel;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.Data.SqlClient;

namespace nc_attendance_app_api.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDataAccessService _dataAccessService;
        private const string SP_GET_DASHBOARD_COUNTS = "usp_GetDashboardCounts";

        public DashboardService(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }
        public async Task<DashboardCounts> GetDashboardCounts()
        {
            var counts = new DashboardCounts();
            using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_DASHBOARD_COUNTS))
            {
                while (await sqlDataReader.ReadAsync())
                {
                   counts.employeeCount  = Convert.ToInt32(sqlDataReader["employeeCount"]);
                   counts.departmentCount = Convert.ToInt32(sqlDataReader["departmentCount"]);
                }

                return counts;
            }
          
        }
    }
}
