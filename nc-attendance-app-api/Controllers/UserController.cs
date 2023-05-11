using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<IActionResult> GetAllUserAsync([FromQuery(Name = "firstName")] string? firstName = null, 
            [FromQuery(Name = "lastName")] string? lastName = null)
        {
            try
            {
                IEnumerable<User> users = await _userBusinessLayer.GetAllUserAsync();

                if (!firstName.IsNullOrEmpty())
                {
                    users = users.Where(a => a.fName == firstName);
                }

                if (!lastName.IsNullOrEmpty())
                {
                    users = users.Where(a => a.lName == lastName);
                }

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

        //[HttpPost("forgotPassword")]
        //public async Task<IActionResult> ForgotPasswordAsync(string phoneNumber)
        //{

        //}
    }
}
