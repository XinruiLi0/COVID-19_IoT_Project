using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Web.Models
{
    public static class DatabaseConnector
    {
        static SqlConnection connection = new SqlConnection("Server=ivmsdb.cs17etkshc9t.us-east-1.rds.amazonaws.com,1433;Database=ivmsdb;User ID=admin;Password=ivmsdbadmin;Trusted_Connection=false;");

        public static Dictionary<string,string> userRegister(string userName, string userPassword, int userRole)
        {
            
            
            return new Dictionary<string, string>
            {
                {"result","success"}, {"message", "Success."}
            };
        }

        public static Dictionary<string, string> userLogin(string userName, string userPassword, int userRole)
        {
            DataSet ds = new DataSet();

            try
            {
                connection.Open();
                SqlDataAdapter adp = new SqlDataAdapter($"select * from AccountLogin where userName = '{userName}'", connection);
                adp.Fill(ds);
            }
            catch
            {
               
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close(); 
            }

            // Do something with data set

            if (userName == null)
            {
                return new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Account not exist."}
                };
            }
            else if (userPassword == null)
            {
                return new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Password incorrect."}
                };
            }
            else if (userRole == 0)
            {
                return new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Usre didn't have permission in this role."}
                };
            }
            else
            {
                return new Dictionary<string, string>
                {
                    {"result","success"}, {"message", "Success."}
                };
            }
        }
    }
}
