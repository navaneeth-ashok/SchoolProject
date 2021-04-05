using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models;
using MySql.Data.MySqlClient;

namespace SchoolProject.Controllers
{
    public class StudentDataController : ApiController
    {
        // DB class for accessing the DB
        private SchoolDBContext School = new SchoolDBContext();

        /// <summary>
        /// Returns a list of students
        /// </summary>
        /// <param name="searchKey">Parameter for filtering the result</param>
        /// <returns>
        /// A list of student objects
        /// </returns>
        [HttpGet]
        [Route("api/StudentData/ListStudents/{searchKey?}")]
        public IEnumerable<Student> ListStudents(string searchKey = null)
        {
            // Creating instance of the connection
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection between server and DB
            Conn.Open();
            // New command for query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query for filtering
            cmd.CommandText = "Select * from students where lower(studentfname) like lower(@key) or" +
                " lower(studentlname) like lower(@key) or" +
                " lower(CONCAT(studentfname, ' ',studentlname)) like lower(@key) or " +
                "lower(studentnumber) like lower(@key);";

            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");
            cmd.Prepare();

            // Storing the result of query execution into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // empty list of type student
            List<Student> StudentDetails = new List<Student> { };


            // Read until the result set is complete
            while (ResultSet.Read())
            {
                int studentID = Convert.ToInt32(ResultSet["studentid"]);
                string studentNumber = Convert.ToString(ResultSet["studentnumber"]);
                string studentFname = Convert.ToString(ResultSet["studentfname"]);
                string studentLname = Convert.ToString(ResultSet["studentlname"]);
                DateTime enrolDate = DateTime.Parse(Convert.ToString(ResultSet["enroldate"]));
                
                Student Newstudent = new Student
                {
                    studentId = studentID,
                    studentFname = studentFname,
                    studentLname = studentLname,
                    studentNumber = studentNumber,
                    enrolDate = enrolDate
                };

                // Adding student object into a list
                StudentDetails.Add(Newstudent);
            }

            // Close the connection
            Conn.Close();
            // Return the list of student objects
            return StudentDetails;
        }

        /// <summary>
        /// Returns a student's detail from the DB
        /// </summary>
        /// <param name="id">student's ID in the DB</param>
        /// <returns>student object</returns>
        [HttpGet]
        public Student FindStudent(int id)
        {
            // Creating connection to access DB
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection
            Conn.Open();
            // Creating a command for sending query
            MySqlCommand cmd = Conn.CreateCommand();
            // SQL query for retrieving a student's info
            cmd.CommandText = "Select * from students where studentid = @key";
            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@key", id);
            cmd.Prepare();
            // Storing result into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // Creating a student object to store the data
            Student NewStudent = new Student();

            while (ResultSet.Read())
            {
                int studentID = Convert.ToInt32(ResultSet["studentid"]);
                string studentNumber = Convert.ToString(ResultSet["studentnumber"]);
                string studentFname = Convert.ToString(ResultSet["studentfname"]);
                string studentLname = Convert.ToString(ResultSet["studentlname"]);
                DateTime enrolDate = DateTime.Parse(Convert.ToString(ResultSet["enroldate"]));

                NewStudent.studentId = studentID;
                NewStudent.studentFname = studentFname;
                NewStudent.studentLname = studentLname;
                NewStudent.studentNumber = studentNumber;
                NewStudent.enrolDate = enrolDate;
            }
            return NewStudent;
        }

        /// <summary>
        /// This is a filter to apply on the list of teachers
        /// </summary>
        /// <param name="name">A substring of firstname and/or last name</param>
        /// <param name="salaryLow">salary low point</param>
        /// <param name="salaryHigh">salary high point</param>
        /// <param name="hireDateLow">hiredate low point</param>
        /// <param name="hireDateHigh">hiredate high point</param>
        /// <param name="employeeNumber">an exact matching number</param>
        /// <returns>A list of teachers who follows the conditions passed</returns>
        [HttpPost]
        [Route("api/StudentData/FilterStudents/{name?}/{enrolDateLow?}/{enrolDateHigh?}/{studentNumber?}")]
        public IEnumerable<Student> FilterStudents(string name = null, string enrolDateLow = null, string enrolDateHigh = null, string studentInpNumber = null)
        {
            // setting default values
            if (enrolDateLow == null) { enrolDateLow = "1900-01-01T00:00:00"; }  
            if (enrolDateHigh == null) { enrolDateHigh = "2250-01-01T00:00:00"; }
            // Creating instance of the connection
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection between server and DB
            Conn.Open();
            // New command for query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query for filtering
            cmd.CommandText = "Select * from students where" +
                "(lower(studentfname) like lower(@nameKey) OR " +
                "lower(studentlname) like lower(@nameKey) OR " +
                "lower(CONCAT(studentfname, ' ',studentlname)) like lower(@nameKey)) AND  " +
                "lower(studentnumber) like lower(@studentNumberKey) AND " +
                "(enroldate > @enrolDateLowKey AND enroldate < @enrolDateHighKey);";

            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@nameKey", "%" + name + "%");
            cmd.Parameters.AddWithValue("@studentNumberKey", "%" + studentInpNumber + "%");
            cmd.Parameters.AddWithValue("@enrolDateLowKey", enrolDateLow);
            cmd.Parameters.AddWithValue("@enrolDateHighKey", enrolDateHigh);
            cmd.Prepare();

            // Storing the result of query execution into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // empty list of type Teacher
            List<Student> StudentDetails = new List<Student> { };


            // Read until the result set is complete
            while (ResultSet.Read())
            {
                int studentID = Convert.ToInt32(ResultSet["studentid"]);
                string studentNumber = Convert.ToString(ResultSet["studentnumber"]);
                string studentFname = Convert.ToString(ResultSet["studentfname"]);
                string studentLname = Convert.ToString(ResultSet["studentlname"]);
                DateTime enrolDate = DateTime.Parse(Convert.ToString(ResultSet["enroldate"]));

                Student NewStudent = new Student
                {
                    studentId = studentID,
                    studentFname = studentFname,
                    studentLname = studentLname,
                    studentNumber = studentNumber,
                    enrolDate = enrolDate
                };

                // Adding teacher object into a list
                StudentDetails.Add(NewStudent);
            }

            // Close the connection
            Conn.Close();
            // Return the list of teacher objects
            return StudentDetails;
        }
    }
}
