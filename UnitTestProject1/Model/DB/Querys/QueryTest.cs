using BITecnored.Model.DB.Querys;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BITecnored.Model.DB.Querys
{
    [TestClass]
    public class QueryTest
    {
        [TestMethod]
        public void ToStringTest()
        {
            Query query = new Query();
            query
                .AddColumn("column")
                .AddFrom("from")
                .AddCriterio(new SQL_Criteria(CriteriaValue.CriteriaType.AND, new Operator("a", Operator.Operator_Type.EQUALS, "b")));
            Assert.AreEqual("SELECT column FROM from WHERE 1=1 AND (a = b)", query.ToString());
        }
    }
}
