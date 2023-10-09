using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Microsoft.Win32;
using EmployeeManager.Models;

namespace EmployeeManager.Helpers
{
    public static class CsvExportHelper
    {
        //TODO: The ExportToCsv method is not generic. It should be refactored.
        public static void ExportToCsv(ObservableCollection<Employee> employees)
        {
            var csvContent = new StringBuilder();

            csvContent.AppendLine("Id,Name,Email,Gender,Status");

            foreach (var employee in employees)
            {
                csvContent.AppendLine($"{employee.Id},{employee.Name},{employee.Email},{employee.Gender},{employee.Status}");
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                DefaultExt = ".csv",
                FileName = "employees.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;

                File.WriteAllText(filePath, csvContent.ToString());
            }
        }
    }
}
