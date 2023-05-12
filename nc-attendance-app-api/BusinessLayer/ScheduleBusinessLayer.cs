using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.BusinessLayer
{
    public class ScheduleBusinessLayer : IScheduleBusinessLayer
    {
        private readonly IScheduleService _scheduleService;
        public ScheduleBusinessLayer(IScheduleService scheduleService) 
        {
            _scheduleService = scheduleService;
        }
        public async Task<IList<Schedule>> GetAllScheduleAsync()
        {
            var schedules = await _scheduleService.GetAllScheduleAsync();

            return schedules;
        }

        public async Task<IList<Schedule>> GetScheduleByUsersAync(string username)
        {
            var sched = await _scheduleService.GetScheduleByUsersAync(username);

            return sched;
        }

        public async Task UpsertScheduleAsync(Schedule schedule)
        {
            await _scheduleService.UpsertScheduleAsync(schedule);
        }
        public async Task DeleteScheduleAsync(string userName)
        {
            await _scheduleService.DeleteScheduleAsync(userName);
        }
    }
}
