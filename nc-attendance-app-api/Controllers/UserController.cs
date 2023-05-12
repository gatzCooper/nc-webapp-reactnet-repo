using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.Text;

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
        public async Task<IActionResult> GetAllUserAsync([FromQuery(Name = "name")] string? name = null)
        {
            try
            {
                IEnumerable<User> users = await _userBusinessLayer.GetAllUserAsync();

                if (!name.IsNullOrEmpty())
                {
                    users = users.Where(a => a.fName.Contains(name) || a.lName.Contains(name));
                }


                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userName}")]
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
        public async Task<IActionResult> UpsertUserDetailsAsync([FromBody] UserRequest request)
        {
            try
            {
                var user = new User()
                {
                    userNo = request.userNo,
                    employmentCode = request.employmentCode,
                    fName = request.fName,
                    lName = request.lName,
                    mName = request.mName,
                    email = request.email,
                    contact = request.contact,
                    address = request.address,
                    departmentName = request.departmentName,
                    username = request.username,
                    status = request.status,
                    hiredDate = request.hiredDate  
                    
                 };
                
                await _userBusinessLayer.UpsertUserDetailsAsync(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteUserDetailsByUserNameAsync(string userName)
        {
            try
            {
                await _userBusinessLayer.DeleteUserByUsernameAsync(userName);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangeUserPass request)
            {
            try
            {
                byte[] byteOldPass = Encoding.UTF8.GetBytes(request.oldPassword);
                string base64String = Convert.ToBase64String(byteOldPass);


                if (!await _userBusinessLayer.IsUserValid(request.userName, base64String))
                {
                    return Unauthorized("Access Denied");
                }

                await _userBusinessLayer.UpdateOldPassword(request.userName,request.oldPassword, request.newPassword);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
