using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.Runtime.CompilerServices;

namespace nc_attendance_app_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserBusinessLayer _userBusinessLayer;

        public AuthController(ILogger<AuthController> logger, IUserBusinessLayer userBusinessLayer)
        {
            _logger = logger;
            _userBusinessLayer = userBusinessLayer;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            try
            {
                var userData = await _userBusinessLayer.GetUserLoginCredentials(request.username, request.password);
                if (userData == null)
                {
                    return Unauthorized();
                }

                Random random = new Random();
                int randomNumber = random.Next(50);

                var token = _userBusinessLayer.GenerateJwtToken(randomNumber, userData.email);

                var user = new
                {
                    authToken = token,
                    user = userData
                };

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

    }
}
