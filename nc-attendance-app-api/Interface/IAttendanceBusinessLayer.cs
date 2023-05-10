﻿using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.Interface
{
    public interface IAttendanceBusinessLayer
    {
        Task<IList<Attendance>> GetAllAttendance();
        Task<Attendance> GetAttendancePerUser(string userName);
        Task UpsertAttendanceAsync(Attendance attendance);

        Task DeleteAttendancePerUserAsync(int attendanceId);
    }
}
