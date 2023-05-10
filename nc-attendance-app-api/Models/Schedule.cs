namespace nc_attendance_app_api.Models
{
    public class Schedule
    {
        public int scheduleId { get; set; }
        public string subjectCode { get; set; }
        public string userName { get; set; }
        public string workDay { get; set; }
        public string workingHours { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
    }
}
