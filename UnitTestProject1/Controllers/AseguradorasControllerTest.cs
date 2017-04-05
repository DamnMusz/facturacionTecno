using BITecnored.Controllers;
using BITecnored.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BITests.Controllers
{
    [TestClass]
    public class AseguradorasControllerTest
    {
        private class testManager : Aseguradoras_dbManager
        {
            override
            public string GetAseguradoras()
            {
                return "OK";
            }
        }

        [TestMethod]
        public void Get()
        {
            AseguradorasController controller = new AseguradorasController();
            controller.setManager(new testManager());
            Assert.AreEqual("OK", controller.Get());
        }
    }
}
