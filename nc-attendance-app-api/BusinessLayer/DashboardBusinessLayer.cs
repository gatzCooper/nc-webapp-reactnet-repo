using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using nc_attendance_app_api.Services;

namespace nc_attendance_app_api.BusinessLayer
{
    public class DashboardBusinessLayer : IDashboardBusinessLayer
    {
        private readonly IDashboardService _dashboardService;
        public DashboardBusinessLayer(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public async Task<DashboardCounts> GetDashboardCounts()
        {
            var counts = await _dashboardService.GetDashboardCounts();

            return counts;
        }

    }
}
