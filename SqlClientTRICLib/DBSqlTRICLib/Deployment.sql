ALTER DATABASE asup SET TRUSTWORTHY ON
ALTER DATABASE TESTDB SET TRUSTWORTHY ON

/* УБРАТЬ ВСЕ ЗАРЕГИСТРИРОВАННЫЕ Функции и сборки
drop function [dbo].GetDebtsByServices
GO
drop function [dbo].GetDebtsByPeriod
GO
drop FUNCTION [dbo].GetDebtsTotal
GO
drop FUNCTION [dbo].[GetLawsuits]
GO
drop FUNCTION [dbo].[GetLivings]
GO
drop ASSEMBLY [SqlTRICAssembly]
GO
drop ASSEMBLY ClientTRICLib
GO
*/

-- регистрация библиотеки доступа через веб-сервис к БД Гелиос
CREATE ASSEMBLY ClientTRICLib FROM 'c:\ClientTRICLib\ClientTRICLib.dll' WITH PERMISSION_SET = EXTERNAL_ACCESS;--SAFE;  
GO
-- регистрация CLR-сборки, работающей через ClientTRICLib и предоставляющей SQL-функции
CREATE ASSEMBLY [SqlTRICAssembly] 
AUTHORIZATION [dbo]
FROM 'c:\ClientTRICLib\SqlTRICAssembly.dll' 
WITH PERMISSION_SET = EXTERNAL_ACCESS;-- UNSAFE;  

GO

-- ф-ия возвращает зарегистрированных
CREATE FUNCTION [dbo].[GetLivings](@accountID NVARCHAR(10), @b_period INT, @e_period INT)
RETURNS TABLE(i_people int, cod_pl int, FIO nvarchar(max), d_born datetime, isMain bit, PlaceBorn nvarchar(max), 
		b_period datetime, e_period datetime, periodName nvarchar(max)) 
AS
EXTERNAL NAME [SqlTRICAssembly].[UserDefinedFunctions].[GetLivings]
 
GO
-- ф-ия возвращает журнал дел, параметр @typeLawsuit: 1-судебные приказы, 2-иски, 3-претензии
CREATE FUNCTION [dbo].[GetLawsuits](@typeLawsuit INT)
RETURNS TABLE([ID] [int], 
	[AccountID] [nvarchar](max),
	[NumberID] [nvarchar](max),
	[Address] [nvarchar](max),
	[FIO] [nvarchar](max),
	[DateBorn] [datetime],
	[SumSaldo] [decimal](18, 4),
	[SumTax] [decimal](18, 4),
	[B_period] [datetime],
	[E_period] [datetime],
	[CreateDate] [datetime],
	[Status] [nvarchar](max),
	[UserName] [nvarchar](max),
	[SumPaySaldo] [decimal](18, 4),
	[SumPayTax]  [decimal](18, 4),
	[SumPeni] [decimal](18, 4),
	[SumPayPeni]  [decimal](18, 4),
	[ServiceGroups] [nvarchar](max),
	[SumActualDebt] [decimal](18, 4),
	[OrderNumber] [nvarchar](max)) 
AS
EXTERNAL NAME [SqlTRICAssembly].[UserDefinedFunctions].[GetLawsuits]
GO

-- возвращает задолженность одной суммой
CREATE FUNCTION [dbo].GetDebtsTotal(@i_lschet INT)
RETURNS TABLE(AccountID int, PeriodStr datetime, SumSaldo [decimal](18, 4)) 
AS
EXTERNAL NAME [SqlTRICAssembly].[UserDefinedFunctions].GetDebtsTotal
 
GO

-- возвращает задолженности за период в разрезе месяцев
CREATE FUNCTION [dbo].GetDebtsByPeriod(@accountID NVARCHAR(10), @b_period DateTime, @e_period DateTime)
RETURNS TABLE(
	cod_pl int, for_period int, s_money  [decimal](18, 4), Address [nvarchar](max),
	FIO [nvarchar](max),
	i_house int,
	d_born datetime
) 
AS
EXTERNAL NAME [SqlTRICAssembly].[UserDefinedFunctions].GetDebtsByPeriod
 
GO

-- возвращает задолженности за период в разрезе месяцев и услуг
CREATE FUNCTION [dbo].GetDebtsByServices(@accountID NVARCHAR(10), @b_period DateTime, @e_period DateTime)
RETURNS TABLE(
	i_lschet int, period [nvarchar](max), 
	sum_nach_11 decimal(18, 4), sum_odn_11 decimal(18, 4), sum_opl_11 decimal(18, 4),
    sum_nach_8 decimal(18, 4), sum_odn_8 decimal(18, 4), sum_opl_8 decimal(18, 4),
    sum_nach_10 decimal(18, 4), sum_odn_10 decimal(18, 4), sum_opl_10 decimal(18, 4),
    sum_nach_9 decimal(18, 4), sum_odn_9 decimal(18, 4), sum_opl_9 decimal(18, 4),
    sum_total_nach decimal(18, 4), sum_total_opl decimal(18, 4), sum_total decimal(18, 4),
    orderby int
) 
AS
EXTERNAL NAME [SqlTRICAssembly].[UserDefinedFunctions].GetDebtsByServices
 
GO

/* ПРИМЕРЫ ПРОВЕРКИ РАБОТОСПОСОБНОСТИ */
select * from [dbo].[GetLawsuits](1)

set dateformat dmy
select * from [dbo].[GetLivings]('7777777', 150, 250)

select * from dbo.GetDebtsTotal(11816105)

set dateformat dmy
select * from [dbo].GetDebtsByPeriod('11816105', '01/12/2014', '01/10/2016')

set dateformat dmy
select * from [dbo].GetDebtsByServices('11789150', '01/10/2012', '31/01/2017')
/* КОНЕЦ ПРОВЕРКИ РАБОТОСПОСОБНОСТИ */