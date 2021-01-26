using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public Dictionary<string, string> userLogin(string userEmail, string userPassword, int userRole)
        {
            return DatabaseConnector.userLogin(userEmail, userPassword, userRole);
        }

        [HttpPost]
        public Dictionary<string, string> userRegister(string userName, string userEmail, string userPassword, int age, int hasInfectedBefore)
        {
            return DatabaseConnector.userRegister(userName, userEmail, userPassword, age, hasInfectedBefore);
        }

        [HttpPost]
        public Dictionary<string, string> guardRegister(string guardName, string guardEmail, string guardPassword, string address, float latitude, float longitude)
        {
            return DatabaseConnector.guardRegister(guardName, guardEmail, guardPassword, address, latitude, longitude);
        }

        [HttpPost]
        public Dictionary<string, string> doctorRegister(string userName, string userEmail, string userPassword)
        {
            return DatabaseConnector.doctorRegister(userName, userEmail, userPassword);
        }
    }
}
