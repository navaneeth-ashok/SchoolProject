using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models
{
    public class Classes
    {
        public int classId;
        public string classCode;
        public int teacherId;
        public DateTime startdate;
        public DateTime finishdate;
        public string classname;
        public string teacherName;
        public string employeeNumber;
    }

    public class ClassesSearchClass
    {
        public int classId;
        public string classCode;
        public string teacherId;
        public DateTime startdate;
        public DateTime finishdate;
        public string classname;
        public string teacherName;
        public string employeeNumber;
    }
}