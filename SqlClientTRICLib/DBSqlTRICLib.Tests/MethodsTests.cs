using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Data;

namespace DBSqlTRICLib.Tests
{
    [TestClass]
    public class MethodsTests
    {
        [TestMethod]
        public void GetLivings()
        {
            var res = UserDefinedFunctions.GetLivings("7777777", 150, 250);

            Assert.IsInstanceOfType(res, typeof(IEnumerable));
            Assert.AreEqual(6, (res as DataRowCollection).Count);
        }

        [TestMethod]
        public void GetLawsuits()
        {
            var res = UserDefinedFunctions.GetLawsuits(2);

            Assert.IsInstanceOfType(res, typeof(IEnumerable));
            Assert.IsTrue((res as DataRowCollection).Count > 0);
        }

        [TestMethod]
        public void GetDebtsTotal()
        {
            int i_lschet = 11816105; // номер лицевого счета
            var res = UserDefinedFunctions.GetDebtsTotal(i_lschet);

            Assert.IsInstanceOfType(res, typeof(IEnumerable));
            Assert.IsTrue((res as DataRowCollection).Count == 1);
        }

        [TestMethod]
        public void GetDebtsByPeriod()
        {
            var res = UserDefinedFunctions.GetDebtsByPeriod("11816105", new DateTime(2004, 12, 1), new DateTime(2016, 10, 1));

            Assert.IsInstanceOfType(res, typeof(IEnumerable));
            Assert.IsTrue((res as DataRowCollection).Count > 1);
        }

        [TestMethod]
        public void GetDebtsByServices()
        {
            var res = UserDefinedFunctions.GetDebtsByServices("11789150", new DateTime(2012, 10, 1), new DateTime(2017, 1, 31));

            Assert.IsInstanceOfType(res, typeof(IEnumerable));
            Assert.IsTrue((res as DataRowCollection).Count > 0);
        }
        
    }
}
