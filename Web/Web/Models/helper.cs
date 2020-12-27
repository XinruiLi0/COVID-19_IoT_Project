using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Web.Models
{
    public class helper
    {
        public string GetRDSConnectionString(IConfiguration _configuration)
            {

                string dbname = _configuration.GetSection("RDS_DB_NAME").Value.ToString();

                if (string.IsNullOrEmpty(dbname)) return null;
            
                string username = _configuration.GetSection("RDS_USERNAME").Value.ToString();
                string password = _configuration.GetSection("RDS_PASSWORD").Value.ToString();
                string hostname = _configuration.GetSection("RDS_HOSTNAME").Value.ToString();
                string port = _configuration.GetSection("RDS_PORT").Value.ToString();

                return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";Persist Security Info=True;User ID=" + username + ";Password=" + password + ";";
            }

        public String ConnectionTest(IConfiguration _configuration)
        {
            String res = null;
            SqlDataReader dataReader;
            SqlConnection sq = new SqlConnection();
            sq.ConnectionString = GetRDSConnectionString(_configuration);
            try { 
                sq.Open();
                String cho = "INSERT INTO dbo.test VALUES ('3', 'TRUMP', '1.1')";
                SqlCommand sc = new SqlCommand(cho, sq);
                dataReader = sc.ExecuteReader();
                while (dataReader.Read())
                {
                    res = res + dataReader.GetValue(0)+ dataReader.GetValue(1)+ dataReader.GetValue(2);
                }
                dataReader.Close();
                sc.Dispose();
                sq.Close();
                return res;
            }
            catch(Exception e)
            {
                e.ToString();
                return e.ToString();
            }
            
        }
    
    }

    }
