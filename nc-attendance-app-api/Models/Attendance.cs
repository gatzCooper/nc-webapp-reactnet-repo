﻿namespace nc_attendance_app_api.Models
{
    public class Attendance
    {
        public int attendanceId { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public string userNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime? date { get; set; }
        public string timeIn { get; set; }
        public string timeOut { get; set; }
        public int? totalHours { get; set; }
        public int? underTime { get; set; }
        public int? overTime { get; set; }
    }
}
