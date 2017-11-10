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
    /// Возвращает задолженность за период в разрезе услуг
    /// </summary>
    /// <param name="accountID">Лицевой счет</param>
    /// <param name="b_period">начало периода</param>
    /// <param name="e_period">конец периода</param>
    /// <returns></returns>
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "GetDebtsByServicesFillRow",
        TableDefinition = "accountID nvarchar(10), b_period datetime, e_period datetime")]
    public static IEnumerable GetDebtsByServices(string accountID, DateTime b_period, DateTime e_period)
    {
        int i_owner = 1032; // код поставщика, выдается администратором системы

        var client = new ServiceIntegratorClient(new ServiceIntegrator(ServiceUrl.Url));
        string ticket = client.GetAuthorizationTicket(IdentityCredential.User, IdentityCredential.Password);

        var factory = new GetTRICDebtsByServicesFactory(client);

        var xmlRoot = new XmlBuilder("Accounts");

        var accountElement = new XmlBuilder("Account");

        accountElement.AddAttribute("i_lschet", accountID);
        string b_period_str = b_period.ToString("dd.MM.yyyy");
        string e_period_str = e_period.ToString("dd.MM.yyyy");
        accountElement.AddAttribute("b_period", b_period_str);
        accountElement.AddAttribute("e_period", e_period_str);

        xmlRoot.AddElement(accountElement.Build());

        string xmlAccounts = xmlRoot.Build().ToString();

        var response = factory.Make(new object[] { i_owner, xmlAccounts }, ticket);

        return response.Tables[0].Rows;
    }

    public static void GetDebtsByServicesFillRow(Object obj,
        out SqlInt32 i_lschet,
        out SqlString period, 
        out SqlDecimal sum_nach_11,
        out SqlDecimal sum_odn_11,
        out SqlDecimal sum_opl_11,

        out SqlDecimal sum_nach_8,
        out SqlDecimal sum_odn_8,
        out SqlDecimal sum_opl_8,

        out SqlDecimal sum_nach_10,
        out SqlDecimal sum_odn_10,
        out SqlDecimal sum_opl_10,

        out SqlDecimal sum_nach_9,
        out SqlDecimal sum_odn_9,
        out SqlDecimal sum_opl_9,

        out SqlDecimal sum_total_nach,
        out SqlDecimal sum_total_opl,
        out SqlDecimal sum_total,
        out SqlInt32 orderby
        )
    {
        DataRow row = (DataRow)obj;
        i_lschet = row.ToSql<SqlInt32>("i_lschet");
        period = row.ToSql<SqlString>("period");
        
        sum_nach_11 = row.ToSql<SqlDecimal>("sum_nach_11");
        sum_odn_11 = row.ToSql<SqlDecimal>("sum_odn_11");
        sum_opl_11 = row.ToSql<SqlDecimal>("sum_opl_11");

        sum_nach_8 = row.ToSql<SqlDecimal>("sum_nach_8");
        sum_odn_8 = row.ToSql<SqlDecimal>("sum_odn_8");
        sum_opl_8 = row.ToSql<SqlDecimal>("sum_opl_8");

        sum_nach_10 = row.ToSql<SqlDecimal>("sum_nach_10");
        sum_odn_10 = row.ToSql<SqlDecimal>("sum_odn_10");
        sum_opl_10 = row.ToSql<SqlDecimal>("sum_opl_10");

        sum_nach_9 = row.ToSql<SqlDecimal>("sum_nach_9");
        sum_odn_9 = row.ToSql<SqlDecimal>("sum_odn_9");
        sum_opl_9 = row.ToSql<SqlDecimal>("sum_opl_9");

        sum_total_nach = row.ToSql<SqlDecimal>("sum_total_nach");
        sum_total_opl = row.ToSql<SqlDecimal>("sum_total_opl");
        sum_total = row.ToSql<SqlDecimal>("sum_total");
        orderby = row.ToSql<SqlInt32>("orderby");
    }
}