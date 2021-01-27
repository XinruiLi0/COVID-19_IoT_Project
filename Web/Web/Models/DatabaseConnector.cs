using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Concurrent;

namespace Web.Models
{
    public static class DatabaseConnector
    {
        private static string connectionstring = "Server=ivmsdb.cs17etkshc9t.us-east-1.rds.amazonaws.com,1433;Database=ivmsdb;User ID=admin;Password=ivmsdbadmin;Trusted_Connection=false;";

        /// <summary>
        /// Execute sql query
        /// </summary>
        /// <param name="query">SQL query</param>
        /// <returns></returns>
        private static DataTable executeQuery(string query)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter(query, connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return ds.Tables.Count == 0 ? null : ds.Tables[0];
        }

        /// <summary>
        /// Convert Datatable to Dictionary
        /// </summary>
        /// <param name="dataTable">Datatable</param>
        /// <returns>Dictionary</returns>
        private static Dictionary<int, Dictionary<string, string>> DataTableToDictionary(DataTable dataTable)
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
        /// Convert Datatable to Concurrent Bag
        /// </summary>
        /// <param name="dataTable">Datatable</param>
        /// <returns>Concurrent Bag</returns>
        private static ConcurrentBag<Dictionary<string, string>> DataTableToConcurrentBag(DataTable dataTable)
        {
            ConcurrentBag<Dictionary<string, string>> result = new ConcurrentBag<Dictionary<string, string>>();
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var rowDetail = new Dictionary<string, string>();
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        rowDetail.Add(dataColumn.ColumnName, dataRow[dataColumn].ToString());
                    }
                    result.Add(rowDetail);
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
        private static Dictionary<string, string> checkStatusByID(int ID)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select UserName, UserEmail, UserStatus from AccountLogin join HealthStatus on AccountLogin.ID = HealthStatus.ID where AccountLogin.ID = {ID}", connection);
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
        /// Private method for checking user id by email.
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <returns>Return user id.</returns>
        private static int getUserID(string userEmail)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select ID from AccountLogin where UserEmail = '{userEmail}'", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    return -1;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            var result = DataTableToDictionary(ds.Tables[0]);
            return result.Count > 0 ? int.Parse(result[0]["ID"]) : 0;
        }

        /// <summary>
        /// Private method for getting a list of guard devices by guard id.
        /// </summary>
        /// <param name="ID">Guard id</param>
        /// <returns>A list of guard devices.</returns>
        private static Dictionary<int, Dictionary<string, string>> getGuardDevices(int ID)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select DeviceID, Description from GuardDevices where ID = {ID}", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    var temp = new Dictionary<string, string>
                        {
                            {"result","error"}, {"message", e.ToString()}
                        };

                    return new Dictionary<int, Dictionary<string, string>>
                        {
                            {0, temp}
                        };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return DataTableToDictionary(ds.Tables[0]);
        }

        /// <summary>
        /// Prepare necessary information for user history update.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <returns>A dictionary contain guard id and current visitor id</returns>
        private static Dictionary<string, string> prepareActivityUpdate(string deviceID)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select ID, VisitorID from GuardDevices where DeviceID = '{deviceID}'", connection);
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

            return result.Count > 0 ? result[0] : new Dictionary<string, string>
                {
                    {"result","error"}, {"message", "Guard id or visitor id not exist."}
                };
        }

        /// <summary>
        /// Get a list of current visitors in a guard place.
        /// </summary>
        /// <param name="guardID">Guard id</param>
        /// <returns>A concurrent bag of visitor id.</returns>
        private static ConcurrentBag<Dictionary<string, string>> getCurrentContacts(string guardID)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select Visitor_ID from CurrentContact where Guard_ID = {guardID}", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return DataTableToConcurrentBag(ds.Tables[0]);
        }

        /// <summary>
        /// Add detail about the user history when user is enter a guard place.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <returns>Return success in default.</returns>
        private static Dictionary<string, string> visitorActivityUpdate(string deviceID)
        {
            var check = prepareActivityUpdate(deviceID);
            if (check.ContainsKey("result"))
            {
                return check;
            }

            var visitorList = new ConcurrentBag<Dictionary<string, string>>();
            try
            {
                visitorList = getCurrentContacts(check["ID"]);
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
            }

            try
            {
                Parallel.Invoke(
                    () =>
                    {
                        DataSet ds = new DataSet();

                        Parallel.ForEach(visitorList, (v) =>
                        {
                            using (SqlConnection connection = new SqlConnection(connectionstring))
                            {
                                try
                                {
                                    connection.Open();
                                    SqlDataAdapter adp = new SqlDataAdapter($"insert into PersonalContact(ID, Contact_ID, Guard_ID, StartTime, ManualUpdate) values ({check["VisitorID"]}, {v["Visitor_ID"]}, {check["ID"]}, GETDATE(), 0); insert into PersonalContact(ID, Contact_ID, Guard_ID, StartTime, ManualUpdate) values ({v["Visitor_ID"]}, {check["VisitorID"]}, {check["ID"]}, GETDATE(), 0);", connection);
                                    adp.Fill(ds);
                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                                finally
                                {
                                    if (connection.State == ConnectionState.Open)
                                        connection.Close();
                                }
                            }
                        });
                    },
                    () =>
                    {
                        DataSet ds = new DataSet(); 
                        
                        using (SqlConnection connection = new SqlConnection(connectionstring))
                        {
                            try
                            {
                                connection.Open();
                                SqlDataAdapter adp = new SqlDataAdapter($"insert into CurrentContact (Visitor_ID, Guard_ID) values ({check["VisitorID"]}, {check["ID"]})", connection);
                                adp.Fill(ds);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            finally
                            {
                                if (connection.State == ConnectionState.Open)
                                    connection.Close();
                            }
                        }
                    });
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
            }

            return new Dictionary<string, string>
                {
                    {"result","success"}, {"message", "Success."}
                };
        }

        /// <summary>
        /// Registor an account by using a new email.
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="userEmail">User email</param>
        /// <param name="userPassword">User password</param>
        /// <param name="age">User age</param>
        /// <param name="hasInfectedBefore">Whether user being infected before</param>
        /// <returns>A dictionary that can indicate whether the procress is success or not.</returns>
        public static Dictionary<string, string> userRegister(string userName, string userEmail, string userPassword, int age, int hasInfectedBefore)
        {
            var check = getUserID(userEmail);

            if (userEmail == null)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Unknow email."}
                    };
            }
            else if (check != 0)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account already exist."}
                    };
            }

            try
            {
                executeQuery($"insert into AccountLogin (UserName, UserEmail, UserPassword, UserRole) values ('{userName}', '{userEmail}', '{userPassword}', '1')");
                var id = getUserID(userEmail);
                executeQuery($"insert into HealthStatus (ID, Age, HasInfectedBefore, UserStatus) values ({id}, {age}, {hasInfectedBefore}, 0)");
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
            }

            return new Dictionary<string, string>
                {
                    {"result","success"}, {"message", "Success."}
                };
        }

        /// <summary>
        /// Registor an account by using a new email.
        /// </summary>
        /// <param name="guardName">Guard name</param>
        /// <param name="guardEmail">Guard email</param>
        /// <param name="guardPassword">Guard password</param>
        /// <param name="address">Address</param>
        /// <param name="latitude">Latitude</param>
        /// <param name="longitude">Longitude</param>
        /// <returns>A dictionary that can indicate whether the procress is success or not.</returns>
        public static Dictionary<string, string> guardRegister(string guardName, string guardEmail, string guardPassword, string address, float latitude, float longitude)
        {
            var check = getUserID(guardEmail);

            if (guardEmail == null)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Unknow email."}
                    };
            }
            else if (check != 0)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account already exist."}
                    };
            }

            try
            {
                executeQuery($"insert into AccountLogin (UserName, UserEmail, UserPassword, UserRole) values ('{guardName}', '{guardEmail}', '{guardPassword}', '2')");
                var id = getUserID(guardEmail);
                executeQuery($"insert into GuardInfo (ID, Address, Latitude, Longitude) values ('{id}', '{address}', '{latitude}', '{longitude}')");
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>
                        {
                            {"result","error"}, {"message", e.ToString()}
                        };
            }

            return new Dictionary<string, string>
                {
                    {"result","success"}, {"message", "Success."}
                };
        }

        /// <summary>
        /// Registor an account by using a new email.
        /// </summary>
        /// <param name="doctorName">Doctor name</param>
        /// <param name="doctorEmail">Doctor email</param>
        /// <param name="doctorPassword">Doctor password</param>
        /// <returns>A dictionary that can indicate whether the procress is success or not.</returns>
        public static Dictionary<string, string> doctorRegister(string doctorName, string doctorEmail, string doctorPassword)
        {
            var check = getUserID(doctorEmail);

            if (doctorEmail == null)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Unknow email."}
                    };
            }
            else if (check != 0)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account already exist."}
                    };
            }

            try
            {
                executeQuery($"insert into AccountLogin (UserName, UserEmail, UserPassword, UserRole) values ('{doctorName}', '{doctorEmail}', '{doctorPassword}', '3')");
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
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
        /// Check visitor's health status for doctor.
        /// </summary>
        /// <param name="userEmail">Current user email</param>
        /// <param name="userPassword">Current user password</param>
        /// <param name="visitorEmail">Visitor Email</param>
        /// <returns>A dictionary contains health status.</returns>
        public static Dictionary<string, string> checkPatientStatus(string userEmail, string userPassword, string visitorEmail)
        {
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
                    SqlDataAdapter adp = new SqlDataAdapter($"select UserName, UserEmail, UserStatus from AccountLogin join HealthStatus on AccountLogin.ID = HealthStatus.ID where UserEmail = '{visitorEmail}'", connection);
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
        /// <param name="visitorEmail">Visitor email</param>
        /// <param name="status">Health status</param>
        /// <returns>A dictionary contains updated health status.<</returns>
        public static Dictionary<string, string> updatePatientStatus(string userEmail, string userPassword, string visitorEmail, int status)
        {
            // Check permission
            var check = userLogin(userEmail, userPassword, 3);
            if (!check["result"].Equals("success"))
            {
                return check;
            }

            // Get User ID
            var VisitorID = getUserID(visitorEmail);
            if (VisitorID == 0)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account not exist."}
                    };
            }

            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"update HealthStatus set UserStatus = {status} where ID = {VisitorID}", connection);
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

            if (status == 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adp = new SqlDataAdapter($"delete from ConfirmedCases where ID = {VisitorID}", connection);
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
            }
            else if (status == 1)
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adp = new SqlDataAdapter($"insert into ConfirmedCases (ID) values ({VisitorID})", connection);
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
            }

            return checkStatusByID(VisitorID);
        }

        /// <summary>
        /// Self-checking user's health status.
        /// </summary>
        /// <param name="userEmail">Current user email</param>
        /// <param name="userPassword">Current user password</param>
        /// <returns>A dictionary contains health status.</returns>
        public static Dictionary<int, Dictionary<string, string>> checkUserStatus(string userEmail, string userPassword)
        {
            // Check permission
            var check = userLogin(userEmail, userPassword, 1);
            if (!check["result"].Equals("success"))
            {
                return new Dictionary<int, Dictionary<string, string>>
                    {
                        {0, check}
                    };
            }

            // Get User ID
            var id = getUserID(userEmail);
            if (id == 0)
            {
                var temp = new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account not exist."}
                    };

                return new Dictionary<int, Dictionary<string, string>>
                    {
                        {0, temp}
                    };
            }

            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select UserName, UserStatus from AccountLogin join HealthStatus on AccountLogin.ID = HealthStatus.ID where AccountLogin.ID = {id}", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    return new Dictionary<int, Dictionary<string, string>> 
                        {
                            {0, new Dictionary<string, string> { { "result", "error" }, { "message", e.ToString() } } }
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
            if (result.Count == 0)
            {
                result.Add(0, new Dictionary<string, string> { { "result", "error" }, { "message", "User health status not exist." } });
                return result;
            }

            var history = checkContactHistory(id);
            if (history.Count == 1 && history[0].ContainsKey("result"))
            {
                return history;
            }

            for (var i = 0; i < history.Count; ++i)
            {
                result.Add(i + 1, history[i]);
            }

            return result; 
        }

        private static Dictionary<int, Dictionary<string, string>> checkContactHistory(int ID)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"delete from PersonalContact where DATEDIFF(day, EndTime, getdate()) >= 14; select [Address], StartTime, EndTime from PersonalContact join GuardInfo on PersonalContact.Guard_ID = GuardInfo.ID where PersonalContact.ID = {ID} and Contact_ID in (select ID as Contact_ID from ConfirmedCases);", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    var temp = new Dictionary<string, string>
                        {
                            {"result","error"}, {"message", e.ToString()}
                        };

                    return new Dictionary<int, Dictionary<string, string>>
                        {
                            {0, temp}
                        };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            // Convert table to dictionary
            return DataTableToDictionary(ds.Tables[0]);
        }

        /// <summary>
        /// Get guard devices by guard email and password.
        /// </summary>
        /// <param name="userEmail">Guard email</param>
        /// <param name="userPassword">Guard password</param>
        /// <returns>A dictionary of guard devices.</returns>
        public static Dictionary<int, Dictionary<string, string>> getGuardDevices(string userEmail, string userPassword)
        {
            // Check permission
            var check = userLogin(userEmail, userPassword, 2);
            if (!check["result"].Equals("success"))
            {
                return new Dictionary<int, Dictionary<string, string>>
                    {
                        {0, check}
                    };
            }

            // Get User ID
            var id = getUserID(userEmail);
            if (id == 0)
            {
                var temp = new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account not exist."}
                    };

                return new Dictionary<int, Dictionary<string, string>>
                    {
                        {0, temp}
                    };
            }

            return getGuardDevices(id);
        }

        /// <summary>
        /// Register new guard devices.
        /// </summary>
        /// <param name="userEmail">Guard email</param>
        /// <param name="userPassword">Guard password</param>
        /// <param name="deviceID">Device id</param>
        /// <param name="deviceDescription">Device description</param>
        /// <returns>A dictionary of updated guard devices.</returns>
        public static Dictionary<int, Dictionary<string, string>> registerGuardDevice(string userEmail, string userPassword, string deviceID, string deviceDescription)
        {
            // Check permission
            var check = userLogin(userEmail, userPassword, 2);
            if (!check["result"].Equals("success"))
            {
                return new Dictionary<int, Dictionary<string, string>>
                    {
                        {0, check}
                    };
            }

            // Get Guard ID
            var guardID = getUserID(userEmail);
            if (guardID == 0)
            {
                var temp = new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account not exist."}
                    };

                return new Dictionary<int, Dictionary<string, string>>
                    {
                        {0, temp}
                    };
            }

            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"insert into GuardDevices (ID, DeviceID, Description, VisitorID, VisitorTemperature, LastUpdated) values ({guardID}, '{deviceID}', '{deviceDescription}', 1, 37, GETDATE()); ", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    var temp = new Dictionary<string, string>
                        {
                            {"result","error"}, {"message", e.ToString()}
                        };

                    return new Dictionary<int, Dictionary<string, string>>
                        {
                            {0, temp}
                        };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return getGuardDevices(guardID);
        }

        /// <summary>
        /// Delete existed guard device.
        /// </summary>
        /// <param name="userEmail">Guard email</param>
        /// <param name="userPassword">Guard password</param>
        /// <param name="deviceID">Device id</param>
        /// <returns>A dictionary of updated guard devices.</returns>
        public static Dictionary<int, Dictionary<string, string>> deleteGuardDevice(string userEmail, string userPassword, string deviceID)
        {
            // Check permission
            var check = userLogin(userEmail, userPassword, 2);
            if (!check["result"].Equals("success"))
            {
                return new Dictionary<int, Dictionary<string, string>>
                    {
                        {0, check}
                    };
            }

            // Get User ID
            var id = getUserID(userEmail);
            if (id == 0)
            {
                var temp = new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account not exist."}
                    };

                return new Dictionary<int, Dictionary<string, string>>
                    {
                        {0, temp}
                    };
            }

            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"delete from GuardDevices where ID = {id} and DeviceID = '{deviceID}'", connection);
                    adp.Fill(ds);
                }
                catch (Exception e)
                {
                    var temp = new Dictionary<string, string>
                        {
                            {"result","error"}, {"message", e.ToString()}
                        };

                    return new Dictionary<int, Dictionary<string, string>>
                        {
                            {0, temp}
                        };
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return getGuardDevices(id);
        }

        /// <summary>
        /// Update visitor status to guard device list.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <param name="visitorEmail">Visitor Email</param>
        /// <returns>Return success in default, return exception otherwise.</returns>
        public static Dictionary<string, string> visitorDetect(string deviceID, string visitorEmail)
        {
            // Get User ID
            var visitorID = getUserID(visitorEmail);
            if (visitorID == 0)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account not exist."}
                    };
            }

            // Get Guard ID
            var GuardInfo = prepareActivityUpdate(deviceID);
            if (GuardInfo.ContainsKey("result"))
            {
                return GuardInfo;
            }

            var visitorList = new ConcurrentBag<Dictionary<string, string>>();
            try
            {
                visitorList = getCurrentContacts(GuardInfo["ID"]);
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
            }

            var isInside = false;
            foreach (Dictionary<string, string> v in visitorList)
            {
                if (int.Parse(v["Visitor_ID"]) == visitorID)
                {
                    isInside = true;
                }
            }

            if (isInside)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Duplicate record received."}
                    };
            }

            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select top 1 EndTime from PersonalContact where ID = {visitorID} and EndTime is not null order by StartTime desc;", connection);
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

            var check = DataTableToDictionary(ds.Tables[0]);

            if (check.Count != 0)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "User has not record for last exit activity."}
                    };
            }

            ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"update GuardDevices set VisitorID = {visitorID}, VisitorTemperature = 0, LastUpdated = GETDATE() where DeviceID = '{deviceID}'", connection);
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
        /// Update end time of vistor contact history when visitor is leaving.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <returns>Return success in default.</returns>
        public static Dictionary<string, string> leavingVisitorUpdate(string deviceID, string visitorEmail, int isMaunalUpdate)
        {
            // Get User ID
            var visitorID = getUserID(visitorEmail);
            if (visitorID == 0)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Account not exist."}
                    };
            }

            // Get Guard ID
            var GuardInfo = prepareActivityUpdate(deviceID);
            if (GuardInfo.ContainsKey("result"))
            {
                return GuardInfo;
            }

            var visitorList = new ConcurrentBag<Dictionary<string, string>>();
            try
            {
                visitorList = getCurrentContacts(GuardInfo["ID"]);
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
            }

            var isInside = false;
            foreach(Dictionary<string, string> v in visitorList)
            {
                if (int.Parse(v["Visitor_ID"]) == visitorID)
                {
                    isInside = true;
                }
            }

            if (!isInside)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "User has no entry activity record."}
                    };
            }

            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select top 1 EndTime, ManualUpdate from PersonalContact where ID = {visitorID} and Guard_ID = {GuardInfo["ID"]} order by StartTime desc;", connection);
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

            var check = DataTableToDictionary(ds.Tables[0]);

            if (check.Count == 0 && visitorList.Count != 1)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "User has no entry activity record."}
                    };
            }
            else if (isMaunalUpdate == 1 && check[0]["EndTime"] != null)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Duplicate manual exit record received."}
                    };
            }

            try
            {
                Parallel.Invoke(
                    () =>
                    {
                        DataSet ds = new DataSet();

                        Parallel.ForEach(visitorList, (v) =>
                        {
                            using (SqlConnection connection = new SqlConnection(connectionstring))
                            {
                                if (visitorID == int.Parse(v["Visitor_ID"]))
                                    return;

                                try
                                {
                                    connection.Open();
                                    SqlDataAdapter adp = new SqlDataAdapter($"update PersonalContact set EndTime = GETDATE(), ManualUpdate = {isMaunalUpdate} where Guard_ID = {GuardInfo["ID"]} and EndTime is null and ((ID = {GuardInfo["VisitorID"]} and Contact_ID = {v["Visitor_ID"]} and StartTime in (select max(StartTime) from PersonalContact where ID = {GuardInfo["VisitorID"]} and Contact_ID = {v["Visitor_ID"]})) or (ID = {v["Visitor_ID"]} and Contact_ID = {GuardInfo["VisitorID"]} and StartTime in (select max(StartTime) from PersonalContact where ID = {GuardInfo["VisitorID"]} and Contact_ID = {v["Visitor_ID"]})))", connection);
                                    adp.Fill(ds);
                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                                finally
                                {
                                    if (connection.State == ConnectionState.Open)
                                        connection.Close();
                                }
                            }
                        });
                    },
                    () =>
                    {
                        DataSet ds = new DataSet();

                        using (SqlConnection connection = new SqlConnection(connectionstring))
                        {
                            try
                            {
                                connection.Open();
                                SqlDataAdapter adp = new SqlDataAdapter($"delete from CurrentContact where Visitor_ID = {GuardInfo["VisitorID"]} and Guard_ID = {GuardInfo["ID"]}", connection);
                                adp.Fill(ds);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            finally
                            {
                                if (connection.State == ConnectionState.Open)
                                    connection.Close();
                            }
                        }
                    });
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", e.ToString()}
                    };
            }

            return new Dictionary<string, string>
                {
                    {"result","success"}, {"message", "Success."}
                };
        }

        /// <summary>
        /// Check whether there is a visitor waiting for temperature scanning.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <returns>Return true if a visitor is waiting.</returns>
        public static Dictionary<string, string> incomingVisitorDetect(string deviceID)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select VisitorTemperature from GuardDevices where DeviceID = '{deviceID}'", connection);
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
            if (result.Count == 0)
            {
                return new Dictionary<string, string>
                    {
                        {"result","error"}, {"message", "Device not registered."}
                    };
            }

            if (Math.Abs(float.Parse(result[0]["VisitorTemperature"])) <= 0.1)
            {
                return new Dictionary<string, string>
                    {
                        {"result","true"}
                    };
            }
            else
            {
                return new Dictionary<string, string>
                    {
                        {"result","false"}
                    };
            }

        }

        /// <summary>
        /// Update scanned body temperature to device.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <param name="temperature">Visitor's body temperature</param>
        /// <returns>Return success in default, return exception otherwise.</returns>
        public static Dictionary<string, string> visitorTemperatureUpdate(string deviceID, float temperature)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"update GuardDevices set VisitorTemperature = {temperature} where DeviceID = '{deviceID}'", connection);
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

            if (temperature + 1 >= 0.1)
            {
                var check = visitorActivityUpdate(deviceID);
                if (!check["result"].Equals("success"))
                {
                    return check;
                }
            }

            return new Dictionary<string, string>
                {
                    {"result","success"}, {"message", "Success."}
                };
        }

        /// <summary>
        /// Check the visitor who being detected by guard device.
        /// </summary>
        /// <param name="deviceID">Device id</param>
        /// <returns>A dictionary contains visitor health status.</returns>
        public static Dictionary<string, string> visitorInfoCheck(string deviceID)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter adp = new SqlDataAdapter($"select AccountLogin.UserName, HealthStatus.UserStatus, GuardDevices.VisitorTemperature, DATEDIFF(ss, GuardDevices.LastUpdated, GETDATE()) AS LastUpdated from GuardDevices join AccountLogin on GuardDevices.VisitorID = AccountLogin.ID join HealthStatus on GuardDevices.VisitorID = HealthStatus.ID where GuardDevices.DeviceID = '{deviceID}'", connection);
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
                    {"result","error"}, {"message", "Device not registered."}
                };
        }
    }
}
