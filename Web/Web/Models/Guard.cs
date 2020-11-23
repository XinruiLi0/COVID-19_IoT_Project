using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class Guard
    {
        public string name;
        private string password;
        private string area;

        private string Getarea()
        {
            return area;
        }

        private void Setarea(string value)
        {
            area = value;
        }

        public string Getpassword()
        {
            return password;
        }

        public void Setpassword(string value)
        {
            password = value;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }

        
        
    }
}
