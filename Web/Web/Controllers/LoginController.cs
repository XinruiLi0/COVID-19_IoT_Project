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
        public Dictionary<string, string> userRegister(string userName, string userEmail, string userPassword, int userRole)
        {
            return DatabaseConnector.userRegister(userName, userEmail, userPassword, userRole);
        }
    }
}
