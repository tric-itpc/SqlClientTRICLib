using System;
using System.Data;
using System.Data.SqlTypes;

namespace SqlTRICNS
{
    public static class SqlExtensions
    {
        public static T ToSql<T>(this DataRow row, string name) where T : struct
        {
            decimal tmpvalue;

            if (typeof(T) == typeof(SqlInt32))
                return (T)Activator.CreateInstance(typeof(T), new object[] { Convert.ToInt32(row[name]) });
            else
                if (typeof(T) == typeof(SqlString))
                    return (T)Activator.CreateInstance(typeof(T), new object[] { row[name].ToString() });
                else
                    if (typeof(T) == typeof(SqlDateTime))
                        return (T)(!string.IsNullOrEmpty(row[name].ToString()) ?
                                Activator.CreateInstance(typeof(T), new object[] { Convert.ToDateTime(row[name]) }) 
                                :
                                SqlDateTime.Null);
                    else
                        if (typeof(T) == typeof(SqlBoolean))
                            return (T)Activator.CreateInstance(typeof(T), new object[] { Convert.ToBoolean(row[name]) });
                        else
                            if (typeof(T) == typeof(SqlDecimal))
                                return (T)(decimal.TryParse(row[name].ToString(), out tmpvalue) ?
                                    Activator.CreateInstance(typeof(T), new object[] { Convert.ToDecimal(row[name]) })
                                    : 
                                    SqlDecimal.Null);
                            else
                                return (T)Activator.CreateInstance<T>();
        }
    }
}
