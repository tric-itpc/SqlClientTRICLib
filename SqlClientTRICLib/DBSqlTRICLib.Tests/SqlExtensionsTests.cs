using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlTypes;
using DBSqlTRICLib;
using SqlTRICNS;
using System.Data;
using System.Linq;
using System.Collections;

namespace DBSqlTRICLib.Tests
{
    [TestClass]
    public class SqlExtensionsTests
    {
        [TestMethod]
        public void ToSql_int()
        {
            SqlInt32 source = 512;
            DataTable dt = new DataTable();

            dt.Columns.Add("ID", typeof(int));
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { 1 };

            var res = dr.ToSql<SqlInt32>("ID");

            Assert.IsInstanceOfType(res, typeof(SqlInt32));
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void ToSql_String()
        {
            SqlString source = "";
            DataTable dt = new DataTable();

            dt.Columns.Add("Address", typeof(string));
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { "street1" };

            var res = dr.ToSql<SqlString>("Address");

            Assert.IsInstanceOfType(res, typeof(SqlString));
            Assert.AreEqual("street1", res);
        }

        [TestMethod]
        public void ToSql_DateTime_as_empty_string()
        {
            SqlDateTime source = DateTime.Now;
            DataTable dt = new DataTable();

            dt.Columns.Add("b_period", typeof(string));
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { "" };

            var res = dr.ToSql<SqlDateTime>("b_period");

            Assert.IsInstanceOfType(res, typeof(SqlDateTime));
            Assert.IsTrue(res.IsNull);
        }

        [TestMethod]
        public void ToSql_DateTime_as_datetime()
        {
            SqlDateTime source = DateTime.Now;
            DataTable dt = new DataTable();

            dt.Columns.Add("b_period", typeof(DateTime));
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { new DateTime(2017,1,1) };

            var res = dr.ToSql<SqlDateTime>("b_period");

            Assert.IsInstanceOfType(res, typeof(SqlDateTime));
            Assert.AreEqual(new DateTime(2017,1,1), res);
        }

        [TestMethod]
        public void ToSql_Boolean()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("isMain", typeof(bool));
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { true };

            var res = dr.ToSql<SqlBoolean>("isMain");

            Assert.IsInstanceOfType(res, typeof(SqlBoolean));
            Assert.AreEqual(true, res);
        }

        [TestMethod]
        public void ToSql_Decimal()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("s_money", typeof(decimal));
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { 10.11M };

            var res = dr.ToSql<SqlDecimal>("s_money");

            Assert.IsInstanceOfType(res, typeof(SqlDecimal));
            Assert.AreEqual(10.11M, res);
        }
    }
}
