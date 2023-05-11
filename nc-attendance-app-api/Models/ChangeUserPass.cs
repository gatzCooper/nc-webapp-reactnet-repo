namespace nc_attendance_app_api.Models
{
    public class ChangeUserPass
    {
        public string userName { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
