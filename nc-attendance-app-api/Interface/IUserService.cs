using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.Interface
{
    public interface IUserService
    {
        Task<IList<User>> GetAllUserAsync();
        Task<User> GetUserByUsernameAsync(string userName);
        Task<User> GetUserCredentialByUserNameAndPasswordAsync(string userName, string password);

        Task UpsertUserDetailsAsync(User user);

        Task DeleteUserByUsernameAsync(string username);
        Task<bool> ValidateMobileNumberAsync(string mobileNumber);
        Task UpdateOldPassword(string userName, string oldPassword, string newPassword);
    }
}
