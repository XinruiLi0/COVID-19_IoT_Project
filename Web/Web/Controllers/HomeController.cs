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
        public Dictionary<int, Dictionary<string, string>> getGuardDevices(string userEmail, string userPassword)
        {
            return DatabaseConnector.getGuardDevices(userEmail, userPassword);
        }

        [HttpPost]
        public Dictionary<int, Dictionary<string, string>> registerGuardDevice(string userEmail, string userPassword, string deviceID, string deviceDescription)
        {
            return DatabaseConnector.registerGuardDevice(userEmail, userPassword, deviceID, deviceDescription);
        }

        [HttpPost]
        public Dictionary<int, Dictionary<string, string>> deleteGuardDevice(string userEmail, string userPassword, string deviceID)
        {
            return DatabaseConnector.deleteGuardDevice(userEmail, userPassword, deviceID);
        }

        [HttpPost]
        public Dictionary<string, string> visitorDetect(string deviceID, string visitorEmail)
        {
            return DatabaseConnector.visitorDetect(deviceID, visitorEmail);
        }

        [HttpPost]
        public Dictionary<string, string> visitorTemperatureUpdate(string deviceID, float temperature)
        {
            return DatabaseConnector.visitorTemperatureUpdate(deviceID, temperature);
        }

        [HttpPost]
        public Dictionary<string, string> visitorInfoCheck(string deviceID)
        {
            return DatabaseConnector.visitorInfoCheck(deviceID);
        }

    }
}
