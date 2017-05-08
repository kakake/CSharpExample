using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using miniCoreFrame.DbProvider;

namespace TestminiCoreFrame
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建Sqlserver对象
            AbstractDbHelper sqldb = CreateDatabase.GetDatabase("SqlServer", "Data Source=192.168.22.42;Initial Catalog=CPSysDB;User ID=sa;pwd=1;");
            object obj = sqldb.GetDataResult("select count(*) from base_user");
            //创建DB2对象
            AbstractDbHelper oledb = CreateDatabase.GetDatabase("IbmDb2", "Provider=IBMDADB2;Database=CPSysDB;Hostname=192.168.22.176;Protocol=TCPIP;Port=50000; Uid=db2inst1;Pwd=db2inst1;");
            object obj2 = oledb.GetDataResult("select count(*) from base_user");
        }
    }
}
