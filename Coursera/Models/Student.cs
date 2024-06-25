using System;
using System.Collections.Generic;

namespace Coursera.Models;

public partial class Student
{
    public string Pin { get; set; } 

    public string FirstName { get; set; } 

    public string LastName { get; set; } 

    public DateTime TimeCreated { get; set; }

    public virtual ICollection<StudentsCoursesXref> StudentsCoursesXrefs { get; set; } = new List<StudentsCoursesXref>();
}
