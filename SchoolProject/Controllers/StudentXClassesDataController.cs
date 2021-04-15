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
    public class StudentXClassesDataController : ApiController
    {
        // DB class for accessing the DB
        private SchoolDBContext School = new SchoolDBContext();

        [HttpPost]
        [Route("api/StudentXClasses/ListClassesOfStudent/")]
        public IEnumerable<StuXCla> ListClassesOfStudent(StuXCla studentClassFilter)
        {

            // Creating instance of the connection
            MySqlConnection Conn = School.AccessDatabase();
            // Opening connection between server and DB
            Conn.Open();
            // New command for query
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query for filtering
            cmd.CommandText = "Select * from studentsxclasses where studentid = @studentkey or classid = @classkey;";

            // Sanitizing the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@studentkey", studentClassFilter.student_id);
            cmd.Parameters.AddWithValue("@classkey", studentClassFilter.class_id);
            cmd.Prepare();

            // Storing the result of query execution into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // empty list of type Student
            List<StuXCla> Details = new List<StuXCla> { };


            // Read until the result set is complete
            while (ResultSet.Read())
            {
                StuXCla NewStuXClassInstance = new StuXCla
                {
                    class_id = Convert.ToInt32(ResultSet["classid"]),
                    student_id = Convert.ToInt32(ResultSet["studentid"])
                };

                // Adding student object into a list
                Details.Add(NewStuXClassInstance);
            }

            // Close the connection
            Conn.Close();
            // Return the list of student objects
            return Details;
        }
    }
}
