using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace TestOracleConnection
{
    public class OracleSqlHelper
    {
        private static Database db = null;

        public static DataSet GetDataTable( string commandText)
        {
            try
            {
                DataSet resultDataSet = null;

                db = DatabaseFactory.CreateDatabase();//建立数据库连接

                resultDataSet =db.ExecuteDataSet(CommandType.Text, commandText);//执行

                return resultDataSet;
            }
            catch (Exception ex)
            {
                return null;

                throw ex;
            }
        }
    }
}
