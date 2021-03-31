using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace IVMSPUnitTest
{
    [TestClass]
    public class HomeControllerTest
    {
        private ILogger<Web.Controllers.HomeController> _logger;

        [TestMethod]
        public void IndexTest()
        {
            Web.Controllers.HomeController controller = new Web.Controllers.HomeController(_logger);
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result); 
        }

        [TestMethod]
        public void GuardTest()
        {
            Web.Controllers.HomeController controller = new Web.Controllers.HomeController(_logger);
            ViewResult result = controller.Guard() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GuardListTest()
        {
            Web.Controllers.HomeController controller = new Web.Controllers.HomeController(_logger);
            ViewResult result = controller.GuardList() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DoctorTest()
        {
            Web.Controllers.HomeController controller = new Web.Controllers.HomeController(_logger);
            ViewResult result = controller.Doctor() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void UserTest()
        {
            Web.Controllers.HomeController controller = new Web.Controllers.HomeController(_logger);
            ViewResult result = controller.User() as ViewResult;
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void DoctorData()
        {
            Web.Controllers.HomeController controller = new Web.Controllers.HomeController(_logger);
            ViewResult result = controller.Doctor() as ViewResult;
            Assert.AreEqual("Doctor", result.ViewData["Title"]);
        }

        [TestMethod]
        public void UserData()
        {
            Web.Controllers.HomeController controller = new Web.Controllers.HomeController(_logger);
            String result = controller.checkUserStatus("U1@test","Password1");
            Console.WriteLine(result);
            Assert.IsInstanceOfType(result, typeof(String));
            
        }

        [TestMethod]
        public void GuardData()
        {
            Web.Controllers.HomeController controller = new Web.Controllers.HomeController(_logger);
            String result = controller.getGuardDevices("U2@test", "Password2");
            Console.WriteLine(result);
            Assert.IsInstanceOfType(result, typeof(String));

        }
    }
}
