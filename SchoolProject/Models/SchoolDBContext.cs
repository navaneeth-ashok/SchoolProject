using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace SchoolProject.Models
{
    public class SchoolDBContext
    {
        private static string User { get { return "humber_student"; } }
        private static string Password { get { return "humberisgreat"; } }
        private static string Database { get { return "humber_school"; } }
        private static string Server { get { return "bittsdevelopment.com"; } }
        private static string Port { get { return "3306"; } }


        protected static string ConnectionString
        {
            get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password;
            }
        }

        public MySqlConnection AccessDatabase()
        {
            // We are instantiating the class to   create and object
            return new MySqlConnection(ConnectionString);
        }

    }
}