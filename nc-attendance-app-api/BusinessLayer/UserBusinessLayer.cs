using Microsoft.IdentityModel.Tokens;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace nc_attendance_app_api.BusinessLayer
{
    public class UserBusinessLayer : IUserBusinessLayer
    {
        private readonly IUserService _userService;
        public UserBusinessLayer(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IList<User>> GetAllUserAsync()
        {
            var users = await _userService.GetAllUserAsync();

            return users;
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            var user = await _userService.GetUserByUsernameAsync(userName);

            return user;
        }

        public string GenerateJwtToken(int id, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("uV07Q~3mkk3-VVt~joAcuDoXMsRxgA5V3~mmI");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<User> GetUserLoginCredentials(string userName, string password)
        {

            byte[] passwordHash;
            byte[] passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = await _userService.GetUserCredentialByUserNameAndPasswordAsync(userName, password);

            if (!VerifyPasswordHash(password, passwordHash, passwordSalt) && user.userNo == null)
            {
                return null;
            }


            return user;
        }

        public Task<bool> ValidateUser(User user)
        {
            throw new NotImplementedException();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public async Task UpsertUserDetailsAsync(User user)
        {
            await _userService.UpsertUserDetailsAsync(user);
        }

        public async Task DeleteUserByUsernameAsync(string username)
        {
            await _userService.DeleteUserByUsernameAsync(username);
        }

        public Task<bool> IsMobileNumberValid(string mobileNumber)
        {
            throw new NotImplementedException();
        }
    }
}
