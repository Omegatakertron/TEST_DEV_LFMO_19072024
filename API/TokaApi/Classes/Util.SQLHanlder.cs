using System;
using System.Data;
using System.Data.SqlClient;

namespace Util
{
    /// <summary>Provides methods to interactuate with a SQL Server database.</summary>
    public class SQLHandler
    {
        #region [Fields]
        /// <summary>Gets or sets the connection used to open the SQL Server database.</summary>
        private string? _connectionString;

        /// <summary>Gets or sets the wait time before terminating the attemp to execute a command and generating an error.</summary>
        private int _queryTimeout;
        #endregion

        #region [Properties]
        /// <summary>Gets or sets the connection used to open the SQL Server database.</summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>Gets or sets the wait time before terminating the attemp to execute a command and generating an error.</summary>
        public int QueryTimeout
        {
            get { return _queryTimeout; }
            set { _queryTimeout = value; }
        }
        #endregion

        #region [Contructors]
        /// <summary>Provides methods to interactuate with a SQL Server database.</summary>
        public SQLHandler()
        {
            _queryTimeout = 90;
        }

        /// <summary>Provides methods to interactuate with a SQL Server database.</summary>
        /// <param name="queryTimeout">Sets the wait time before terminating the attemp to execute a command and generating an error.</param>
        public SQLHandler(int queryTimeout)
        {
            _queryTimeout = queryTimeout;
        }

        /// <summary>Provides methods to interactuate with a SQL Server database.</summary>
        /// <param name="connectionString">Sets the connection used to open the SQL Server database.</param>
        public SQLHandler(string connectionString)
        {
            _connectionString = connectionString;
            _queryTimeout = 90;
        }

        /// <summary>Provides methods to interactuate with a SQL Server database.</summary>
        /// <param name="connectionString">Sets the connection used to open the SQL Server database.</param>
        /// <param name="queryTimeout">Sets the wait time before terminating the attemp to execute a command and generating an error.</param>
        public SQLHandler(string connectionString, int queryTimeout)
        {
            _connectionString = connectionString;
            _queryTimeout = queryTimeout;
        }
        #endregion

        #region [Global Methods]
        /// <summary>Sets the connection string from the stored in the appsettings.json file.</summary>
        /// <param name="connectionStringName">The connection string key.</param>
        public void SetConnectionStringFromAppSettingsFile(string connectionStringName)
        {
            _connectionString = Util.AppSettingsHandler.GetConnectionString(connectionStringName);// System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
        }

        /// <summary>Gets if the connection can be established.</summary>
        /// <returns>System.Boolean</returns>
        public bool TestConnection()
        {
            if (_connectionString != null && _connectionString.Length > 0)
            {
                var connection = new SqlConnection(_connectionString);

                connection.Open();
                connection.Close();
                return true;
            }
            else throw new Exception("Connection string value null or empty.");
        }

        /// <summary>This method copies all rows in the supplied System.Data.DataTable in the specified destination table on the server.</summary>
        /// <param name="destinationTableName">Name of the destination table on the server.</param>
        /// <param name="dataTableToUpload">System.Data.DataTable to write in the destination table.</param>
        /// <param name="exceptionMessage">Gets an exception message when the method fails.</param>
        public void BulkCopy(string destinationTableName, DataTable dataTableToUpload, out string exceptionMessage)
        {
            try
            {
                exceptionMessage = string.Empty;

                var sqlBulkCopy = new SqlBulkCopy(_connectionString);

                foreach (DataColumn oColumn in dataTableToUpload.Columns)
                {
                    sqlBulkCopy.ColumnMappings.Add(oColumn.ColumnName, oColumn.ColumnName);
                }

                sqlBulkCopy.DestinationTableName = destinationTableName;
                sqlBulkCopy.BulkCopyTimeout = _queryTimeout;
                sqlBulkCopy.WriteToServer(dataTableToUpload);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }
        }

        /// <summary>Executes a SQL command.</summary>
        /// <param name="commandText">SQL command to execute.</param>
        public void Exec(string commandText)
        {
            var connection = new SqlConnection(_connectionString);

            connection.Open();

            var command = new SqlCommand()
            {
                CommandText = commandText,
                Connection = connection,
                CommandTimeout = _queryTimeout
            };

            command.ExecuteNonQuery();
            connection.Close();

        }

        /// <summary>Executes an stored procedure.</summary>
        /// <param name="storedProcedureName">Name of the stored procedure to execute.</param>
        /// <param name="args">An object array that contains the parameters of the stored procedure.</param>
        public void ExecStoredProcedure(string storedProcedureName, params object[] args)
        {
            var exceptionMessage = string.Empty;
            var commandText = GetStoredProcedureCommandExecution(out exceptionMessage, storedProcedureName, args);

            if (exceptionMessage.Length > 0) throw new Exception(exceptionMessage);

            var connection = new SqlConnection(_connectionString);

            connection.Open();

            var command = new SqlCommand()
            {
                CommandText = commandText,
                Connection = connection,
                CommandTimeout = _queryTimeout
            };

            command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>Executes an stored procedure.</summary>
        /// <param name="storedProcedureName">Name of the stored procedure to execute.</param>
        /// <param name="parameters"></param>
        public void ExecStoredProcedure(string storedProcedureName, List<SqlParameter> parameters)
        {

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandTimeout = _queryTimeout,
                CommandType = CommandType.StoredProcedure
            };

            foreach (var parameter in parameters) { command.Parameters.Add(parameter); }

            command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>Gets data table obtained from a SQL command execution. Important! This method requires Microsoft.AspNetCore.Mvc.NewtonsoftJson</summary>
        /// <param name="commandText">SQL command to execute.</param>
        /// <returns>System.Data.DataTable</returns>
        public DataTable GetDataTable(string commandText)
        {
            var dataTable = new DataTable();
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(commandText, connection) { CommandTimeout = _queryTimeout };
            var dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataTable);

            return dataTable;
        }

        /// <summary>Gets data table obtained from a StoredProcedure execution. Important! This method requires Microsoft.AspNetCore.Mvc.NewtonsoftJson</summary>
        /// <param name="storedProcedureName">Name of the stored procedure to execute.</param>
        /// <param name="args">An object array that contains the parameters of the stored procedure.</param>
        /// <returns>System.Data.DataTable</returns>
        public DataTable GetDataTable(string storedProcedureName, params object[] args)
        {
            var exceptionMessage = string.Empty;
            var commandText = GetStoredProcedureCommandExecution(out exceptionMessage, storedProcedureName, args);

            if (exceptionMessage.Length > 0) throw new Exception(exceptionMessage);

            var dataTable = new DataTable();
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(commandText, connection)
            {
                CommandTimeout = _queryTimeout
            };
            var dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataTable);

            return dataTable;
        }

        /// <summary>Gets data table obtained from a StoredProcedure execution. Important! This method requires Microsoft.AspNetCore.Mvc.NewtonsoftJson</summary>
        /// <param name="storedProcedureName">Name of the stored procedure to execute.</param>
        /// <param name="parameters"></param>
        /// <returns>System.Data.DataTable</returns>
        public DataTable GetDataTable(string storedProcedureName, List<SqlParameter> parameters)
        {
            var dataTable = new DataTable();
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandTimeout = _queryTimeout,
                CommandType = CommandType.StoredProcedure
            };

            foreach (var parameter in parameters) { command.Parameters.Add(parameter); }

            var dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataTable);

            return dataTable;
        }

        /// <summary>Gets the tables obtained from a SQL command execution. Important! This method requires Microsoft.AspNetCore.Mvc.NewtonsoftJson</summary>
        /// <param name="commandText">SQL command to execute.</param>
        /// <returns>System.Data.DataSet</returns>
        public DataSet GetDataSet(string commandText)
        {
            var dataSet = new DataSet();
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(commandText, connection) { CommandTimeout = _queryTimeout };
            var dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataSet);

            return dataSet;
        }

        /// <summary>Gets the tables obtained from a StoredProcedure execution. Important! This method requires Microsoft.AspNetCore.Mvc.NewtonsoftJson</summary>
        /// <param name="storedProcedureName">Name of the stored procedure to execute.</param>
        /// <param name="args">An object array that contains the parameters of the stored procedure.</param>
        /// <returns>System.Data.DataSet</returns>
        public DataSet GetDataSet(string storedProcedureName, params object[] args)
        {
            var exceptionMessage = string.Empty;
            var commandText = GetStoredProcedureCommandExecution(out exceptionMessage, storedProcedureName, args);

            if (exceptionMessage.Length > 0) throw new Exception(exceptionMessage);

            var dataSet = new DataSet();
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(commandText, connection)
            {
                CommandTimeout = _queryTimeout
            };
            var dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataSet);

            return dataSet;
        }

        /// <summary>Gets the tables obtained from a StoredProcedure execution. Important! This method requires Microsoft.AspNetCore.Mvc.NewtonsoftJson</summary>
        /// <param name="storedProcedureName">Name of the stored procedure to execute.</param>
        /// <param name="parameters"></param>
        /// <returns>System.Data.DataSet</returns>
        public DataSet GetDataSet(string storedProcedureName, List<SqlParameter> parameters)
        {
            var dataSet = new DataSet();
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandTimeout = _queryTimeout,
                CommandType = CommandType.StoredProcedure
            };

            foreach (var parameter in parameters) { command.Parameters.Add(parameter); }

            var dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataSet);

            return dataSet;
        }
        #endregion

        #region [Local Methods]
        /// <summary></summary>
        /// <param name="exceptionMessage">Gets an exception message when the method fails.</param>
        /// <param name="storedProcedureName">Name of the stored procedure name.</param>
        /// <param name="args">An object array that contains the parameters of the stored procedure.</param>
        /// <returns>System.String</returns>
        private static string GetStoredProcedureCommandExecution(out string exceptionMessage, string storedProcedureName, params object[] args)
        {
            try
            {
                exceptionMessage = string.Empty;
                var cmdText = string.Concat("EXEC ", storedProcedureName);

                if (args == null || args.Length <= 0) return cmdText;

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == null)
                    {
                        cmdText = string.Concat(cmdText, " null", i < args.Length - 1 ? "," : string.Empty);
                    }
                    else
                    {
                        string type = args[i].GetType().ToString().ToLower();

                        if (type.EndsWith("char") || type.EndsWith("string"))
                            cmdText = string.Concat(cmdText, " '", (string)args[i], "'", i < args.Length - 1 ? "," : string.Empty);
                        else if (type.EndsWith("datetime"))
                            cmdText = string.Concat(cmdText, " '", ((DateTime)args[i]).ToString("yyyy-MM-dd HH:mm:ss"), "'", i < args.Length - 1 ? "," : string.Empty);
                        else
                            cmdText = string.Concat(cmdText, " ", args[i], i < args.Length - 1 ? "," : string.Empty);
                    }
                }

                return cmdText;
            }
            catch (Exception ex)
            {
                exceptionMessage = string.Concat("Util.SQLHandler.GetStoredProcedureCommandExecution.Exception: ", ex.Message);
                return string.Empty;
            }
        }
        #endregion
    }
}
