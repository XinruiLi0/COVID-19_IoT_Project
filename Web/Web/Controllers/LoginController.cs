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
        public Dictionary<string, string> userLogin(string userName, string userPassword, int userRole)
        {
            return DatabaseConnector.userLogin(userName, userPassword, userRole);
        }

        [HttpPost]
        public Dictionary<string, string> userRegister(string userName, string userPassword, int userRole)
        {
            return DatabaseConnector.userRegister(userName, userPassword, userRole);
        }
    }
}
