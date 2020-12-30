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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public Dictionary<string, string> checkVisitorStatus(string visitorEmail, int userRole)
        {
            return DatabaseConnector.checkVisitorStatus(visitorEmail, userRole);
        }
        
        [HttpPost]
        public Dictionary<string, string> UpdatePatientStatus(string userEmail, string userPassword, string visitorID, float status)
        {
            return DatabaseConnector.UpdatePatientStatus(userEmail, userPassword, visitorID, status);
        }

    }
}
