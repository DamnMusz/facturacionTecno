using BITecnored.Model.DB.Querys;
using BITecnored.Model.SLA;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BITecnored.Model.DB.Querys
{
    [TestClass]
    public class InsertTest
    {
        [TestMethod]
        public void ToStringTest()
        {
            List<string> columns = new List<string>();
            columns.Add("first");
            columns.Add("second");
            columns.Add("third");
            Insert query = new Insert("example_table");
            query.AddColumns(columns);

            List<string> value1 = new List<string>();
            value1.Add("one");
            value1.Add("two");
            value1.Add("three");

            List<string> value2 = new List<string>();
            value2.Add("I");
            value2.Add("II");
            value2.Add("III");

            query.AddValues(value1);
            query.AddValues(value2);

            Assert.AreEqual("INSERT INTO example_table (first, second, third) VALUES (one, two, three), (I, II, III);", query.ToString());
        }

        [TestMethod]
        public void GetBaseTest()
        {
            new BaseSLAGenerator().GetBase(new DateTime(2016,01,01));
        }
    }
}
