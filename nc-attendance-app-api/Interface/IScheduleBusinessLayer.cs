using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.Interface
{
    public interface IScheduleBusinessLayer
    {
        Task<IList<Schedule>> GetAllScheduleAsync();
        Task<IList<Schedule>> GetScheduleByUsersAync(string username);
        Task UpsertScheduleAsync(Schedule schedule);
        Task DeleteScheduleAsync(string userName);
    }
}
