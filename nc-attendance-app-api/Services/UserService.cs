using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace nc_attendance_app_api.Services
{
    public class UserService :  IUserService
    {
        private readonly IDataAccessService _dataAccessService;
        private const string SP_GET_ALL_USER = "dbo.usp_GetAllUser";
        private const string SP_GET_USER_BY_USERNAME = "usp_GetUserInfoByUserName";
        private const string SP_GET_USER_BY_USERNAME_AND_PASSWORD = "usp_GetUserInfoByUserNameAndPassword";
        private const string SP_UPSERT_USER_DETAILS = "usp_UpsertUserDetails";
        private const string SP_DELETE_USER_DETAILS = "usp_DeleteUserByUserName";
        private const string SP_VALIDATE_MOBILE_NUMBER = "usp_ValidatePhoneNumber";
        private const string SP_CHANGE_USER_PASSWORD = "usp_ChangeUserPassword";
        public UserService(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }

        public async Task<IList<User>> GetAllUserAsync()
        {
            var userList = new List<User>();
            


            try
            {
                using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_ALL_USER))
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        var user = new User();
                        user.userNo = Convert.ToString(sqlDataReader["userNumber"]) ?? "";
                        user.employmentCode = Convert.ToString(sqlDataReader["employmentCode"]) ?? "";
                        user.empDescription = Convert.ToString(sqlDataReader["empDescription"]) ?? "";
                        user.fName = Convert.ToString(sqlDataReader["firstName"]) ?? "";
                        user.lName = Convert.ToString(sqlDataReader["lastName"]) ?? "";
                        user.mName = Convert.ToString(sqlDataReader["middleName"]) ?? "";
                        user.email = Convert.ToString(sqlDataReader["emailAddress"]) ?? "";
                        user.contact = Convert.ToString(sqlDataReader["mobileNumber"]) ?? "";
                        user.address = Convert.ToString(sqlDataReader["homeAddress"]) ?? "";
                        user.departmentName = Convert.ToString(sqlDataReader["departmentName"]) ?? "";
                        user.username = Convert.ToString(sqlDataReader["userName"]) ?? "";
                        user.status = Convert.ToString(sqlDataReader["statusName"]) ?? "";
                        user.hiredDate = sqlDataReader["hiredDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(sqlDataReader["hiredDate"]);
                        user.createdAt = sqlDataReader["createdAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(sqlDataReader["createdAt"]);

                        userList.Add(user);

                    }
                    return userList;
                }
            }
            catch (Exception err)
            {
                throw;
            }

        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {

            var user = new User();

            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@userName", userName)
             };
            try
            {
                using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_USER_BY_USERNAME, parameters))
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        user.userNo = Convert.ToString(sqlDataReader["userNumber"]) ?? "";
                        user.employmentCode = Convert.ToString(sqlDataReader["employmentCode"]) ?? "";
                        user.empDescription = Convert.ToString(sqlDataReader["empDescription"]) ?? "";
                        user.fName = Convert.ToString(sqlDataReader["firstName"]) ?? "";
                        user.lName = Convert.ToString(sqlDataReader["lastName"]) ?? "";
                        user.mName = Convert.ToString(sqlDataReader["middleName"]) ?? "";
                        user.email = Convert.ToString(sqlDataReader["emailAddress"]) ?? "";
                        user.contact = Convert.ToString(sqlDataReader["mobileNumber"]) ?? "";
                        user.address = Convert.ToString(sqlDataReader["homeAddress"]) ?? "";
                        user.departmentName = Convert.ToString(sqlDataReader["departmentName"]) ?? "";
                        user.username = Convert.ToString(sqlDataReader["userName"]) ?? "";
                        user.status = Convert.ToString(sqlDataReader["statusName"]) ?? "";
                        user.hiredDate = sqlDataReader["hiredDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(sqlDataReader["hiredDate"]);
                        user.createdAt = sqlDataReader["createdAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(sqlDataReader["createdAt"]);

                    }
                    return user;
                }
            }
            catch (Exception err)
            {
                throw;
            }

        }

        public async Task<User> GetUserCredentialByUserNameAndPasswordAsync(string userName, string password)
        {
            byte[] bytePass = Encoding.UTF8.GetBytes(password);
            string base64String = Convert.ToBase64String(bytePass);
            var user = new User();

            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@userName", userName),
                new SqlParameter("@password", base64String)
             };

            try
            {
                using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_USER_BY_USERNAME_AND_PASSWORD, parameters))
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        user.userNo = Convert.ToString(sqlDataReader["userNumber"]) ?? "";
                        user.employmentCode = Convert.ToString(sqlDataReader["employmentCode"]) ?? "";
                        user.empDescription = Convert.ToString(sqlDataReader["empDescription"]) ?? "";
                        user.fName = Convert.ToString(sqlDataReader["firstName"]) ?? "";
                        user.lName = Convert.ToString(sqlDataReader["lastName"]) ?? "";
                        user.mName = Convert.ToString(sqlDataReader["middleName"]) ?? "";
                        user.email = Convert.ToString(sqlDataReader["emailAddress"]) ?? "";
                        user.contact = Convert.ToString(sqlDataReader["mobileNumber"]) ?? "";
                        user.address = Convert.ToString(sqlDataReader["homeAddress"]) ?? "";
                        user.departmentName = Convert.ToString(sqlDataReader["departmentName"]) ?? "";
                        user.username = Convert.ToString(sqlDataReader["userName"]) ?? "";
                        user.status = Convert.ToString(sqlDataReader["statusName"]) ?? "";
                        user.hiredDate = sqlDataReader["hiredDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(sqlDataReader["hiredDate"]);
                        user.createdAt = sqlDataReader["createdAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(sqlDataReader["createdAt"]);

                    }

                    return user;
                }
            }
            catch (Exception err)
            {
                throw;
            }

        }

        public async Task UpsertUserDetailsAsync(User user)
        {

            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@userNumber", user.userNo),
                new SqlParameter("@employmentCode", user.employmentCode),
                new SqlParameter("@firstName", user.fName),
                new SqlParameter("@lastName", user.lName),
                new SqlParameter("@middleName", user.mName),
                new SqlParameter("@contact", user.contact),
                new SqlParameter("@email", user.email),
                new SqlParameter("@userName", user.username),
                new SqlParameter("@status", user.status),
                new SqlParameter("@department", user.departmentName),
                new SqlParameter("@address", user.address),
                new SqlParameter("@hiredDate", user.hiredDate)
           };

            await _dataAccessService.ExecuteNonQueryAsync(SP_UPSERT_USER_DETAILS, parameters);
        }

        public async Task DeleteUserByUsernameAsync(string username)
        { 
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@userName", username)
            };

            await _dataAccessService.ExecuteNonQueryAsync(SP_DELETE_USER_DETAILS, parameters);
        }

        public async Task<bool> ValidateMobileNumberAsync(string mobileNumber)
        {
            bool isValid = false;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@mobileNumber", mobileNumber),
            };
            using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_VALIDATE_MOBILE_NUMBER, parameters))
            {
                while (await sqlDataReader.ReadAsync())
                {
                    isValid = Convert.ToBoolean(sqlDataReader["IsValid"]);

                }          
            }
            return isValid;

        }

        public async Task<bool> IsUserValid(string userName, string oldPassword)
        {

            return await _dataAccessService.IsUserValid(userName, oldPassword);

        }

        public async Task UpdateOldPassword(string userName, string oldPassword, string newPassword)
        {

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@userName", userName),
                    new SqlParameter("@oldPassword", oldPassword),
                    new SqlParameter("@newPassword", newPassword)
                };
      

                 await _dataAccessService.ExecuteNonQueryAsync(SP_CHANGE_USER_PASSWORD, parameters);
        }

       
    }
}
