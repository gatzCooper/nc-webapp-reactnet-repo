using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.Interface
{
    public interface IAttendanceService
    {
        Task<IList<Attendance>> GetAllAttendance();
        Task<Attendance> GetAttendancePerUser(string userName);
        Task UpsertAttendanceAsync(Attendance attendance);
        Task DeleteAttendancePerIdAsync(int attendanceId);
    }
}
