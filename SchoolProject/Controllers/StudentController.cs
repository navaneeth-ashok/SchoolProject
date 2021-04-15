using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;
using SchoolProject.Models.ViewModels;

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

            StuXCla filterClass = new StuXCla
            {
                student_id = id
            };

            StudentXClassesDataController studentxclass_controller = new StudentXClassesDataController();
            IEnumerable<StuXCla> listofStuXClass = studentxclass_controller.ListClassesOfStudent(filterClass);

            List<Classes> listofClassesStudentIsTaking = new List<Classes> { };
            ClassesDataController class_controller = new ClassesDataController();
            string StringOfClassIDs = "";
            foreach(var StuXIns in listofStuXClass)
            {
                StringOfClassIDs += StuXIns.class_id + ",";
            }

            Classes listClass = new Classes
            {
                classId = StringOfClassIDs
            };

            IEnumerable<Classes> ListOfClassesTakenByAStudent = class_controller.FilterClasses(listClass);

            StudentAndTheirClasses studentclass = new StudentAndTheirClasses
            {
                student = newStudent,
                classes = ListOfClassesTakenByAStudent
            };

            return View(studentclass);
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