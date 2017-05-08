//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;

namespace miniCoreFrame.DbProvider
{
	/// <summary>
	/// DB2数据库访问对象
	/// </summary>
    public class IbmDB2Helper : AbstractDbHelper
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected IDbConnection connection = null;			//数据库连接
        /// <summary>
        /// 得到数据库自增长ID的SQL语句
        /// </summary>
        protected string GET_IDENTITY = "VALUES IDENTITY_VAL_LOCAL() ";

        private string _connString;

        public override string ConnectionString
        {
            get
            {
                return _connString;
            }
        }

        public override DbCommand GetDbCommand()
        {
            return (DbCommand)connection.CreateCommand();
        }

        public override AbstractDbHelper GetCopy()
        {
            return new IbmDB2Helper(ConnectionString);
        }

        public IbmDB2Helper(string connstr)
        {
            try
            {
                _connString = connstr;
                //获得连接
                OleDbConnection cnn = new OleDbConnection(ConnectionString);
                this.connection = cnn;
            }
            catch (OleDbException e)
            {
                throw new Exception("连接数据库失败！参考：" + e.Message);
            }
        }

        public override void BeginTransaction()
        {
            try
            {
                if (!isInTransaction)
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    transaction = (DbTransaction)connection.BeginTransaction();
                    isInTransaction = true;
                }
                else
                {
                    throw new Exception("您有操作正在进行，请等待！");
                }
            }
            catch (Exception e)
            {
                throw new Exception("事务启动失败，请再试一次！\n" + e.Message);
            }
        }

        public override void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                isInTransaction = false;

                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
            }
            else
            {
                throw new Exception("无可用事务！");
            }
        }

        public override void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                isInTransaction = false;

                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
            }
            else
            {
                throw new Exception("无可用事务！");
            }
        }     

        public override int InsertRecord(IDbCommand cmd)
        {
            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();

                cmd.CommandText = GET_IDENTITY;
                return Convert.ToInt32(cmd.ExecuteScalar());                
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
            }
        }

        public override int InsertRecord(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            OleDbCommand cmd = new OleDbCommand(commandtext);
            if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
            cmd.Connection = (OleDbConnection)connection;
            try
            {
                cmd.ExecuteNonQuery();
                cmd.CommandText = GET_IDENTITY;
                object obj = cmd.ExecuteScalar();
                int identity;
                if (obj == DBNull.Value)
                {
                    identity = 0;
                }
                else
                {
                    identity = Convert.ToInt32(obj);
                }

                return identity;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
            }
        }

        public override DbDataAdapter GetAdapter(IDbCommand cmd)
        {
            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = (OleDbCommand)cmd;
                cmd.Connection = connection;
                if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
                return adapter;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
            }
        }

        public override DbDataAdapter GetAdapter(string commandtext)
        {
            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                OleDbDataAdapter adapter = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand(commandtext);
                adapter.SelectCommand = (OleDbCommand)cmd;
                cmd.Connection = (OleDbConnection)connection;
                if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
                return adapter;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
            }
        }

        public override DataTable GetDataTable(IDbCommand cmd)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            cmd.Connection = this.connection;
            if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;

            OleDbDataAdapter adapter = new OleDbDataAdapter((OleDbCommand)cmd);

            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                adapter.Dispose();
                adapter = null;
            }
        }

        public override DataTable GetDataTable(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            OleDbCommand cmd = new OleDbCommand(commandtext);
            cmd.Connection = (OleDbConnection)this.connection;
            if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;

            OleDbDataAdapter adapter = new OleDbDataAdapter((OleDbCommand)cmd);
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
                adapter.Dispose();
                adapter = null;
            }
        }

        public override DataRow GetDataRow(IDbCommand cmd)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            cmd.Connection = this.connection;				//添加连接
            DataRow r;

            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = (OleDbCommand)cmd;
            if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    r = dt.Rows[0];
                }
                else
                {
                    r = null;
                }
                return r;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction==false) connection.Close();
                cmd.Dispose();
                cmd = null;
                adapter.Dispose();
                adapter = null;
            }
        }

        public override DataRow GetDataRow(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            OleDbCommand cmd = new OleDbCommand(commandtext);
            cmd.Connection = (OleDbConnection)this.connection;				//添加连接
            DataRow r;

            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = (OleDbCommand)cmd;
            if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    r = dt.Rows[0];
                }
                else
                {
                    r = null;
                }
                return r;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
                adapter.Dispose();
                adapter = null;
            }
        }

        public override IDataReader GetDataReader(IDbCommand cmd)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            try
            {
                cmd.Connection = this.connection;
                if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
                System.Data.OleDb.OleDbDataReader reader = (OleDbDataReader)cmd.ExecuteReader();
                return reader;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }

        public override IDataReader GetDataReader(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            OleDbCommand cmd = new OleDbCommand(commandtext);
            cmd.Connection = (OleDbConnection)this.connection;
            if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
            try
            {
                System.Data.OleDb.OleDbDataReader reader = (OleDbDataReader)cmd.ExecuteReader();
                return reader;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                //if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
            }
        }

        public override int DoCommand(IDbCommand cmd)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            try
            {
                int result = 0;
                if (isInTransaction) cmd.Transaction = transaction;
                cmd.Connection = connection;
                result = cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
            }
        }

        public override int DoCommand(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            int result = 0;
            OleDbCommand cmd = new OleDbCommand(commandtext);
            if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
            cmd.Connection = (OleDbConnection)connection;
            try
            {
                result = cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
            }
        }

        public override object GetDataResult(IDbCommand cmd)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            try
            {
                object Result = null;
                if (isInTransaction) cmd.Transaction = transaction;
                cmd.Connection = connection;
                Result = cmd.ExecuteScalar();
                return Result;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
            }
        }

        public override object GetDataResult(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            object Result = null;
            OleDbCommand cmd = new OleDbCommand(commandtext);
            if (isInTransaction) cmd.Transaction = (OleDbTransaction)transaction;
            cmd.Connection = (OleDbConnection)connection;
            try
            {
                Result = cmd.ExecuteScalar();
                return Result;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();
                cmd.Dispose();
                cmd = null;
            }
        }

        public override DataTable GetDataTable(string storeProcedureName, IDbDataParameter[] parameters)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            DataTable dataTable = null;
            DataTable table2;

            try
            {
                IDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                cmd.CommandText = storeProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                SetParameters(cmd, parameters);
                dataTable = this.GetDataTable(cmd);
                ReturnParameters(cmd, parameters);
                cmd.Dispose();
                cmd = null;
                table2 = dataTable;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) connection.Close();
            }
            return table2;
        }

        public override DataRow GetDataRow(string storeProcedureName, IDbDataParameter[] parameters)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            DataRow dataRow = null;
            DataRow row2;

            try
            {
                IDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                cmd.CommandText = storeProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                SetParameters(cmd, parameters);
                dataRow = this.GetDataRow(cmd);
                ReturnParameters(cmd, parameters);
                cmd.Dispose();
                cmd = null;
                row2 = dataRow;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) connection.Close();
            }
            return row2;
        }

        public override int DoCommand(string storeProcedureName, IDbDataParameter[] parameters)
        {

            if (connection.State == ConnectionState.Closed) connection.Open();
            int num2;
            int num = 0;

            try
            {
                IDbCommand cmd = new OleDbCommand();
                cmd.Connection = (OleDbConnection)connection;
                cmd.CommandText = storeProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                SetParameters(cmd, parameters);

                num = this.DoCommand(cmd);
                ReturnParameters(cmd, parameters);
                cmd.Dispose();
                cmd = null;
                num2 = num;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) connection.Close();
            }

            return num2;
        }

        public override object GetDataResult(string storeProcedureName, IDbDataParameter[] parameters)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            object dataResult = null;
            object obj3;

            try
            {
                IDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                cmd.CommandText = storeProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                SetParameters(cmd, parameters);

                dataResult = this.GetDataResult(cmd);
                ReturnParameters(cmd, parameters);
                cmd.Dispose();
                cmd = null;
                obj3 = dataResult;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) connection.Close();
            }
            return obj3;
        }

        private void SetParameters(IDbCommand cmd, IDbDataParameter[] parameters)
        {
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    OleDbParameter para = new OleDbParameter();
                    para.ParameterName = parameters[i].ParameterName;
                    para.DbType = parameters[i].DbType;
                    para.Direction = parameters[i].Direction;
                    para.Size = parameters[i].Size;
                    para.Value = parameters[i].Value;

                    cmd.Parameters.Add(para);
                }
            }
        }

        private void ReturnParameters(IDbCommand cmd, IDbDataParameter[] parameters)
        {
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i].Value = ((OleDbParameter)cmd.Parameters[i]).Value;
                }
            }
        }

        public override DataSet GetDataSet(string storeProcedureName, IDbDataParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public override void CloseDb()
        {
            if (connection.State != ConnectionState.Closed && isInTransaction == false) connection.Close();        
        }
    }
}