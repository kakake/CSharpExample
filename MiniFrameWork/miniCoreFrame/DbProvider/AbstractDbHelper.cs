//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================
using System;
using System.Data;
using System.Data.Common;

namespace miniCoreFrame.DbProvider
{
	/// <summary>
	/// RelationalDatabase 的摘要说明。
	/// </summary>
    [Serializable]
    public abstract class AbstractDbHelper
	{
		
		/// <summary>
		/// 数据库事务
		/// </summary>
		protected DbTransaction transaction=null;			//数据库事务
		/// <summary>
		/// 是否在事务中	
		/// </summary>
		protected bool isInTransaction=false;				//是否在事务中	


        #region 属性
        /// <summary>
        /// 返回数据库连接字符串
        /// </summary>
        public abstract string ConnectionString { get; }

        /// <summary>
        /// 返回是否处于事务中
        /// </summary>
        public bool IsInTransaction
        {
            get { return this.isInTransaction; }
        }

        public DatabaseType DbType { get; set; }

        #endregion


        /// <summary>
        /// 得到Command
        /// </summary>
        /// <returns></returns>
        public abstract DbCommand GetDbCommand();

        /*抽象方法*/
        /// <summary>
        /// 返回一个RelationalDatabase 
        /// </summary>
        /// <returns></returns>
        public abstract AbstractDbHelper GetCopy();
		
		/// <summary>
		/// 关系型数据库访问对象
		/// </summary>
        public AbstractDbHelper()
        {

        }
		
		/// <summary>
		/// 启动一个事务
		/// </summary>
        public abstract void BeginTransaction();

		/// <summary>
		/// 提交一个事务
		/// </summary>
        public abstract void CommitTransaction();
		/// <summary>
		/// 回滚一个事务
		/// </summary>
        public abstract void RollbackTransaction();

        public virtual void CloseDb() { }
        

		#region 执行插入一条记录 适用于有 自动生成标识的列
        public abstract int InsertRecord(IDbCommand cmd);
        public abstract int InsertRecord(string commandtext);
		#endregion

		#region 返回一个IDataAdpter 
		/// <summary>
		///  返回一个IDataAdpter 
		/// </summary>
		/// <param name="cmd">IDbCommand对象</param>
		/// <returns></returns>
		public abstract DbDataAdapter GetAdapter(IDbCommand cmd);
		/// <summary>
		///  返回一个IDataAdpter 
		/// </summary>
		/// <param name="commandtext">SQL语句字符串</param>
		/// <returns></returns>
		public abstract DbDataAdapter GetAdapter(string commandtext);

		#endregion

		#region 返回一个DataTable
		/// <summary>
		/// 返回一个DataTable
		/// </summary>
		/// <param name="cmd">IDbCommand对象</param>
		/// <returns></returns>
		public abstract DataTable GetDataTable(IDbCommand cmd);
		/// <summary>
		/// 返回一个DataTable
		/// </summary>
		/// <param name="commandtext">SQL语句字符串</param>
		/// <returns></returns>
		public abstract DataTable GetDataTable(string commandtext);

        public abstract DataTable GetDataTable(string storeProcedureName, IDbDataParameter[] parameters);

        public abstract DataSet GetDataSet(string storeProcedureName, IDbDataParameter[] parameters);
		#endregion

		#region 返回一个DataRow
		/// <summary>
		/// 返回一个DataRow
		/// </summary>
		/// <param name="cmd">IDbCommand对象</param>
		/// <returns></returns>
		public abstract DataRow GetDataRow(IDbCommand cmd);
		/// <summary>
		/// 返回一个DataRow
		/// </summary>
		/// <param name="commandtext">SQL语句字符串</param>
		/// <returns></returns>
		public abstract DataRow GetDataRow(string commandtext);

        public abstract DataRow GetDataRow(string storeProcedureName, IDbDataParameter[] parameters);
		#endregion

		#region 返回一个DataReader
		/// <summary>
		/// 返回一个DataReader
		/// </summary>
		/// <param name="cmd">IDbCommand对象</param>
		/// <returns></returns>
		public abstract IDataReader GetDataReader(IDbCommand cmd);
		/// <summary>
		/// 返回一个DataReader
		/// </summary>
		/// <param name="commandtext">SQL语句字符串</param>
		/// <returns></returns>
		public abstract IDataReader GetDataReader(string commandtext);
		#endregion

		#region 执行一个语句，返回执行情况
		/// <summary>
		/// 执行一个语句，返回执行情况
		/// </summary>
		/// <param name="cmd">IDbCommand对象</param>
		/// <returns></returns>
		public abstract int DoCommand(IDbCommand cmd);
		/// <summary>
		/// 执行一个语句，返回执行情况
		/// </summary>
		/// <param name="commandtext">SQL语句字符串</param>
		/// <returns></returns>
		public abstract int DoCommand(string commandtext);

        public abstract int DoCommand(string storeProcedureName, IDbDataParameter[] parameters);
		#endregion

		#region 执行一个命令返回一个数据结果
		/// <summary>
		/// 执行一个命令返回一个数据结果
		/// </summary>
		/// <param name="cmd">IDbCommand对象</param>
		/// <returns></returns>
		public abstract object GetDataResult(IDbCommand cmd);
		/// <summary>
		/// 执行一个命令返回一个数据结果
		/// </summary>
		/// <param name="commandtext">SQL语句字符串</param>
		/// <returns></returns>
		public abstract object GetDataResult(string commandtext);

        public abstract object GetDataResult(string storeProcedureName, IDbDataParameter[] parameters);
		     
        #endregion

    }



}
