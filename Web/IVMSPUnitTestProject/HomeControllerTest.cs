using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.controllers;


namespace IVMSPUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IndexTest()
        {
            HomeController hc = new HomeController();
            ViewResult result = hc.Index() as ViewResult;
            Assert.IsNotNull(result); // 判断控制器是否有返回视图
        }
    }
}
