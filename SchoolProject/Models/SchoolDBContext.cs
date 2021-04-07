using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace SchoolProject.Models
{
    public class SchoolDBContext
    {
        private static string User { get { return "monitor"; } }
        private static string Password { get { return "r3adU$erp4d"; } }
        private static string Database { get { return "schooldb"; } }
        private static string Server { get { return "schooldb-1.cfuaabilmpd2.us-east-2.rds.amazonaws.com"; } }
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