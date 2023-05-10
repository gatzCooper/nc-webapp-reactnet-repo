using Microsoft.AspNetCore.Mvc;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace nc_attendance_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ILogger<SubjectController> _logger;
        private readonly ISubjectBusinessLayer _subjectBusinessLayer;

        public SubjectController(ILogger<SubjectController> logger, ISubjectBusinessLayer subjectBusinessLayer)
        {
            _logger = logger;
            _subjectBusinessLayer = subjectBusinessLayer;
        }

        // GET: api/<SubjectController>
        [HttpGet]
        public async Task<IActionResult> GetAllSubject()
        {
            try
            {
                var subject = await _subjectBusinessLayer.GetSubjectsAsync();
                return Ok(subject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<SubjectController>/5
        [HttpGet("{subjectCode}")]
        public async Task<IActionResult> GetSubjectBySubjectCode(string subjectCode)
        {
            try
            {
                var subject = await _subjectBusinessLayer.GetSubjectBySubjectCodeAsync(subjectCode);
                return Ok(subject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT api/<SubjectController>/5
        [HttpPut]
        public async Task<IActionResult> UpsertSubject([FromBody] Subject subject)
        {
            try
            {
                await _subjectBusinessLayer.UpsertSubjectDetailsAsync(subject);
                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<SubjectController>/5
        [HttpDelete("{subjectCode}")]
        public async Task<IActionResult> DeleteSubject(string subjectCode)
        {
            try
            {
                await _subjectBusinessLayer.DeleteSubjectAsync(subjectCode);
                return Ok();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
