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
    public class ClassesDataController : ApiController
    {
        // DB class for accessing the DB
        private SchoolDBContext School = new SchoolDBContext();
        /// <summary>
        /// Returns a student's detail from the DB
        /// </summary>
        /// <param name="id">student's ID in the DB</param>
        /// <returns>student object</returns>
        [HttpGet]
        public Classes FindClass(int id)
        {
            // Creating connection to access DB
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection
            Conn.Open();
            // Creating a command for sending query
            MySqlCommand cmd = Conn.CreateCommand();
            // SQL query for retrieving a student's info
            cmd.CommandText = "SELECT * FROM classes left outer join teachers on classes.teacherid = teachers.teacherid where classid = @key";
            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@key", id);
            cmd.Prepare();
            // Storing result into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // Creating a student object to store the data
            Classes NewClass = new Classes();

            while (ResultSet.Read())
            {
                NewClass.classId = Convert.ToInt32(ResultSet["classid"]);
                NewClass.classCode = Convert.ToString(ResultSet["classcode"]);
                NewClass.teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                NewClass.startdate = DateTime.Parse(Convert.ToString(ResultSet["startdate"]));
                NewClass.finishdate = DateTime.Parse(Convert.ToString(ResultSet["finishdate"]));
                NewClass.classname = Convert.ToString(ResultSet["classname"]);
                NewClass.teacherName = Convert.ToString(ResultSet["teacherfname"]) + " " + Convert.ToString(ResultSet["teacherlname"]);
                NewClass.employeeNumber = Convert.ToString(ResultSet["employeenumber"]);
                
            }
            return NewClass;
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
        [Route("api/ClassesData/FilterClasses/{searchClass}")]
        public IEnumerable<Classes> FilterClasses(Classes searchClass)
        {
            // setting default values
            if (searchClass.startdate == null) { searchClass.startdate = DateTime.Parse("1900-01-01T00:00:00"); }
            if (searchClass.finishdate == null) { searchClass.finishdate = DateTime.Parse("2250-01-01T00:00:00"); }
            // Creating instance of the connection
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection between server and DB
            Conn.Open();
            // New command for query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query for filtering
            cmd.CommandText = "SELECT * FROM classes  left outer join teachers on classes.teacherid = teachers.teacherid  where" +
                " lower(classcode) like lower(@classCodeKey) AND " +
                " (lower(teachers.teacherfname) like lower(@teacherNameKey) or" +
                " lower(teachers.teacherlname) like lower(@teacherNameKey) or" +
                " lower(CONCAT(teachers.teacherfname, ' ',teachers.teacherlname)) like lower(@teacherNameKey)) AND "+
                " lower(teachers.employeenumber) like lower(@teacherNumberKey) AND " +
                " lower(classes.classname) like lower(@classNameKey) AND " +
                " (startdate > @startDateKey AND finishdate < @finishDateKey);";

            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@classCodeKey", "%" + searchClass.classCode + "%");
            cmd.Parameters.AddWithValue("@teacherNameKey", "%" + searchClass.teacherName + "%");
            cmd.Parameters.AddWithValue("@teacherNumberKey", "%" + searchClass.employeeNumber + "%");
            cmd.Parameters.AddWithValue("@classNameKey", "%" + searchClass.classname + "%");
            cmd.Parameters.AddWithValue("@startDateKey", searchClass.startdate);
            cmd.Parameters.AddWithValue("@finishDateKey", searchClass.finishdate);
            cmd.Prepare();

            // Storing the result of query execution into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // empty list of type Teacher
            List<Classes> ClassesDetails = new List<Classes> { };


            // Read until the result set is complete
            while (ResultSet.Read())
            {
                Classes NewClass = new Classes
                {
                    classId = Convert.ToInt32(ResultSet["classid"]),
                    classCode = Convert.ToString(ResultSet["classcode"]),
                    teacherId = Convert.ToInt32(ResultSet["teacherid"]),
                    startdate = DateTime.Parse(Convert.ToString(ResultSet["startdate"])),
                    finishdate = DateTime.Parse(Convert.ToString(ResultSet["finishdate"])),
                    classname = Convert.ToString(ResultSet["classname"]),
                    teacherName = Convert.ToString(ResultSet["teacherfname"] + " " + ResultSet["teacherlname"]) ,
                    employeeNumber = Convert.ToString(ResultSet["employeenumber"])
                };

                // Adding teacher object into a list
                ClassesDetails.Add(NewClass);
            }

            // Close the connection
            Conn.Close();
            // Return the list of teacher objects
            return ClassesDetails;
        }
    }
}
