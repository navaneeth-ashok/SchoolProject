using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/List
        // beta implementation : deprecated : use Student/Filter
        public ActionResult List(string searchKey=null)
        {
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> students = controller.ListStudents(searchKey);
            return View(students);
        }

        // GET: Student/Show
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student newStudent = controller.FindStudent(id);
            return View(newStudent);
        }

        // POST: Student/Filter/{}
        public ActionResult Filter(string name = null, string enrolDateLow = null, string enrolDateHigh = null, string studentInpNumber = null)
        {
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> filterStudents = controller.FilterStudents(name, enrolDateLow, enrolDateHigh, studentInpNumber);
            return View(filterStudents);
        }
    }
}