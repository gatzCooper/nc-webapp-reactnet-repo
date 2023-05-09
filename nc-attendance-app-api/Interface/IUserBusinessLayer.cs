using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.Interface
{
    public interface IUserBusinessLayer
    {
        Task<IList<User>> GetAllUserAsync();
        Task<User> GetUserByUsernameAsync(string userName);

        string GenerateJwtToken(int id, string userName);
        Task<bool> ValidateUser(User user);
        Task<User> GetUserLoginCredentials(string userName, string password);
        Task UpsertUserDetailsAsync(User user);
        Task DeleteUserByUsernameAsync(string username);
    }
}
