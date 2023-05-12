using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.Data.SqlClient;

namespace nc_attendance_app_api.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IDataAccessService _dataAccessService;
        private const string SP_GET_ALL_SCHEDULE = "usp_GetAllSchedule";
        private const string SP_GET_SCHEDULE_BY_USERNAME = "usp_GetAllScheduleByUsername";
        private const string SP_UPSERT_USER_SCHEDULE = "usp_UpsertUserSchedule";
        private const string SP_DELETE_USER_SCHEDULE = "usp_DeleteScheduleByUserName";

        public ScheduleService(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }
       
        public async Task<IList<Schedule>> GetAllScheduleAsync()
        {
            var schedules = new List<Schedule>();
            

            try
            {
                using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_ALL_SCHEDULE))
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        var sched = new Schedule();
                        sched.scheduleId = Convert.ToInt32(sqlDataReader["scheduleId"]);
                        sched.subjectCode = Convert.ToString(sqlDataReader["subjectCode"]) ?? "";
                        sched.userName = Convert.ToString(sqlDataReader["userName"]) ?? "";
                        sched.workDay = Convert.ToString(sqlDataReader["workDay"]) ?? "";
                        sched.workingHours = Convert.ToString(sqlDataReader["workingHours"]) ?? "";
                        sched.startTime = Convert.ToString(sqlDataReader["startTime"]) ?? "";
                        sched.endTime = Convert.ToString(sqlDataReader["endTime"]) ?? "";

                        schedules.Add(sched);

                    }
                    return schedules;
                }
            }
            catch (Exception err)
            {
                throw;
            }
        }

        public async Task<IList<Schedule>> GetScheduleByUsersAync(string username)
        {
            var schedList = new List<Schedule> ();

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@userName", username)
            };

            try
            {
                using (SqlDataReader sqlDataReader = (SqlDataReader)await _dataAccessService.ExecuteReaderAsync(SP_GET_SCHEDULE_BY_USERNAME, parameters))
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        var sched = new Schedule();
                        sched.scheduleId = Convert.ToInt32(sqlDataReader["scheduleId"]);
                        sched.subjectCode = Convert.ToString(sqlDataReader["subjectCode"]) ?? "";
                        sched.userName = Convert.ToString(sqlDataReader["userName"]) ?? "";
                        sched.workDay = Convert.ToString(sqlDataReader["workDay"]) ?? "";
                        sched.workingHours = Convert.ToString(sqlDataReader["workingHours"]) ?? "";
                        sched.startTime = Convert.ToString(sqlDataReader["startTime"]) ?? "";
                        sched.endTime = Convert.ToString(sqlDataReader["endTime"]) ?? "";
                        schedList.Add(sched);

                    }
                    return schedList;
                }
            }
            catch (Exception err)
            {
                throw;
            }


        }
        public async Task UpsertScheduleAsync(Schedule schedule)
        {
             SqlParameter[] parameters = new SqlParameter[]
             {
                    new SqlParameter("@scheduleId", schedule.scheduleId),
                    new SqlParameter("@subjectCode", schedule.subjectCode),
                    new SqlParameter("@userName", schedule.userName),
                    new SqlParameter("@workDay", schedule.workDay),
                    new SqlParameter("@startTime", schedule.startTime),
                    new SqlParameter("@endTime", schedule.endTime)
             };

            await _dataAccessService.ExecuteNonQueryAsync(SP_UPSERT_USER_SCHEDULE, parameters);
        }

        public async Task DeleteScheduleAsync(string username)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@userName", username)
            };

            await _dataAccessService.ExecuteNonQueryAsync(SP_DELETE_USER_SCHEDULE, parameters);
        }
    }
}
