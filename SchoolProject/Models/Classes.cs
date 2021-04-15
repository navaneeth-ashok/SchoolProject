using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models
{
    // a class for storing info regarding classes
    public class Classes
    {
        public string classId = Convert.ToString(null);
        public string classCode = Convert.ToString(null);
        public int teacherId = Convert.ToInt32(null);
        public DateTime startdate = DateTime.Parse("2000-01-01");
        public DateTime finishdate = DateTime.Parse("2200-01-01");
        public string classname = Convert.ToString(null);
        public string teacherName = Convert.ToString(null);
        public string employeeNumber = Convert.ToString(null);
    }
}