using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace miniCoreFrame.DbProvider
{
    /// <summary>
    /// Oracle
    /// </summary>
    public class SqlServerHelper : AbstractDbHelper
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected DbConnection connection = null;			//数据库连接

        /// <summary>
        /// 数据库对象执行命令
        /// </summary>
        protected DbCommand command = null;

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
            return new SqlCommand();
        }

        public override AbstractDbHelper GetCopy()
        {
            return new SqlServerHelper(ConnectionString);
        }



        public SqlServerHelper(string connstr)
        {
            _connString = connstr;

            SqlConnection cnn = new SqlConnection(connstr);
            this.connection = cnn;
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

        public override int InsertRecord(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            SqlCommand cmd = new SqlCommand(commandtext);
            if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;
            cmd.Connection = (SqlConnection)connection;
            try
            {
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT  @@IDENTITY";
                object obj = cmd.ExecuteScalar();
                return Convert.ToInt32(obj == DBNull.Value ? 0 : obj);
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

        public override int InsertRecord(System.Data.IDbCommand cmd)
        {
            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT  @@IDENTITY";
                object obj = cmd.ExecuteScalar();
                return Convert.ToInt32(obj == DBNull.Value ? 0 : obj);
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
            throw new NotImplementedException();
        }

        public override DbDataAdapter GetAdapter(string commandtext)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetDataTable(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            SqlCommand cmd = new SqlCommand(commandtext);
            cmd.Connection = (SqlConnection)this.connection;
            if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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

        public override DataTable GetDataTable(System.Data.IDbCommand cmd)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            cmd.Connection = this.connection;
            if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;

            SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)cmd);

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

        public override DataTable GetDataTable(string storeProcedureName, IDbDataParameter[] parameters)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            SqlDataAdapter adapter = null;
            DataTable table2 = new DataTable();

            try
            {
                IDbCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = storeProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                SetParameters(cmd, parameters);
                adapter = new SqlDataAdapter((SqlCommand)cmd);
                adapter.Fill(table2);
                ReturnParameters(cmd, parameters);
                cmd.Dispose();
                cmd = null;
            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) connection.Close();
                adapter.Dispose();
                adapter = null;
            }
            return table2;
        }

        public override DataSet GetDataSet(string storeProcedureName, IDbDataParameter[] parameters)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            SqlDataAdapter adapter = null;
            DataSet ds = new DataSet();

            try
            {
                IDbCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = storeProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                SetParameters(cmd, parameters);
                adapter = new SqlDataAdapter((SqlCommand)cmd);
                adapter.Fill(ds);
                ReturnParameters(cmd, parameters);
                cmd.Dispose();
                cmd = null;

            }
            catch (Exception e)
            {
                throw new Exception("操作数据库失败！参考：" + e.Message);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) connection.Close();
                adapter.Dispose();
                adapter = null;
            }
            return ds;
        }

        public override DataRow GetDataRow(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            SqlCommand cmd = new SqlCommand(commandtext);
            cmd.Connection = (SqlConnection)this.connection;				//添加连接
            DataRow r;

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = (SqlCommand)cmd;
            if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;
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

        public override DataRow GetDataRow(System.Data.IDbCommand cmd)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            cmd.Connection = this.connection;				//添加连接
            DataRow r;

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = (SqlCommand)cmd;
            if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;
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

        public override DataRow GetDataRow(string storeProcedureName, IDbDataParameter[] parameters)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            DataRow dataRow = null;
            DataRow row2;

            try
            {
                IDbCommand cmd = new SqlCommand();
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

        public override IDataReader GetDataReader(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            SqlCommand cmd = new SqlCommand(commandtext);
            cmd.Connection = (SqlConnection)this.connection;
            if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;
            try
            {
                SqlDataReader reader = (SqlDataReader)cmd.ExecuteReader();
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

        public override IDataReader GetDataReader(System.Data.IDbCommand cmd)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            try
            {
                cmd.Connection = this.connection;
                if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;
                SqlDataReader reader = (SqlDataReader)cmd.ExecuteReader();
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

        public override int DoCommand(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            int result = 0;
            SqlCommand cmd = new SqlCommand(commandtext);
            if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;
            cmd.Connection = (SqlConnection)connection;
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

        public override int DoCommand(System.Data.IDbCommand cmd)
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

        public override int DoCommand(string storeProcedureName, IDbDataParameter[] parameters)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
            int num2;
            int num = 0;

            try
            {
                IDbCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)connection;
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

        public override object GetDataResult(string commandtext)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            object Result = null;
            SqlCommand cmd = new SqlCommand(commandtext);
            if (isInTransaction) cmd.Transaction = (SqlTransaction)transaction;
            cmd.Connection = (SqlConnection)connection;
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

        public override object GetDataResult(System.Data.IDbCommand cmd)
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

        public override object GetDataResult(string storeProcedureName, IDbDataParameter[] parameters)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();

            object dataResult = null;
            object obj3;

            try
            {
                IDbCommand cmd = new SqlCommand();
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
                    SqlParameter para = new SqlParameter();
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
                    parameters[i].Value = ((SqlParameter)cmd.Parameters[i]).Value;
                }
            }
        }
    }
}
