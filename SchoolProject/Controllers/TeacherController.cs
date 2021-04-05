using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        // GET: Teacher/List
        public ActionResult List(string searchKey=null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.ListTeachers(searchKey);
            return View(teachers);
        }

        // GET: Teacher/Show/{id}
        // Deprecated method : Please use Filter
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher newTeacher = controller.FindTeacher(id);


            return View(newTeacher);
        }

        // POST: Teacher/Filter/{}
        public ActionResult Filter(string name = null, string salaryLow = null, string salaryHigh = null, string hireDateLow = null, string hireDateHigh = null, string employeeNumber = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> filterTeachers = controller.FilterTeachers(name, salaryLow, salaryHigh, hireDateLow, hireDateHigh, employeeNumber);
            return View(filterTeachers);
        }
    }
}