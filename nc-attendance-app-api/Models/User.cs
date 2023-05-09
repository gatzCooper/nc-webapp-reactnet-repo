namespace nc_attendance_app_api.Models
{
    public class User
    {
        public string userNo { get; set; }
        public string role { get; set; }
        public string employmentCode { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string mName { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public string address { get; set; }
        public string departmentName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string status { get; set; }
        public DateTime? createdAt { get; set; }
    }
}
