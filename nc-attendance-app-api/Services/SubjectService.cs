using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.Data.SqlClient;

namespace nc_attendance_app_api.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IDataAccessService _dataAccessService;
        private const string SP_GET_ALL_SUBJECTS = "usp_GetAllSubjects";
        private const string SP_GET_ALL_SUBJECTS_BY_CODE = "usp_GetSubjectBySubjectCode";
        private const string SP_UPSERT_SUBJECTS = "usp_UpsertSubject";
        private const string SP_DELETE_SUBJECT = "usp_DeleteSubjectByCode";

        public SubjectService(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }
        public async Task DeleteSubjectAsync(string subjectCode)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@subjectCode", subjectCode)
            };
            await _dataAccessService.ExecuteNonQueryAsync(SP_DELETE_SUBJECT, parameters);
        }

        public async Task<Subject> GetSubjectBySubjectCodeAsync(string subjectCode)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@subjectCode", subjectCode)
            };
            var subject = new Subject();

            try
            {
                using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_ALL_SUBJECTS_BY_CODE, parameters))
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        subject.subjectCode = Convert.ToString(sqlDataReader["subjectCode"]) ?? "";
                        subject.description = Convert.ToString(sqlDataReader["description"]) ?? "";
                    }
                    return subject;
                }
            }
            catch (Exception err)
            {
                throw;
            }
        }

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            var subjects = new List<Subject>();
            

            try
            {
                using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_ALL_SUBJECTS))
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        var subject = new Subject();
                        subject.subjectCode = Convert.ToString(sqlDataReader["subjectCode"]) ?? "";
                        subject.description = Convert.ToString(sqlDataReader["description"]) ?? "";
                        subjects.Add(subject);
                    }

                    return subjects;
                }
            }
            catch (Exception err)
            {
                throw;
            }
        }

        public async Task UpsertSubjectDetailsAsync(Subject subject)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@subjectCode", subject.subjectCode),
                new SqlParameter("@description", subject.description)
            };

            await _dataAccessService.ExecuteNonQueryAsync(SP_UPSERT_SUBJECTS, parameters);
        }
    }
}
