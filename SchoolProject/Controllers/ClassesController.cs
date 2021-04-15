using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using SchoolProject.Models.ViewModels;

namespace SchoolProject.Controllers
{
    public class ClassesController : Controller
    {
        // GET: Classes
        public ActionResult Index()
        {
            return View();
        }

        // GET: Classes/Show/{}
        // ActionResult function to display details about a particular class
        public ActionResult Show(int id)
        {
            ClassesDataController controller = new ClassesDataController();
            Classes newClass = controller.FindClass(id);

            StuXCla filterClass = new StuXCla
            {
                class_id = id
            };

            StudentXClassesDataController studentxclass_controller = new StudentXClassesDataController();
            IEnumerable<StuXCla> listofStuXClass = studentxclass_controller.ListClassesOfStudent(filterClass);

            List<Student> listofStudentsTakingAClass = new List<Student> { };
            StudentDataController student_controller = new StudentDataController();
            string StringOfStudentIDs = "";
            
            foreach (var StuXIns in listofStuXClass)
            {
                StringOfStudentIDs += StuXIns.student_id + ",";
            }

            IEnumerable<Student> ListOfStudentsInAClass = student_controller.FilterStudents(null,null,null,null,StringOfStudentIDs);

            ClassAndTheirStudents studentclass = new ClassAndTheirStudents
            {
                classes = newClass,
                students = ListOfStudentsInAClass
            };

            return View(studentclass);
        }

        //POST: Classes/Filter/{}
        // ActionResult function to filter and display all the classes that follows the defined constraints.
        public ActionResult Filter(string class_id = null, string class_code = null, string teacher_id = null,
            string date_start = null, string date_end = null, string class_name = null, string teacher_name = null, string employee_number = null)
        {
            // setting default date here for testing
            if (date_start == null) { date_start = "2000-01-01"; }
            if (date_end == null) { date_end = "2200-01-01"; }

            Classes NewSearchClass = new Classes
            {
                classId = Convert.ToString(class_id),
                classCode = Convert.ToString(class_code),
                teacherId = Convert.ToInt32(teacher_id),
                startdate = DateTime.Parse(date_start),
                finishdate = DateTime.Parse(date_end),
                classname = Convert.ToString(class_name),
                teacherName = Convert.ToString(teacher_name),
                employeeNumber = Convert.ToString(employee_number)
            };

            ClassesDataController controller = new ClassesDataController();
            IEnumerable<Classes> filterClasses = controller.FilterClasses(NewSearchClass);
            return View(filterClasses);
        }
    }
}