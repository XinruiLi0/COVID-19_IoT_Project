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
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult User()
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
        public Dictionary<string, string> checkPatientStatus(string userEmail, string userPassword, string visitorEmail)
        {
            return DatabaseConnector.checkPatientStatus(userEmail, userPassword, visitorEmail);
        }
        
        [HttpPost]
        public Dictionary<string, string> updatePatientStatus(string userEmail, string userPassword, string visitorEmail, float status)
        {
            return DatabaseConnector.updatePatientStatus(userEmail, userPassword, visitorEmail, status);
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

        [HttpPost]
        public Dictionary<int, Dictionary<string, string>> registerGuardDevice(string userEmail, string userPassword)
        {
            return DatabaseConnector.registerGuardDevice(userEmail, userPassword);
        }

        [HttpPost]
        public Dictionary<int, Dictionary<string, string>> deleteGuardDevice(string userEmail, string userPassword, int deviceID)
        {
            return DatabaseConnector.deleteGuardDevice(userEmail, userPassword, deviceID);
        }

        [HttpPost]
        public Dictionary<string, string> guardDeviceChecking(int deviceID, string visitorEmail, float temperature)
        {
            return DatabaseConnector.guardDeviceChecking(deviceID, visitorEmail, temperature);
        }

        [HttpPost]
        public Dictionary<string, string> guardChecking(int deviceID)
        {
            return DatabaseConnector.guardChecking(deviceID);
        }
    }
}
