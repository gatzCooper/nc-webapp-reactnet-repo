using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.Data.SqlClient;

namespace nc_attendance_app_api.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IDataAccessService _dataAccessService;
        private const string SP_GET_ALL_ATTENDANCE = "usp_GetAllAttendance";
        private const string SP_GET_ALL_ATTENDANCE_BY_USERNAME = "usp_GetAllAttendanceByUsername";
        private const string SP_UPDATE_ATTENDANCE = "usp_UpdateAttendance";
        private const string SP_DELETE_ATTENDANCE = "usp_DeleteAttendance";
        public AttendanceService(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }
        public async Task DeleteAttendancePerIdAsync(int attendanceId)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@attendanceId", attendanceId)
            };

            await _dataAccessService.ExecuteNonQueryAsync(SP_DELETE_ATTENDANCE, parameters);
        }

        public async Task<IList<Attendance>> GetAllAttendance()
        {
            var attendanceList = new List<Attendance>();

            try
            {
                using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_ALL_ATTENDANCE))
                {
                    while(await sqlDataReader.ReadAsync())
                    {
                        var attendance = new Attendance();

                        attendance.attendanceId = Convert.ToInt32(sqlDataReader["attendanceId"]);
                        attendance.userId = Convert.ToInt32(sqlDataReader["userId"]);
                        attendance.userName = Convert.ToString(sqlDataReader["userName"]) ?? "";
                        attendance.firstName = Convert.ToString(sqlDataReader["firstName"]) ?? "";
                        attendance.lastName = Convert.ToString(sqlDataReader["lastName"]) ?? "";
                        attendance.date = sqlDataReader["date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(sqlDataReader["date"]);
                        attendance.timeIn = Convert.ToString(sqlDataReader["timeIn"]) ?? "";
                        attendance.timeOut = Convert.ToString(sqlDataReader["timeOut"]) ?? "";
                        attendance.totalHours = Convert.ToInt32(sqlDataReader["totalHours"]);
                        attendance.underTime = Convert.ToInt32(sqlDataReader["underTime"]);
                        attendance.overTime = Convert.ToInt32(sqlDataReader["overTime"]);

                        attendanceList.Add(attendance);
                    }

                    return attendanceList;
                }
            }
            catch (Exception err)
            {
                throw;
            }

        }

        public async Task<Attendance> GetAttendancePerUser(string userName)
        {

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@userName", userName)
            };

            try
            {
                var attendance = new Attendance();

                using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_ALL_ATTENDANCE_BY_USERNAME,parameters))
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        
                        attendance.attendanceId = Convert.ToInt32(sqlDataReader["attendanceId"]);
                        attendance.userId = Convert.ToInt32(sqlDataReader["userId"]);
                        attendance.userName = Convert.ToString(sqlDataReader["userName"]) ?? "";
                        attendance.firstName = Convert.ToString(sqlDataReader["firstName"]) ?? "";
                        attendance.lastName = Convert.ToString(sqlDataReader["lastName"]) ?? "";
                        attendance.date = sqlDataReader["date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(sqlDataReader["date"]);
                        attendance.timeIn = Convert.ToString(sqlDataReader["timeIn"]) ?? "";
                        attendance.timeOut = Convert.ToString(sqlDataReader["timeOut"]) ?? "";
                        attendance.totalHours = Convert.ToInt32(sqlDataReader["totalHours"]);
                        attendance.underTime = Convert.ToInt32(sqlDataReader["underTime"]);
                        attendance.overTime = Convert.ToInt32(sqlDataReader["overTime"]);
                    }

                    return attendance;
                }
            }
            catch (Exception err)
            {
                throw;
            }

        }

        public Task UpsertAttendanceAsync(Attendance attendance)
        {
            throw new NotImplementedException();
        }
    }
}
