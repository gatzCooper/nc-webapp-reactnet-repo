using Microsoft.IdentityModel.Tokens;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace nc_attendance_app_api.BusinessLayer
{
    public class UserBusinessLayer : IUserBusinessLayer
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private const string gmailSmtp = "smtp.gmail.com";
        private const int gmailPort = 587;
        public UserBusinessLayer(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
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
            string emailUsername = _configuration.GetSection("SmtpCredentials")["UserName"];
            string emailPassword = _configuration.GetSection("SmtpCredentials")["Password"];
            var userDetails = await RetievePassword(userName);

            var email = new MimeMessage();

            StringBuilder sb = new();
            sb.AppendLine($"Dear {userDetails.firstName},");
            sb.AppendLine($"<p>Your current password is: <b>{userDetails.password}</b>. Kindly update your password immediately. </p>");
            sb.AppendLine("<p>- NC Admin</p>");
            sb.AppendLine($"<p><i>This email is auto-generated. Please do not respond.</i></p>");

            email.From.Add(new MailboxAddress("NC Mailer", emailUsername));
            email.To.Add(new MailboxAddress("Receiver Name", userDetails.email));

            email.Subject = "Password Recovery";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = sb.ToString()
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(gmailSmtp, gmailPort, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate(emailUsername, emailPassword);

                smtp.Send(email);
                smtp.Disconnect(true);
            }


        }
        catch (Exception ex)
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
                firstName = user.firstName,
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
