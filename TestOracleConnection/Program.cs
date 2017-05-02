using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace TestOracleConnection
{
    class Program
    {
        //需要测试该代码，只要改动sql语句和连接数据库字符串即可。
        static void Main(string[] args)
        {
            object result = OracleDataProvider.TestReadTable("110");

            Console.WriteLine();
        }
    }
}
