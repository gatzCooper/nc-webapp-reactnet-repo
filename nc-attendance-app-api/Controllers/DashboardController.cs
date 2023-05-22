using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nc_attendance_app_api.Interface;

namespace nc_attendance_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardBusinessLayer _dashboardBusinessLayer;
        public DashboardController(IDashboardBusinessLayer dashboardBusinessLayer)
        {
            _dashboardBusinessLayer = dashboardBusinessLayer;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardCountsAsync()
        {
            try
            {
                var counts = await _dashboardBusinessLayer.GetDashboardCounts();

                return Ok(counts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
