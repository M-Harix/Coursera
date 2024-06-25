using Coursera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursera
{
    public  class HtmlReport
    {
        public HtmlReport(List<Student> students, string outputDirectory)
        {

            string htmlContent = @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Students Report HTML File</title>
            </head>
                <body>
                    <h1>Students Report</h1>
                    <table style='border: 1px solid black;'>
                        <tr style='background-color: blue;'>
                            <th style='border: 1px solid black;'>Student Name</th>
                            <th style='border: 1px solid black;'>Total Credit</th>
                            <th></th>
                            <th></th>
                            <th></th>
                        </tr>
                        <tr style='background-color: blue;'>
                            <th style='border: 1px solid black;'></th>
                            <th style='border: 1px solid black;'>Course Name</th>
                            <th style='border: 1px solid black;'>Total Time</th>
                            <th style='border: 1px solid black;'>Credit</th>
                            <th style='border: 1px solid black;'>Instructor Name</th>
                        </tr>";        

        string filePath = outputDirectory + "studentsreport.html";

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(htmlContent);

                foreach (var student in students)
                {
                    writer.WriteLine("<tr style='background-color: skyblue;'>");
                    writer.WriteLine($"<td>{student.FirstName} {student.LastName}</td>");
                    writer.WriteLine($"<td>{student.StudentsCoursesXrefs.Sum(c => c.Course.Credit)}</td>");
                    writer.WriteLine($"<td></td>");
                    writer.WriteLine($"<td></td>");
                    writer.WriteLine($"<td></td>");
                    writer.WriteLine("</tr>");

                    foreach (var course in student.StudentsCoursesXrefs)
                    {
                        writer.WriteLine("<tr style='background-color: lightgreen;'>>");
                        writer.WriteLine($"<td></td>");
                        writer.WriteLine($"<td>{course.Course.Name}</td>");
                        writer.WriteLine($"<td>{course.Course.TotalTime}</td>");
                        writer.WriteLine($"<td>{course.Course.Credit}</td>");
                        writer.WriteLine($"<td>{course.Course.Instructor.FirstName} {course.Course.Instructor.LastName}</td>");
                        writer.WriteLine("</tr>");
                    }

                    writer.WriteLine("</table>");
                    writer.WriteLine("</td>");
                    writer.WriteLine("</tr>");
                }

                writer.WriteLine("</table>");

                writer.WriteLine("</body>");
                writer.WriteLine("</html>");
            }
            Console.WriteLine($"HTML file created successfully at {Path.GetFullPath(filePath)}");
        }
    }
}
