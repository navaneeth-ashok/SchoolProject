using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models.ViewModels
{
    // a class to hold the details of the teacher and the classes they are teaching
    public class TeacherClass
    {
        public IEnumerable<Classes> classes;
        public Teacher teacher;
    }
}