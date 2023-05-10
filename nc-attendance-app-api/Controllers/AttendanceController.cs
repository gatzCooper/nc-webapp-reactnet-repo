using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System;
using System.Reflection;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nc_attendance_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceBusinessLayer _attendanceBusinessLayer;
        private readonly ILogger<AttendanceController> _logger;
        public AttendanceController(IAttendanceBusinessLayer attendanceBusinessLayer, ILogger<AttendanceController> logger)
        {
            _attendanceBusinessLayer = attendanceBusinessLayer;
            _logger = logger;   

        }

        // GET: api/<AttendanceController>
        [HttpGet]
        public async Task<IActionResult> GetAllAttendance()
        {
            try
            {
                var attendance = await _attendanceBusinessLayer.GetAllAttendance();
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<AttendanceController>/5
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetAttendanceByUserName(string userName)
        {
            try
            {
                var attendance = await _attendanceBusinessLayer.GetAttendancePerUser(userName);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<AttendanceController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateAttendanceById([FromBody] Attendance attendance)
        {
            try
            {
                await _attendanceBusinessLayer.UpsertAttendanceAsync(attendance);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<AttendanceController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendanceById(int id)
        {
            try
            {
                await _attendanceBusinessLayer.DeleteAttendancePerUserAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("exportAttendance")]
        public async Task<FileStreamResult> ExportAttendance() 
        {
            try
            {
                var attendanceList = await _attendanceBusinessLayer.GetAllAttendance();
                var random = new Random();
                var fileName = "Employee_Attendance_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".csv";

                using (var memoryStream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
                    {
                        // Write CSV header row
                        writer.WriteLine("attendanceId,userId,userName,firstName,lastName,date,timeIn,timeOut,totalHours,underTime,overTime");

                        // Write data rows
                        foreach (var item in attendanceList)
                        {
                            writer.WriteLine($"{item.attendanceId},{item.userId},{item.userName},{item.firstName},{item.lastName},{item.date},{item.timeIn},{item.timeOut},{item.totalHours},{item.underTime},{item.overTime}");
                        }

                        writer.Flush();

                        var byteArray = memoryStream.ToArray();

                        var fileContent = new MemoryStream(byteArray);

                        return File(fileContent, "text/csv", fileName);
                    }
                }



            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
