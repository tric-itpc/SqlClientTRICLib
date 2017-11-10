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
    /// Возвращает задолженность за период в разрезе месяцев
    /// </summary>
    /// <param name="accountID">Лицевой счет</param>
    /// <param name="b_period">начало периода</param>
    /// <param name="e_period">конец периода</param>
    /// <returns></returns>
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "GetDebtsByPeriodFillRow",
        TableDefinition = "accountID nvarchar(10), b_period datetime, e_period datetime")]
    public static IEnumerable GetDebtsByPeriod(string accountID, DateTime b_period, DateTime e_period)
    {
        int i_owner = 1032; // код поставщика, выдается администратором системы

        var client = new ServiceIntegratorClient(new ServiceIntegrator(ServiceUrl.Url));
        string ticket = client.GetAuthorizationTicket(IdentityCredential.User, IdentityCredential.Password);

        var factory = new GetTRICDebtsByPeriodFactory(client);

        var xmlRoot = new XmlBuilder("Accounts");

        var accountElement = new XmlBuilder("Account");

        accountElement.AddAttribute("accountID", accountID);
        string b_period_str = b_period.ToString("dd.MM.yyyy");
        string e_period_str = e_period.ToString("dd.MM.yyyy");
        accountElement.AddAttribute("b_period", b_period_str);
        accountElement.AddAttribute("e_period", e_period_str);

        xmlRoot.AddElement(accountElement.Build());

        string xmlAccounts = xmlRoot.Build().ToString();

        var response = factory.Make(new object[] { i_owner, xmlAccounts }, ticket);

        return response.Tables[0].Rows;
    }

    public static void GetDebtsByPeriodFillRow(Object obj, 
        out SqlInt32 cod_pl,
        out SqlInt32 for_period, out SqlDecimal s_money,
        out SqlString Address, out SqlString FIO,
        out SqlInt32 i_house, 
        out SqlDateTime d_born)
    {
        DataRow row = (DataRow)obj;
        cod_pl = row.ToSql<SqlInt32>("cod_pl");
        for_period = row.ToSql<SqlInt32>("for_period");
        s_money = row.ToSql<SqlDecimal>("s_money");
        Address = row.ToSql<SqlString>("Address");
        FIO = row.ToSql<SqlString>("FIO");
        i_house = row.ToSql<SqlInt32>("i_house");
        d_born = row.ToSql<SqlDateTime>("d_born");
    }
}