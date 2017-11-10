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
    /// Возвращает задолженность одной суммой
    /// </summary>
    /// <param name="i_lschet">лицевой счет</param>
    /// <returns></returns>
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "GetDebtsTotalFillRow", 
        TableDefinition = "i_lschet int")]
    public static IEnumerable GetDebtsTotal(int i_lschet)
    {
        var client = new ServiceIntegratorClient(new ServiceIntegrator(ServiceUrl.Url));
        string ticket = client.GetAuthorizationTicket(IdentityCredential.User, IdentityCredential.Password);

        var factory = new GetTRICDebtsTotalFactory(client);

        string i_owner = "1032"; // код поставщика: "Тепло Тюмени - филиал ПАО СУЭНКО"

        var response = factory.Make(new object[] { i_owner, i_lschet }, ticket);

        return response.Tables[0].Rows;
    }

    public static void GetDebtsTotalFillRow(Object obj, out SqlInt32 AccountID, out SqlDateTime PeriodStr, out SqlDecimal SumSaldo)
    {
        DataRow row = (DataRow)obj;
        AccountID = row.ToSql<SqlInt32>("AccountID");
        PeriodStr = row.ToSql<SqlDateTime>("PeriodStr");
        SumSaldo = row.ToSql<SqlDecimal>("SumSaldo");
    }
}