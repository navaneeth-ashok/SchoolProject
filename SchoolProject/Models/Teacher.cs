using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models
{
    public class Teacher
    {
        // The following fields define a teacher
        public int teacherId;
        public string teacherFname;
        public string teacherLname;
        public string employeeNumber;
        public string salary;
        public DateTime hireDate;
        public IEnumerable<ClassNamesID> classList;
    }

    // class that contains class id and class name 
    public class ClassNamesID
    {
        public int classId;
        public string className;
    }
}