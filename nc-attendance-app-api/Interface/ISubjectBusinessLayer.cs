using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.Interface
{
    public interface ISubjectBusinessLayer
    {
        Task<List<Subject>> GetSubjectsAsync();
        Task<Subject> GetSubjectBySubjectCodeAsync(string subjectCode);
        Task UpsertSubjectDetailsAsync(Subject subject);
        Task DeleteSubjectAsync(string subjectCode);
    }
}
