
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;

namespace miniCoreFrame.DbProvider
{

    public class CreateDatabase
    {

        public static AbstractDbHelper GetDatabase(string dbType, string connstring)
        {

            AbstractDbHelper _oleDb = null;
            
            switch (dbType)
            {
                case "SqlServer":
                    _oleDb = new SqlServerHelper(connstring);
                    _oleDb.DbType = DatabaseType.SqlServer2005;
                    break;
                case "Oracle":
                    _oleDb = new OracleHelper(connstring);
                    _oleDb.DbType = DatabaseType.Oracle;
                    break;
                case "MySql":
                    _oleDb.DbType = DatabaseType.MySQL;
                    break;
                case "IbmDb2":
                    _oleDb = new IbmDB2Helper(connstring);
                    _oleDb.DbType = DatabaseType.IbmDb2;
                    break;
                default:
                    throw new Exception("系统不支持此数据库类型！");
            }
            return _oleDb;
        }
    }

    /// <summary>
    /// 数据库类别
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>未指定数据库</summary>
        UnKnown,
        /// <summary>IBMDB2数据库</summary>
        IbmDb2,
        /// <summary>SqlServer2000数据库</summary>
        SqlServer2000,
        /// <summary>SqlServer2005数据库</summary>
        SqlServer2005,
        /// <summary>Access数据库</summary>
        MsAccess,
        /// <summary>MySQL数据库</summary>
        MySQL,
        /// <summary>Oracle数据库</summary>
        Oracle,
        /// <summary>
        /// 中间件
        /// </summary>
        MidLinkDB
    }
}