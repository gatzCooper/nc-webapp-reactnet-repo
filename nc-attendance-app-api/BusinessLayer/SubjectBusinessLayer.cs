using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;

namespace nc_attendance_app_api.BusinessLayer
{
    public class SubjectBusinessLayer : ISubjectBusinessLayer
    {
        private readonly ISubjectService _subjectService;

        public SubjectBusinessLayer(ISubjectService subject)
        {
            _subjectService = subject;
        }
        public async Task DeleteSubjectAsync(string subjectCode)
        {
            await _subjectService.DeleteSubjectAsync(subjectCode);
        }

        public async Task<Subject> GetSubjectBySubjectCodeAsync(string subjectCode)
        {
            var subject = await _subjectService.GetSubjectBySubjectCodeAsync(subjectCode);

            return subject;
        }

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            var subjects = await _subjectService.GetSubjectsAsync();

            return subjects;
        }

        public async Task UpsertSubjectDetailsAsync(Subject subject)
        {
            await _subjectService.UpsertSubjectDetailsAsync(subject);
        }
    }
}
