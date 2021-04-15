using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models.ViewModels
{   
    // a class to hold details of a student and the classes the student is taking
    public class StudentAndTheirClasses
    {
        public Student student;
        public IEnumerable<Classes> classes;
    }

    public class ClassAndTheirStudents
    {
        public Classes classes;
        public IEnumerable<Student> students;
    }
}