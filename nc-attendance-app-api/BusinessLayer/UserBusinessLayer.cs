using Microsoft.IdentityModel.Tokens;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
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

        public async Task UpdateOldPassword(string userName, string oldPassword, string newPassword)
        {
            byte[] byteNewPass = Encoding.UTF8.GetBytes(newPassword);
            string base64StringNewPass = Convert.ToBase64String(byteNewPass);

            byte[] byteOldPass = Encoding.UTF8.GetBytes(oldPassword);
            string base64StringOldPass = Convert.ToBase64String(byteOldPass);

            await _userService.UpdateOldPassword(userName, base64StringOldPass, base64StringNewPass);         
        }

        public async Task<bool> IsUserValid(string userName, string oldPassword)
        {
            return await _userService.IsUserValid(userName, oldPassword);
        }

       
        public async Task SendEmailToUser(string userName)
        {
            try
             {
                var userDetails = await RetievePassword(userName);

              
                // Set up the SMTP client for Gmail
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("user.ncsmtp@gmail.com", "NCAdmin2023");
                smtpClient.EnableSsl = true;

                // Create the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(userDetails.email);
                mailMessage.To.Add(userDetails.email);
                mailMessage.Subject = "Retrieve password";
                mailMessage.Body = $"Hi good day!, here is your password: {userDetails.password}. Please change password as soon as possible.Thanks! \n -Admin \n" +
                    $"This is system generated please do not reply.";

                // Send the email
                smtpClient.Send(mailMessage);
            }
            catch(Exception ex)
            {
                throw;
            }     

        }
        private async Task<UserRetrieve> RetievePassword(string userName)
        {
            var user = await _userService.RetrievePasswordAsync(userName);

            string base64Pass = user.password;
             byte[] bytes = Convert.FromBase64String(base64Pass);

            string stringPass = Encoding.UTF8.GetString(bytes);

            var userDetails = new UserRetrieve()
            {
                email = user.email,
                password = stringPass
            };

            return userDetails;
        }

        public async Task BulkUserUpload(User user)
        {
            await _userService.BulkUserUpload(user);
        }
    }
}
