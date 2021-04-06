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
    public class TeacherDataController : ApiController
    {
        // DB class for accessing the DB
        private SchoolDBContext School = new SchoolDBContext();
        
        /// <summary>
        /// Returns a list of teachers
        /// </summary>
        /// <param name="searchKey">Parameter for filtering the result</param>
        /// <returns>
        /// A list of teacher objects
        /// </returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{searchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string searchKey=null)
        {
            // Creating instance of the connection
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection between server and DB
            Conn.Open();
            // New command for query
            MySqlCommand cmd = Conn.CreateCommand();
            
            // SQL query for filtering
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or" +
                " lower(teacherlname) like lower(@key) or" +
                " lower(CONCAT(teacherfname, ' ',teacherlname)) like lower(@key) or " +
                "lower(employeenumber) like lower(@key);";

            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");
            cmd.Prepare();

            // Storing the result of query execution into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // empty list of type Teacher
            List<Teacher> TeacherDetails = new List<Teacher> { };


            // Read until the result set is complete
            while (ResultSet.Read())
            {
                int teacherID = Convert.ToInt32(ResultSet["teacherid"]);
                string teacherEmpNumber = Convert.ToString(ResultSet["employeenumber"]);
                string teacherFname = Convert.ToString(ResultSet["teacherfname"]);
                string teacherLname = Convert.ToString(ResultSet["teacherlname"]);
                DateTime hireDate = DateTime.Parse(Convert.ToString(ResultSet["hiredate"]));
                string teacherSalary = Convert.ToString(ResultSet["salary"]);

                Teacher NewTeacher = new Teacher
                {
                    teacherId = teacherID,
                    teacherFname = teacherFname,
                    teacherLname = teacherLname,
                    employeeNumber = teacherEmpNumber,
                    hireDate = hireDate,
                    salary = teacherSalary
                };

                // Adding teacher object into a list
                TeacherDetails.Add(NewTeacher);
            }

            // Close the connection
            Conn.Close();
            // Return the list of teacher objects
            return TeacherDetails;
        }

        /// <summary>
        /// Returns a teacher's detail from the DB
        /// </summary>
        /// <param name="id">Teacher's ID in the DB</param>
        /// <returns>Teacher object</returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id)
        {
            // Creating connection to access DB
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection
            Conn.Open();
            // Creating a command for sending query
            MySqlCommand cmd = Conn.CreateCommand();
            // SQL query for retrieving a teacher's info
            cmd.CommandText = "Select * from teachers where teacherid = @key";
            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@key", id);
            cmd.Prepare();
            // Storing result into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // Creating a teacher object to store the data
            Teacher NewTeacher = new Teacher();

            while (ResultSet.Read())
            {
                int teacherID = Convert.ToInt32(ResultSet["teacherid"]);
                string teacherEmpNumber = Convert.ToString(ResultSet["employeenumber"]);
                string teacherFname = Convert.ToString(ResultSet["teacherfname"]);
                string teacherLname = Convert.ToString(ResultSet["teacherlname"]);
                DateTime hireDate = DateTime.Parse(Convert.ToString(ResultSet["hiredate"]));
                string teacherSalary = Convert.ToString(ResultSet["salary"]);

                NewTeacher.teacherId = teacherID;
                NewTeacher.teacherFname = teacherFname;
                NewTeacher.teacherLname = teacherLname;
                NewTeacher.employeeNumber = teacherEmpNumber;
                NewTeacher.hireDate = hireDate;
                NewTeacher.salary = teacherSalary;
                NewTeacher.classList = RetrieveClasslist(teacherID);
            }
            return NewTeacher;
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
        [Route("api/TeacherData/FilterTeachers/{name?}/{salaryLow?}/{salaryHigh?}/{hireDateLow?}/{hireDateHigh?}/{employeeNumber?}")]
        public IEnumerable<Teacher> FilterTeachers(string name = null, string salaryLow = null, string salaryHigh = null, string hireDateLow = null, string hireDateHigh = null, string employeeNumber = null)
        {
            // setting default values
            if (salaryLow == null) { salaryLow = "-1"; }
            if (salaryHigh == null) { salaryHigh = "9999"; }
            if (hireDateLow == null) { hireDateLow = "1900-01-01T00:00:00"; }
            if (hireDateHigh == null) { hireDateHigh = "2250-01-01T00:00:00"; }
            // Creating instance of the connection
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection between server and DB
            Conn.Open();
            // New command for query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query for filtering
            cmd.CommandText = "Select * from teachers where" +
                "(lower(teacherfname) like lower(@nameKey) OR " +
                "lower(teacherlname) like lower(@nameKey) OR " +
                "lower(CONCAT(teacherfname, ' ',teacherlname)) like lower(@nameKey)) AND  " +
                "lower(employeenumber) like lower(@empNumberKey) AND " +
                "(hiredate > @hireDateLowKey AND hiredate < @hireDateHighKey) AND " +
                "(salary > @salaryLowKey AND salary < @salaryHighKey) ;";

            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@nameKey", "%" + name + "%");
            cmd.Parameters.AddWithValue("@empNumberKey", "%" + employeeNumber + "%");
            cmd.Parameters.AddWithValue("@hireDateLowKey", hireDateLow);
            cmd.Parameters.AddWithValue("@hireDateHighKey", hireDateHigh);
            cmd.Parameters.AddWithValue("@salaryLowKey", salaryLow);
            cmd.Parameters.AddWithValue("@salaryHighKey", salaryHigh);
            cmd.Prepare();

            // Storing the result of query execution into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // empty list of type Teacher
            List<Teacher> TeacherDetails = new List<Teacher> { };


            // Read until the result set is complete
            while (ResultSet.Read())
            {
                int teacherID = Convert.ToInt32(ResultSet["teacherid"]);
                string teacherEmpNumber = Convert.ToString(ResultSet["employeenumber"]);
                string teacherFname = Convert.ToString(ResultSet["teacherfname"]);
                string teacherLname = Convert.ToString(ResultSet["teacherlname"]);
                DateTime hireDate = DateTime.Parse(Convert.ToString(ResultSet["hiredate"]));
                string teacherSalary = Convert.ToString(ResultSet["salary"]);

                Teacher NewTeacher = new Teacher
                {
                    teacherId = teacherID,
                    teacherFname = teacherFname,
                    teacherLname = teacherLname,
                    employeeNumber = teacherEmpNumber,
                    hireDate = hireDate,
                    salary = teacherSalary,
                    classList = RetrieveClasslist(teacherID)
                };

                // Adding teacher object into a list
                TeacherDetails.Add(NewTeacher);
            }

            // Close the connection
            Conn.Close();
            // Return the list of teacher objects
            return TeacherDetails;
        }

        /// <summary>
        /// A function to retrieve the list of classes taken by a teacher
        /// </summary>
        /// <param name="teacherID">Teacher ID as mentioned in DB</param>
        /// <returns>A list of type ClassNamesID</returns>
        [HttpGet]
        [Route("api/TeacherData/RetrieveClasslist/{teacherID}")]
        public IEnumerable<ClassNamesID> RetrieveClasslist(int teacherID)
        {
            // Creating instance of the connection
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection between server and DB
            Conn.Open();
            // New command for query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query for filtering
            cmd.CommandText = "SELECT * FROM teachers left outer join classes on teachers.teacherid = classes.teacherid" +
                " where teachers.teacherid = @key;";

            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@Key", teacherID);
            cmd.Prepare();

            // Storing the result of query execution into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // empty list of type ClassNamesID
            List<ClassNamesID> ClassDetails = new List<ClassNamesID> { };


            // Read until the result set is complete
            while (ResultSet.Read())
            {
                // checking a dbnull does not cause any issues for the list
                if (ResultSet["classid"] != DBNull.Value)
                {
                    ClassNamesID NewClassDetailObj = new ClassNamesID
                    {
                        classId = Convert.ToInt32(ResultSet["classid"]),
                        className = Convert.ToString(ResultSet["classname"])
                    };

                    ClassDetails.Add(NewClassDetailObj);
                }
                
            }

            // Close the connection
            Conn.Close();
            // Return the list of classes object
            return ClassDetails;

        }
    }

}
