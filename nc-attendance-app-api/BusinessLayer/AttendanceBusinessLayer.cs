using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.BusinessLayer
{
    public class AttendanceBusinessLayer : IAttendanceBusinessLayer
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceBusinessLayer(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        public async Task DeleteAttendancePerUserAsync(int attendanceId)
        {
            await _attendanceService.DeleteAttendancePerIdAsync(attendanceId);
        }

        public async Task<IList<Attendance>> GetAllAttendance()
        {
            var attendance = await _attendanceService.GetAllAttendance();

            return attendance;
        }

        public async Task<Attendance> GetAttendancePerUser(string userName)
        {
            var attendance = await _attendanceService.GetAttendancePerUser(userName);

            return attendance;
        }

        public async Task UpsertAttendanceAsync(Attendance attendance)
        {
            await _attendanceService.UpsertAttendanceAsync(attendance);
        }
    }
}
