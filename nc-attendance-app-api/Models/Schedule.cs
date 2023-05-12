namespace nc_attendance_app_api.Models
{
    public class Schedule
    {
        public int scheduleId { get; set; }
        public string subjectCode { get; set; }
        public string userName { get; set; }
        public string workDay { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string workingHours => CalculateWorkingHours();
        private string CalculateWorkingHours()
        {
            // Add code to calculate the working hours based on startTime and endTime
            // For example:
            var startTime = DateTime.Parse(this.startTime);
            var endTime = DateTime.Parse(this.endTime);
            var workingHours = (endTime - startTime).TotalHours.ToString("0.##");
            return workingHours;
        }
    }
}
