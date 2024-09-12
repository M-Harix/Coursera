using System.Globalization;
using Coursera.Data;
using Coursera.Models;
using Microsoft.EntityFrameworkCore;
using Coursera;

internal class Program
{
    private static void Main(string[] args)
    {
        TakeInputs();
    }

    static void TakeInputs()
    {
        Console.WriteLine("Enter the Comma separated list of personal identifiers (PIN) of the students to be included in the report OR press Enter to select all students:");
        string std_pin_Input = Console.ReadLine();
        string[] std_pin_Input_List = String.IsNullOrWhiteSpace(std_pin_Input) ? new string[0] : std_pin_Input.Split(',');

    credit:
        Console.WriteLine("Enter required minimum credit:");
        string minimumCredit = Console.ReadLine();
        if (!int.TryParse(minimumCredit, out int minimum_Credit))
        {
            Console.WriteLine("Credit is required and must be a number.");
            goto credit;
        }

    startdate:
        Console.WriteLine("Enter the start date of the time period for which the students should have collected the requested credit (yyyy-mm-dd):");
        string startdate = Console.ReadLine();
        if (!DateTime.TryParseExact(startdate, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start_date))
        {
            Console.WriteLine("Invalid date format. Please enter the date in the correct format (yyyy-mm-dd).");
            goto startdate;
        }

    enddate:
        Console.WriteLine("Enter the end date of the time period for which the students should have collected the requested credit (yyyy-mm-dd):");
        string enddate = Console.ReadLine();
        if (!DateTime.TryParseExact(enddate, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end_date))
        {
            Console.WriteLine("Invalid date format. Please enter the date in the correct format (yyyy-mm-dd).");
            goto enddate;
        }

        Console.WriteLine("Enter the output format (csv or html) OR press Enter for both:");
        string format = Console.ReadLine();

    path:
        Console.WriteLine("Enter the path to save result (e.g., D://New Folder//):");
        string path = Console.ReadLine();
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("Path is required.");
            goto path;
        }

        GetStudentsReport(std_pin_Input_List, minimum_Credit, start_date, end_date, path, format);
    }

    public static void GetStudentsReport(string[] inputPins, int inputCredit, DateTime inputStartingDate, DateTime inputEndingDate, string path, string format)
    {
        using (var dbContext = new CourseraContext())
        {
            try
            {
                IQueryable<Student> studentsQuery = dbContext.Students
                    .Include(s => s.StudentsCoursesXrefs)
                    .ThenInclude(c => c.Course)
                    .ThenInclude(course => course.Instructor);

                if (inputPins.Length == 1)
                {
                    studentsQuery = studentsQuery.Where(s => s.Pin == inputPins[0]);
                }
                else if (inputPins.Length > 1)
                {
                    studentsQuery = studentsQuery.Where(x => inputPins.Equals(x.Pin));
                }
                else
                {
                    studentsQuery = studentsQuery;
                }

                List<Student> studentsReport = studentsQuery
                    .Where(s => s.StudentsCoursesXrefs
                        .Where(c => c.CompletionDate >= inputStartingDate && c.CompletionDate <= inputEndingDate)
                        .Sum(c => c.Course.Credit) >= inputCredit)
                    .ToList();

                if (format == "csv")
                {
                    CSVReport report = new CSVReport(studentsReport, path);
                }
                else if (format == "html")
                {
                    HtmlReport report = new HtmlReport(studentsReport, path);
                }
                else
                {
                    HtmlReport htmlReport = new HtmlReport(studentsReport, path);
                    CSVReport csvReport = new CSVReport(studentsReport, path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while querying students: {ex.Message}");
            }
        }
    }
}
