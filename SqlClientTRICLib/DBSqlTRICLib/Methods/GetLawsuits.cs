using System;
using System.Data;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ClientTRICLib.wsIntegration;
using ClientTRICLib.Common;
using ClientTRICLib;
using System.Collections;
using SqlTRICNS;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Перечень дел из журнала Гелиос
    /// </summary>
    /// <param name="typeLawsuit">1-судебные приказы, 2-иски, 3-претензии</param>
    /// <returns></returns>
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "GetLawsuitsFillRow",
        TableDefinition = "typeLawsuit int")]
    public static IEnumerable GetLawsuits(int typeLawsuit) // 1-судебные приказы, 2-иски, 3-претензии
    {
        var client = new ServiceIntegratorClient(new ServiceIntegrator(ServiceUrl.Url));
        string ticket = client.GetAuthorizationTicket(IdentityCredential.User, IdentityCredential.Password);

        var factory = new GetTRICLawsuitsFactory(client);
        var response = factory.Make(new object[] { typeLawsuit }, ticket);

        return response.Tables[0].Rows;
    }

    public static void GetLawsuitsFillRow(Object obj,
        out SqlInt32 ID, out SqlString AccountID, out SqlString NumberID,
        out SqlString Address, out SqlString FIO,
        out SqlDateTime DateBorn, out SqlDecimal SumSaldo, out SqlDecimal SumTax,
        out SqlDateTime b_period, out SqlDateTime e_period, out SqlDateTime CreateDate,
        out SqlString Status, out SqlString UserName,
        out SqlDecimal SumPaySaldo, out SqlDecimal SumPayTax, out SqlDecimal SumPeni, out SqlDecimal SumPayPeni,
        out SqlString ServiceGroups, out SqlDecimal SumActualDebt, out SqlString OrderNumber)
    {
        DataRow row = (DataRow)obj;
        ID = row.ToSql<SqlInt32>("ID");
        AccountID = row.ToSql<SqlString>("AccountID");
        NumberID = row.ToSql<SqlString>("NumberID");
        Address = row.ToSql<SqlString>("Address");
        FIO = row.ToSql<SqlString>("FIO");
        DateBorn = row.ToSql<SqlDateTime>("DateBorn");
        SumSaldo = row.ToSql<SqlDecimal>("SumSaldo");
        SumTax = row.ToSql<SqlDecimal>("SumTax");
        b_period = row.ToSql<SqlDateTime>("b_period");
        e_period = row.ToSql<SqlDateTime>("e_period");
        CreateDate = row.ToSql<SqlDateTime>("CreateDate");
        Status = row.ToSql<SqlString>("Status");
        UserName = row.ToSql<SqlString>("UserName");
        SumPaySaldo = row.ToSql<SqlDecimal>("SumPaySaldo");
        SumPayTax = row.ToSql<SqlDecimal>("SumPayTax");
        SumPeni = row.ToSql<SqlDecimal>("SumPeni");
        SumPayPeni = row.ToSql<SqlDecimal>("SumPayPeni");
        ServiceGroups = row.ToSql<SqlString>("ServiceGroups");
        SumActualDebt = row.ToSql<SqlDecimal>("SumActualDebt");
        OrderNumber = row.ToSql<SqlString>("OrderNumber");
    }
}