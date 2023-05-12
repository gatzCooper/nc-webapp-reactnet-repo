using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.Interface
{
    public interface IDashboardBusinessLayer
    {
        Task<DashboardCounts> GetDashboardCounts();
    }
}
