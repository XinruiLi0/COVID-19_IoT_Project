using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Newtonsoft.Json;

namespace IVMSPUnitTest
{
    [TestClass]
    public class HomeController
    {
        private ILogger<Web.Controllers.HomeController> _logger;

        [TestMethod]
        public void Index()
        {
            Web.Controllers.HomeController controllor = new Web.Controllers.HomeController(_logger);
            ViewResult result = controllor.Index() as ViewResult;
            Assert.IsNotNull(result); 
        }
    }
}
