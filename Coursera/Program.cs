using Coursera.Data;
using Coursera.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Coursera;
using Microsoft.IdentityModel.Tokens;


internal class Program
{
    string std_pin_Input;
    private static void Main(string[] args)
    {
        TakeIputs();
    }
    static void TakeIputs()
    {
        Console.WriteLine("Enter the Comma separated list of personal identifiers (PIN) of the students to be included in the report OR press Enter to select all students:");
        string std_pin_Input = Console.ReadLine();
        string[] std_pin_Input_List = String.IsNullOrWhiteSpace(std_pin_Input) ? new string[0] : std_pin_Input.Split(',');

    credit:
        Console.WriteLine("Enter required minimum credit:");
        string minimumCredit = Console.ReadLine();
        if (int.TryParse(minimumCredit, out int minimum_Credit))
        {
        }
        else
        {
            Console.WriteLine("Credit is required..");
            goto credit;
        }

    startdate:
        Console.WriteLine("Enter the start date of the time period for which the students should have collected the requested credit:");
        string startdate = Console.ReadLine();
        DateTime start_date;
        if (DateTime.TryParseExact(startdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out start_date))
        {
        }
        else
        {
            Console.WriteLine("Invalid date format. Please enter the date in the correct format.");
            goto startdate;
        }

    enddate:
        Console.WriteLine("Enter the end date of the time period for which the students should have collected the requested credit:");
        string enddate = Console.ReadLine();
        DateTime end_date;
        if (DateTime.TryParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out end_date))
        {
        }
        else
        {
            Console.WriteLine("Invalid date format. Please enter the date in the correct format.");
            goto enddate;
        }

        Console.WriteLine("Enter the output format (csv or html) OR Press Enter for both:");
        string format = Console.ReadLine();

        path:
        Console.WriteLine("Enter the path to save result: e.g; (D://New Folder//)");
        string path = Console.ReadLine();
        if (path.IsNullOrEmpty())
        {
            Console.WriteLine("Path is required..");
            goto path;
        }
        
        List<Student> _students = GetStudentsReport(std_pin_Input_List, minimum_Credit, start_date, end_date);

        if (format == "csv")
        {
            CSVReport _report = new CSVReport(_students, path);
        }
        else if (format == "html")
        {
            HtmlReport _report = new HtmlReport(_students, path);
        }
        else
        {
            HtmlReport _report = new HtmlReport(_students, path);
            CSVReport report = new CSVReport(_students, path);
        }
    }

    public static List<Student> GetStudentsReport(string[] InputPin, int InputCredit, DateTime InputStartingDate, DateTime InputEndingDate)
    {
        using (var dbContext = new CourseraContext())
        {
            try
            {
                IQueryable<Student> studentsQuery = dbContext.Students
                    .Include(s => s.StudentsCoursesXrefs)
                    .ThenInclude(c => c.Course)
                    .ThenInclude(course => course.Instructor);


                if (InputPin.Length == 1)
                {
                    studentsQuery = studentsQuery.Where(s => s.Pin == InputPin[0]);
                }
                else
                {
                    studentsQuery = studentsQuery.Where(s => InputPin.Contains(s.Pin));
                }

                List<Student> stduents_report = studentsQuery
                    .Where(s => s.StudentsCoursesXrefs
                        .Where(c => c.CompletionDate >= InputStartingDate && c.CompletionDate <= InputEndingDate)
                        .Sum(c => c.Course.Credit) >= InputCredit)
                    .ToList();
                return stduents_report;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while querying students: {ex.Message}");
            }
        }
        return null;
    }
}
