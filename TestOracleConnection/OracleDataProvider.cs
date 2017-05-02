using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

namespace TestOracleConnection
{
    public class OracleDataProvider
    {
        public static DataTable  TestReadTable(string id)
        {
            DataTable dataTable = null;

            try
            {
                string commandText = "select * from Books t";

                DataSet resultTable = OracleSqlHelper.GetDataTable(commandText);

                int table = resultTable != null ? resultTable.Tables.Count : 0;

                if (table > 0)
                {
                    dataTable = resultTable.Tables[0];
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return dataTable;
        }
    }
}
