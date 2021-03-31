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
    public class LoginControllerTest
    {
        [TestMethod]
        public void LoginTest()
        {
            Web.Controllers.LoginController controller = new Web.Controllers.LoginController();
            ViewResult result = controller.Login() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SignupTest()
        {
            Web.Controllers.LoginController controller = new Web.Controllers.LoginController();
            ViewResult result = controller.Signup() as ViewResult;
            Assert.IsNotNull(result); 
        }

        [TestMethod]
        public void LoginDbTest()
        {
            Web.Controllers.LoginController controller = new Web.Controllers.LoginController();
            Dictionary<string, string> result = controller.userLogin("U2@test", "Password2", 2);
            Console.WriteLine(result);
            Assert.IsInstanceOfType(result, typeof(Dictionary<string, string>));

        }
    }
}
