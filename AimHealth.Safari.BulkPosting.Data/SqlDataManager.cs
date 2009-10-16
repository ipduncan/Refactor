using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.ObjectModel;
using AimHealth.Safari.BulkPosting.Data;

namespace DataAccessBulkManager
{

    public class SqlDataManager: IDataManager
    {
        private SqlConnection _conn;
        private SqlConnection _connTrans;
        static readonly SqlDataManager manager = new SqlDataManager();


        SqlDataManager()
        {
            _conn = new SqlConnection(GetConnectionString());
            _connTrans = new SqlConnection(GetConnectionString());
            Open(TransactionChoice.WithoutTransaction);
            Open(TransactionChoice.WithTransaction);
        }
    

        /// <summary>
        /// Property to return the existing instance
        /// </summary>
        public static SqlDataManager UniqueInstance
        { get { return manager; } }

        public object GetData(TransactionChoice connType
            , CommandType commandType 
            , string procedureName
            , IList<SqlParameter> parameters
            , int retries
            , ExecuteType executeType, out ExecuteOutcome outcome
            , out string outcomeMsg)
        {
            object result = new object();
            IDataManager manager = SqlDataManager.UniqueInstance;
            ExecuteOutcome outcome1 = new ExecuteOutcome();
            outcome1 = ExecuteOutcome.Timeout;
            string outcomeMsg1 = "";
            //make database call
            for (int i = 0; i < retries && outcome1 == ExecuteOutcome.Timeout; i++)
            {
                result = manager.Execute(connType, commandType , procedureName
                                                       , executeType
                                                       , parameters
                                                       , out outcome1, out outcomeMsg1);
            }

            outcome = outcome1;
            outcomeMsg = outcomeMsg1;
            return result;

        }

        public object Execute(
            TransactionChoice connType
            , CommandType commandType
            , string procedure
            , ExecuteType executeType
            , IList<SqlParameter> parameters
            , out ExecuteOutcome executeOutcome, out string executeOutcomeMessage)
        {
            object result = null;
            
            SqlCommand cmd = NewSqlCommand(connType, commandType, procedure, parameters);

            try
            {
                result = CommandExecute(executeType, cmd);
                executeOutcome = ExecuteOutcome.Success;
                executeOutcomeMessage = "";
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("timeout") || ex.GetBaseException().Message.Contains("deadlock"))
                {
                    //return message that it timed out
                    executeOutcome = ExecuteOutcome.Timeout;
                    executeOutcomeMessage = "Server timed out or thread deadlock victim.  "
                           + ex.Message;
                }
                else
                {
                    executeOutcome = ExecuteOutcome.Failure;
                    throw ex;
                }
            }

            return result;
        }

        private object CommandExecute(ExecuteType executeType, SqlCommand cmd)
        {
            object result = null;
            switch (executeType)
            {
                case ExecuteType.ExecuteReader:
                    SqlDataReader reader = cmd.ExecuteReader();
                    result = reader;
                    break;
                case ExecuteType.ExecuteScaler:
                    result = cmd.ExecuteScalar();
                    break;
                case ExecuteType.ExecuteNonQuery:
                    result = cmd.ExecuteNonQuery();
                    break;
                default:
                    //do nothing
                    break;
            }
            return result;
        }

        internal string GetConnectionString()
        {
            //todo modify to use the same datasource as the Safari Application
            return  ConfigurationManager.ConnectionStrings["Aim.Excel.Importer.Presentation.Properties.Settings.SafariConnection"].ToString();
//            return "Data Source=IBM-C2-B13;Initial Catalog=Safari;Integrated Security=True;Connect Timeout=120";
        }

        internal void Open(TransactionChoice connType)
        {
            try
            {
                switch (connType)
                {
                    case TransactionChoice.WithoutTransaction:
                        if (_conn.State != ConnectionState.Open)
                        {
                            _conn.Open();
                        }
                        break;
                    case TransactionChoice.WithTransaction:
                        if (_connTrans.State != ConnectionState.Open)
                        {
                            _connTrans.Open();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Problem opening connection.", ex);
            }

        }

        internal void Close(TransactionChoice connType)
        {
            try
            {
                switch (connType)
                {
                    case TransactionChoice.WithoutTransaction:
                        if (_conn.State != ConnectionState.Closed)
                        {
                            _conn.Close();
                        }
                        break;
                    case TransactionChoice.WithTransaction:
                        if (_connTrans.State != ConnectionState.Closed)
                        {
                            _connTrans.Close();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Problem opening connection.", ex);
            }
        }

        internal SqlCommand NewSqlCommand (
                TransactionChoice connType
                , CommandType commandType
                , string procedure
                , IList<SqlParameter> parameters)
        {
            SqlCommand cmd = CreateSqlCommand(connType, procedure);
            cmd.CommandType = commandType;

            if (parameters != null)
            {
                cmd = AddParametersToSqlCommand(cmd, parameters);
            }
            return cmd;
            
        }

        internal SqlCommand AddParametersToSqlCommand(SqlCommand cmd, IList<SqlParameter> parameters)
        {
            foreach (DbParameter parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                if (parameter.Direction == ParameterDirection.Output)
                {
                    cmd.Parameters[parameter.ParameterName].Direction = parameter.Direction;
                }
            }

            return cmd;
        }

        internal SqlCommand CreateSqlCommand (
                TransactionChoice connType
                , string procedure)
        {
            SqlCommand cmd;
            switch (connType)
            {
                case TransactionChoice.WithoutTransaction:
                    cmd = new SqlCommand(procedure, _conn);
                    break;
                case TransactionChoice.WithTransaction:
                    cmd = new SqlCommand(procedure, _connTrans);
                    break;
                default:
                    cmd = new SqlCommand(procedure, _conn);
                    break;
            }
            return cmd;
            
        }



    }
}
