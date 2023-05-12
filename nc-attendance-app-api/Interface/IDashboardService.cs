using nc_attendance_app_api.Models;
using nc_attendance_app_api.Services;

namespace nc_attendance_app_api.Interface
{
    public interface IDashboardService
    {
        Task<DashboardCounts> GetDashboardCounts();
    }
}
