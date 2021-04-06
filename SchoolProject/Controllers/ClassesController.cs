using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

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
            return View(newClass);
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
                classId = Convert.ToInt32(class_id),
                classCode = Convert.ToString(class_code),
                teacherId = Convert.ToInt32(teacher_id),
                startdate = DateTime.Parse(date_start),
                finishdate = DateTime.Parse(date_end),
                classname = Convert.ToString(class_name),
                teacherName = Convert.ToString(teacher_name),
                employeeNumber = Convert.ToString(employee_number)
            };

            ClassesDataController controller = new ClassesDataController();
            IEnumerable<Classes> filterStudents = controller.FilterClasses(NewSearchClass);
            return View(filterStudents);
        }
    }
}