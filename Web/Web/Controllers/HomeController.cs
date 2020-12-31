using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Doctor()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Guard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public Dictionary<string, string> checkVisitorStatus(string userEmail, string userPassword, int userRole, string visitorEmail)
        {
            return DatabaseConnector.checkVisitorStatus(userEmail, userPassword, userRole, visitorEmail);
        }
        
        [HttpPost]
        public Dictionary<string, string> updatePatientStatus(string userEmail, string userPassword, string visitorID, float status)
        {
            return DatabaseConnector.updatePatientStatus(userEmail, userPassword, visitorID, status);
        }

        [HttpPost]
        public Dictionary<string, string> checkUserStatus(string userEmail, string userPassword)
        {
            return DatabaseConnector.checkUserStatus(userEmail, userPassword);
        }

        [HttpPost]
        public Dictionary<string, string> abnormalBodyTrmperatureAlert(string userEmail, string userPassword, string visitorEmail)
        {
            return DatabaseConnector.abnormalBodyTrmperatureAlert(userEmail, userPassword, visitorEmail);
        }
    }
}
