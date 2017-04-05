using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BITecnored.Controllers
{
    [TestClass]
    public class FacturacionCentrosControllerTest
    {
        [TestMethod]
        public void GetTest() // HttpRequestMessage request, [FromUri]int page, [FromUri]int view_size, [FromUri] bool activos
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void BuscarTest() // HttpRequestMessage request, [FromUri]int page, [FromUri]int view_size, [FromUri] bool activos, [FromBody] CentroBusqueda centro
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void BuscarIncompletosTest() // HttpRequestMessage request, [FromUri]int page, [FromUri]int view_size
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void BuscarTarifasIncompletasTest() // HttpRequestMessage request, [FromUri]int page, [FromUri]int view_size
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void GetAmountTest() // HttpRequestMessage request, [FromUri] bool activos, [FromBody] CentroBusqueda criterios
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void GetAmountIncompletosTest() // HttpRequestMessage request
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void GetAmountTarifasIncompletasTest() // HttpRequestMessage request
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void UnlockTest() // HttpRequestMessage request
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void UpdateTest() // HttpRequestMessage request, [FromBody] CentroFacturable centro
        {
            Assert.IsTrue(false);
        }
    }
}
