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
        private static string connectionstring = "Server=ivmsdb.cs17etkshc9t.us-east-1.rds.amazonaws.com,1433;Database=ivmsdb;User ID=admin;Password=ivmsdbadmin;Trusted_Connection=false;";

        /// <summary>
        /// Convert Datatable to Dictionary
        /// </summary>
        /// <param name="dataTable">Datatable</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<int, Dictionary<string, string>> DataTableToDictionary(DataTable dataTable)
        {
            Dictionary<int, Dictionary<string, string>> result = new Dictionary<int, Dictionary<string, string>>();
            if (dataTable != null)
            {
                int rowNum = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var rowDetail = new Dictionary<string, string>();
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        rowDetail.Add(dataColumn.ColumnName, dataRow[dataColumn].ToString());
                    }
                    result.Add(rowNum, rowDetail);
                    rowNum++;
                }
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Private method for checking user info by ID directory.
        /// </summary>
        /// <param name="ID">User ID</param>
        /// <returns>A dictionary contains user info.</returns>
        private static Dictionary<string, string> checkStatusByID(string ID)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select AccountLogin.ID as ID, UserName, UserEmail, UserStatus from AccountLogin join HealthStatus on AccountLogin.ID = HealthStatus.ID where AccountLogin.ID = '{ID}'", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            // Convert table to dictionary
            var result = DataTableToDictionary(ds.Tables[0]);

            return result.Count > 0 ? result[0] : new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Visitor account not exist."}
                };
        }

        /// <summary>
        /// Registor an account by using a new email.
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="userEmail">User email</param>
        /// <param name="userPassword">User password</param>
        /// <param name="userRole">User role</param>
        /// <returns>A dictionary that can indicate whether the procress is success or not.</returns>
        public static Dictionary<string,string> userRegister(string userName, string userEmail, string userPassword, int userRole)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select UserEmail from AccountLogin where UserEmail = '{userEmail}'", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            var result = DataTableToDictionary(ds.Tables[0]);
            if (userEmail == null)
            {
                return new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Unknow email."}
                };
            }
            else if (result.Count > 0)
            {
                return new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Account already exist."}
                };
            }

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"insert into AccountLogin (UserName, UserEmail, UserPassword, UserRole) values ('{userName}', '{userEmail}', '{userPassword}', '{userRole}');", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return new Dictionary<string, string>
            {
                {"result","success"}, {"message", "Success."}
            };
        }

        /// <summary>
        /// Check user login.
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="userPassword">User password</param>
        /// <param name="userRole">User role</param>
        /// <returns>If success, return a dictionary contains user name, return error message otherwise.</returns>
        public static Dictionary<string, string> userLogin(string userEmail, string userPassword, int userRole)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select UserName, UserEmail, UserPassword, UserRole from AccountLogin where UserEmail = '{userEmail}'", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            
            // Convert table to dictionary
            var result = DataTableToDictionary(ds.Tables[0]);

            if (userEmail == null || result.Count == 0)
            {
                return new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Account not exist."}
                };
            }
            else if (userPassword == null || !userPassword.Equals(result[0]["UserPassword"]))
            {
                return new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Password incorrect."}
                };
            }
            else if (userRole != int.Parse(result[0]["UserRole"]))
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
                    {"result","success"}, {"message", $"{result[0]["UserName"]}"}
                };
            }
        }

        /// <summary>
        /// Check visitor's health status for third-party (guard and doctor).
        /// </summary>
        /// <param name="userEmail">Current user email</param>
        /// <param name="userPassword">Current user password</param>
        /// <param name="userRole">Current user role</param>
        /// <param name="visitorEmail">Visitor Email</param>
        /// <returns>A dictionary contains health status.</returns>
        public static Dictionary<string, string> checkVisitorStatus(string userEmail, string userPassword, int userRole, string visitorEmail)
        {
            var check = userLogin(userEmail, userPassword, userRole);
            if (!check["result"].Equals("success"))
            {
                return check;
            }

            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(userRole == 3 ? $"select AccountLogin.ID as ID, UserName, UserEmail, UserStatus from AccountLogin join HealthStatus on AccountLogin.ID = HealthStatus.ID where UserEmail = '{visitorEmail}'" : $"select UserName, UserStatus from AccountLogin join HealthStatus on AccountLogin.ID = HealthStatus.ID where UserEmail = '{visitorEmail}'", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            // Convert table to dictionary
            var result = DataTableToDictionary(ds.Tables[0]);

            return result.Count > 0 ? result[0] : new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Visitor account not exist."}
                };
        }

        /// <summary>
        /// Update visitor's health status.
        /// </summary>
        /// <param name="userEmail">Current user email</param>
        /// <param name="userPassword">Current user password</param>
        /// <param name="visitorID">Visitor id</param>
        /// <param name="status">Health status</param>
        /// <returns>A dictionary contains updated health status.<</returns>
        public static Dictionary<string, string> updatePatientStatus(string userEmail, string userPassword, string visitorID, float status)
        {
            // Check permission
            var check = userLogin(userEmail, userPassword, 3);
            if (!check["result"].Equals("success"))
            {
                return check;
            }

            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"update HealthStatus set UserStatus = {status} WHERE ID = {visitorID}", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return checkStatusByID(visitorID);
        }

        /// <summary>
        /// Self-checking user's health status.
        /// </summary>
        /// <param name="userEmail">Current user email</param>
        /// <param name="userPassword">Current user password</param>
        /// <returns>A dictionary contains health status.</returns>
        public static Dictionary<string, string> checkUserStatus(string userEmail, string userPassword)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select UserName, UserStatus from AccountLogin join HealthStatus on AccountLogin.ID = HealthStatus.ID where UserEmail = '{userEmail}' and UserPassword = '{userPassword}'", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            // Convert table to dictionary
            var result = DataTableToDictionary(ds.Tables[0]);

            return result.Count > 0 ? result[0] : new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "User health status not exist."}
                };
        }

        /// <summary>
        /// Raise an alert to prodiction subsystem for abnormal visitor's body temperature.
        /// </summary>
        /// <param name="userEmail">Current user email</param>
        /// <param name="userPassword">Current user password</param>
        /// <param name="visitorEmail">Visitor Email</param>
        /// <returns>Return success message in default.</returns>
        public static Dictionary<string, string> abnormalBodyTrmperatureAlert(string userEmail, string userPassword, string visitorEmail)
        {
            // Check permission
            var check = userLogin(userEmail, userPassword, 2);
            if (!check["result"].Equals("success"))
            {
                return check;
            }

            // More detail need to be added.
            return new Dictionary<string, string>
            {
                {"result","success"}, {"message", "Alert received."}
            };
        }
    }
}
