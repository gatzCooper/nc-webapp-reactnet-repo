using Microsoft.AspNetCore.Mvc;
using nc_attendance_app_api.BusinessLayer;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nc_attendance_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleBusinessLayer _schedulebusinessLayer;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleBusinessLayer scheduleBusinessLayer)
        {
            _schedulebusinessLayer = scheduleBusinessLayer;
            _logger = logger;
        }


        // GET: api/<ScheduleController>
        [HttpGet]
        public async Task<IActionResult> GetAllScheduleAsync()
        {
            try
            {
                var schedules = await _schedulebusinessLayer.GetAllScheduleAsync();
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ScheduleController>/5
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetAllScheduleAsyncByUsernameAsync(string userName)
        {
            try
            {
                var sched = await _schedulebusinessLayer.GetScheduleByUsersAync(userName);
                return Ok(sched);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ScheduleController>/5
        [HttpPut]
        public async Task<IActionResult> UpsertUserScheduleAsync([FromBody] Schedule schedule)
        {
            try
            {
                await _schedulebusinessLayer.UpsertScheduleAsync(schedule);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ScheduleController>/5
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteScheduleAsync(string username)
        {
            try
            {
                await _schedulebusinessLayer.DeleteScheduleAsync(username);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
