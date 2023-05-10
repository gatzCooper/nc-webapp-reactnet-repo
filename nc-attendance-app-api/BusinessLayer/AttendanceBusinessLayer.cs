using Microsoft.Office.Interop.Excel;
using nc_attendance_app_api.Interface;
using nc_attendance_app_api.Models;
using System.Reflection;

namespace nc_attendance_app_api.BusinessLayer
{
    public class AttendanceBusinessLayer : IAttendanceBusinessLayer
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceBusinessLayer(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        public async Task DeleteAttendancePerUserAsync(int attendanceId)
        {
            await _attendanceService.DeleteAttendancePerIdAsync(attendanceId);
        }

        public async Task<IList<Attendance>> GetAllAttendance()
        {
            var attendance = await _attendanceService.GetAllAttendance();

            return attendance;
        }

        public async Task<Attendance> GetAttendancePerUser(string userName)
        {
            var attendance = await _attendanceService.GetAttendancePerUser(userName);

            return attendance;
        }

        public async Task UpsertAttendanceAsync(Attendance attendance)
        {
            await _attendanceService.UpsertAttendanceAsync(attendance);
        }

        public void  ExportAttendanceToExcel(IList<Attendance> attendanceList, string fileName)
        {
            // Create a new Excel Application
            Application excelApp = new Application();

            // Create a new workbook
            Workbook workbook = excelApp.Workbooks.Add();

            // Get the active worksheet
            Worksheet worksheet = (Worksheet)workbook.ActiveSheet;

            // Write the column headers
            int row = 1;
            int col = 1;
            PropertyInfo[] properties = typeof(Attendance).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                worksheet.Cells[row, col] = property.Name;
                col++;
            }

            // Write the attendance data rows
            row = 2;
            foreach (Attendance attendance in attendanceList)
            {
                col = 1;
                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(attendance);
                    worksheet.Cells[row, col] = value != null ? value.ToString() : "";
                    col++;
                }
                row++;
            }

            // Save the workbook
            workbook.SaveAs(fileName);

            // Close the workbook and Excel application
            workbook.Close();
            excelApp.Quit();
        }
    }
}
