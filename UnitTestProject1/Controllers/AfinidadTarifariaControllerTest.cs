using BITecnored.Controllers;
using BITecnored.Model;
using BITecnored.Model.DataContract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Http;

namespace BITests.Controllers
{
    [TestClass]
    public class AfinidadTarifariaControllerTest
    {
        private class testManager : AfinidadTarifaria_dbManager
        {
            override
            public List<IdValue> Buscar()
            {
                List<IdValue> list = new List<IdValue>();
                list.Add(new IdValue(0, "TEST OK"));
                return list;
            }

            public bool createdValue = false;
            public bool borrada = false;

            override
            public void Crear(string nombre)
            {
                createdValue = true;
            }

            override
            public bool Eliminar(int id)
            {
                borrada = true;
                return borrada;
            }
        }

        [TestMethod]
        public void GetAfinidadTest()
        {
            AfinidadTarifariaController controller = new AfinidadTarifariaController();
            controller.setManager(new testManager());
            HttpResponseMessage response = controller.Get();
            var readAsStringAsync = response.Content.ReadAsStringAsync();
            Assert.AreEqual("[{\"id\":0,\"value\":\"TEST OK\"}]", readAsStringAsync.Result);
        }

        [TestMethod]
        public void PostAfinidadTest()
        {
            AfinidadTarifariaController controller = new AfinidadTarifariaController();
            controller.setManager(new testManager());
            controller.Post("TEST");
            Assert.IsTrue(((testManager)controller.getManager()).createdValue);
        }

        [TestMethod]
        public void Delete()
        {
            AfinidadTarifariaController controller = new AfinidadTarifariaController();
            controller.setManager(new testManager());
            controller.Delete(0);
            Assert.IsTrue(((testManager)controller.getManager()).borrada);
        }
    }
}
