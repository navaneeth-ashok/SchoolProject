using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using SchoolProject.Models.ViewModels;

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
        // Deprecated method : Please use Filter
        public ActionResult List(string searchKey=null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.ListTeachers(searchKey);
            return View(teachers);
        }

        // GET: Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher newTeacher = controller.FindTeacher(id);

            Classes NewSearchClass = new Classes
            {
                employeeNumber = Convert.ToString(newTeacher.employeeNumber)
            };

            ClassesDataController class_controller = new ClassesDataController();
            IEnumerable<Classes> filterClasses = class_controller.FilterClasses(NewSearchClass);

            TeacherClass newTeacherClass = new TeacherClass
            {
                classes = filterClasses,
                teacher = newTeacher
            };

            return View(newTeacherClass);
        }

        // POST: Teacher/Filter/{}
        public ActionResult Filter(string name = null, string salaryLow = null, string salaryHigh = null, string hireDateLow = null, string hireDateHigh = null, string employeeNumber = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> filterTeachers = controller.FilterTeachers(name, salaryLow, salaryHigh, hireDateLow, hireDateHigh, employeeNumber);
            return View(filterTeachers);
        }

        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher newTeacher = controller.FindTeacher(id);
            return View(newTeacher);
        }

        public ActionResult DeleteTeacher(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("Filter");
        }

        public ActionResult CreateTeacher(string teacherFName, string teacherLName, string empNumber, string salary,string teacherid = null, string hireDate = null)
        {   
            // if teacherid is null, we are adding a new data, if the teacherid is present we are updating a record
            if(teacherid == null)
            {
                Teacher newTeacher = new Teacher
                {
                    teacherFname = teacherFName,
                    teacherLname = teacherLName,
                    employeeNumber = empNumber,
                    salary = salary,
                    hireDate = DateTime.Parse(hireDate)
                };
                TeacherDataController controller = new TeacherDataController();
                controller.CreateTeacher(newTeacher, "INSERT");
                return RedirectToAction("Filter");
            } else
            {
                Teacher newTeacher = new Teacher
                {
                    teacherFname = teacherFName,
                    teacherLname = teacherLName,
                    employeeNumber = empNumber,
                    salary = salary,
                    hireDate = DateTime.Parse(hireDate),
                    teacherId = Convert.ToInt32(teacherid)
                };
                TeacherDataController controller = new TeacherDataController();
                controller.CreateTeacher(newTeacher, "UPDATE");
                return RedirectToAction("Show/" + teacherid); 
            }
            
        }


        public ActionResult NewTeacher()
        {
            return View();
        }


        public ActionResult UpdateTeacher(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher newTeacher = controller.FindTeacher(id);
            return View(newTeacher);
        }
    }
}