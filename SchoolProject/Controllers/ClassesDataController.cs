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
        /// A function to display detailed information regarding a particular class
        /// </summary>
        /// <param name="id">An integer representing the PKEY in the DB</param>
        /// <returns>A Classes object with details fetched from DB</returns>
        [HttpGet]
        [Route("api/ClassesData/FindClass/{id}")]
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
                NewClass.classId = Convert.ToString(ResultSet["classid"]);
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
        /// A function to filter and list all the classes that follows certain constraints
        /// set by the input
        /// </summary>
        /// <param name="searchClass">A classes object which contains few fields on which filtering needs to be done</param>
        /// <returns>A list of classes object with details fetched from DB</returns>
        [HttpPost]
        [Route("api/ClassesData/FilterClasses/")]
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

            if (searchClass.classId != null)
            {
                // function used only for finding all the classes taken by a student
                // filtering by the classid is not provided as it doesn't make sense
                // for filtering by classid please use filtering by classcode like HTTP5101
                int index = 1;
                string strAppend = "";
                String[] strArrayIDs;
                string strNames = searchClass.classId;
                strArrayIDs = strNames.Split(',');
                string paramName;
                foreach (String item in strArrayIDs)
                {
                    paramName = "@idParam" + index;
                    cmd.Parameters.AddWithValue(paramName, item); //Making individual parameters for every ID  
                    strAppend += paramName + ",";
                    index += 1;
                }
                strAppend = strAppend.ToString().Remove(strAppend.LastIndexOf(","), 1); //Remove the last comma  

                // SQL query for filtering, appended with the parameterized values
                cmd.CommandText = "SELECT * FROM classes  left outer join teachers on classes.teacherid = teachers.teacherid WHERE classid IN (" + strAppend + ")";
            } else
            {
                // SQL query for normal filtering
                cmd.CommandText = "SELECT * FROM classes  left outer join teachers on classes.teacherid = teachers.teacherid  where" +
                    " lower(classcode) like lower(@classCodeKey) AND " +
                    " (lower(teachers.teacherfname) like lower(@teacherNameKey) or" +
                    " lower(teachers.teacherlname) like lower(@teacherNameKey) or" +
                    " lower(CONCAT(teachers.teacherfname, ' ',teachers.teacherlname)) like lower(@teacherNameKey)) AND " +
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

            }
            cmd.Prepare();

            // Storing the result of query execution into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();
            // empty list of type Classes
            List<Classes> ClassesDetails = new List<Classes> { };


            // Read until the result set is complete
            while (ResultSet.Read())
            {
                Classes NewClass = new Classes
                {
                    classId = Convert.ToString(ResultSet["classid"]),
                    classCode = Convert.ToString(ResultSet["classcode"]),
                    teacherId = Convert.ToInt32(ResultSet["teacherid"]),
                    startdate = DateTime.Parse(Convert.ToString(ResultSet["startdate"])),
                    finishdate = DateTime.Parse(Convert.ToString(ResultSet["finishdate"])),
                    classname = Convert.ToString(ResultSet["classname"]),
                    teacherName = Convert.ToString(ResultSet["teacherfname"] + " " + ResultSet["teacherlname"]) ,
                    employeeNumber = Convert.ToString(ResultSet["employeenumber"])
                };

                // Adding classes object into a list
                ClassesDetails.Add(NewClass);
            }

            // Close the connection
            Conn.Close();
            // Return the list of classes objects
            return ClassesDetails;
        }
    }
}
