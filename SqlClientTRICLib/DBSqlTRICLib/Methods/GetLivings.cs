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
    /// Возвращает зарегистрированных граждан по указанному лс и периоду 
    /// </summary>
    /// <param name="accountID">лицевой счет</param>
    /// <param name="b_period">начало периода</param>
    /// <param name="e_period">конец периода</param>
    /// <returns></returns>
    [Microsoft.SqlServer.Server.SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "GetLivingsFillRow", 
        TableDefinition = "accountID nvarchar(10), b_period int, e_period int")]
    public static IEnumerable GetLivings(string accountID, int b_period, int e_period)
    {
        var client = new ServiceIntegratorClient(new ServiceIntegrator(ServiceUrl.Url));
        string ticket = client.GetAuthorizationTicket(IdentityCredential.User, IdentityCredential.Password);

        var factory = new GetTRICLivingsFactory(client);

        var xmlRoot = new XmlBuilder("Accounts");

        var accountElement = new XmlBuilder("Account");

        accountElement.AddAttribute("accountID", accountID);
        accountElement.AddAttribute("b_period", b_period);
        accountElement.AddAttribute("e_period", e_period);

        xmlRoot.AddElement(accountElement.Build());

        string xmlAccounts = xmlRoot.Build().ToString();

        var response = factory.Make(new object[] { xmlAccounts, 3 }, ticket);

        return response.Tables[0].Rows;
    }

    public static void GetLivingsFillRow(Object obj, out SqlInt32 i_people, out SqlInt32 cod_pl, 
        out SqlString FIO, out SqlDateTime d_born, out SqlBoolean isMain,
        out SqlString PlaceBorn, out SqlDateTime b_period, out SqlDateTime e_period, out SqlString periodName)
    {
        DataRow row = (DataRow)obj;
        i_people = row.ToSql<SqlInt32>("i_people");
        cod_pl = row.ToSql<SqlInt32>("cod_pl");
        FIO = row.ToSql<SqlString>("FIO");
        d_born = row.ToSql<SqlDateTime>("d_born");
        isMain = row.ToSql<SqlBoolean>("isMain");
        PlaceBorn = row.ToSql<SqlString>("PlaceBorn");
        b_period = row.ToSql<SqlDateTime>("b_period");
        e_period = row.ToSql<SqlDateTime>("e_period");
        periodName = row.ToSql<SqlString>("periodName");
    }
}