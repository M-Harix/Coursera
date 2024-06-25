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
                <title>Sample HTML File</title>
            </head>
            <body>
                <h1>Hello, World!</h1>
                <p>This is a sample HTML file created using C#.</p>
            </body>
            </html>";

            // Specify the file path
            string filePath = "E://a1.html";

            // Write the HTML content to the file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(htmlContent);
            }

            Console.WriteLine($"HTML file created successfully at {Path.GetFullPath(filePath)}");

        }
    }
}
