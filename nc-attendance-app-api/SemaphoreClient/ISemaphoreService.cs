using Microsoft.AspNetCore.Http;

namespace nc_attendance_app_api.SemaphoreClient
{
    public interface ISemaphoreService
    {
        Task<int> SendOtpAsync(string phoneNumber);
    }
}
