using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BITest.Controllers
{
    [TestClass]
    public abstract class AbstractLongProcessControllerTest
    {
        [TestMethod]
        public void Get() // HttpRequestMessage request, [FromUri] int mode, [FromUri] string param
        {
            Assert.IsTrue(false);
        }
        [TestMethod]
        protected void QueryState()
        {
            Assert.IsTrue(false);
        }
        [TestMethod]
        protected void QueryResult()
        {
            Assert.IsTrue(false);
        }
        [TestMethod]
        protected void ResetState()
        {
            Assert.IsTrue(false);
        }
    }
}
