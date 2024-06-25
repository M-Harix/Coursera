using Coursera.Models;
using System.Formats.Asn1;
using System.Globalization;
using Coursera;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
namespace Coursera
{
    public class CSVReport
    {

        public CSVReport(List<Student> _students, string outputDirectory)
        {
            string csvReportPath = Path.Combine(outputDirectory, "studentsreport.csv");
            CsvReport(_students, csvReportPath);
            Console.WriteLine($"CSV report saved to: {csvReportPath}");
        }
        static void CsvReport(List<Student> students, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteField("Student Name");
                csv.WriteField("Total Credit");
                csv.NextRecord();
                csv.WriteField("");
                csv.WriteField("Course Name");
                csv.WriteField("Total Time");
                csv.WriteField("Credit");
                csv.WriteField("Instructor Name");
                csv.NextRecord();

                foreach (var student in students)
                {
                    csv.WriteField($"{student.FirstName} {student.LastName}");
                    csv.WriteField(student.StudentsCoursesXrefs.Sum(c => c.Course.Credit));
                    csv.NextRecord();

                    foreach (var course in student.StudentsCoursesXrefs)
                    {
                        csv.WriteField("");
                        csv.WriteField(course.Course.Name);
                        csv.WriteField(course.Course.TotalTime);
                        csv.WriteField(course.Course.Credit);
                        csv.WriteField($"{course.Course.Instructor.FirstName} {course.Course.Instructor.LastName}");
                        csv.NextRecord();
                    }
                }
            }
        }
    }
}