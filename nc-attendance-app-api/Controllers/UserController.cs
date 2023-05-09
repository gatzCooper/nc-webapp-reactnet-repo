using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserBusinessLayer _userBusinessLayer;

        public UserController(ILogger<UserController> logger, IUserBusinessLayer userBusinessLayer)
        {
            _logger = logger;
            _userBusinessLayer = userBusinessLayer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync()
        {
            try
            {
                var users = await _userBusinessLayer.GetAllUserAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsernameAsync(string userName)
        {
            try
            {
                var user = await _userBusinessLayer.GetUserByUsernameAsync(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpsertUserDetailsAsync([FromBody] User request)
        {
            try
            {
                await _userBusinessLayer.UpsertUserDetailsAsync(request);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("username")]
        public async Task<IActionResult> DeleteUserDetailsByUserNameAsync(string username)
        {
            try
            {
                await _userBusinessLayer.DeleteUserByUsernameAsync(username);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
