using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PocoGenerator
{
    abstract class DAOBase
    {
        protected DbConnection _connection;
        protected DbCommand _command;

        public DAOBase()
        {
            FlexibleMessageBox.MAX_WIDTH_FACTOR = Screen.PrimaryScreen.WorkingArea.Width;
            FlexibleMessageBox.MAX_HEIGHT_FACTOR = Screen.PrimaryScreen.WorkingArea.Height;
        }

        public string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings["connString1"].ConnectionString;
        }
        protected void SetConnectionStringInternal(string connStr)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.ConnectionStrings.ConnectionStrings["connString1"].ConnectionString = connStr;
            config.ConnectionStrings.ConnectionStrings["connString1"].ProviderName = "System.Data.SqlClient";
            config.Save(ConfigurationSaveMode.Modified);

            _connection.ConnectionString = config.ConnectionStrings.ConnectionStrings["connString1"].ConnectionString;
        }
        private void GetServerAndInstanceNames(out string serverName, out string instanceName)
        {
            string servername = string.Empty;
            string instancename = string.Empty;
            var table = SqlDataSourceEnumerator.Instance.GetDataSources();

            string str = string.Empty;
            foreach (DataRow row in table.AsEnumerable())
            {
                if (!(row["ServerName"] is DBNull)) servername = (string)row["ServerName"];
                if (!(row["InstanceName"] is DBNull)) instancename = "\\" + (string)row["InstanceName"];
            }
            serverName = servername;
            instanceName = instancename;
        }
        /// <summary>
        /// Implementing mechanism that provides names of all the MSSQL databases
        /// </summary>
        /// <returns>List<string> with names of all the databases</returns>
        public List<string> GetAllDataBases()
        {
            List<string> databases = new List<string>();
            GetServerAndInstanceNames(out string serverName, out string instanceName);

            //
            //the server name also represented by Environment.MachineName,
            //as: 
            //serverName = Environment.MachineName;
            //so uncommenting the previous line is a way to get this machine name for Smo object
            //if the method "GetServerAndInstanceNames" don't working
            //

            var server = new Microsoft.SqlServer.Management.Smo.Server(serverName + instanceName);
            foreach (Database db in server.Databases)
            {
                databases.Add(db.Name);
            }
            return databases;
        }
        public List<string> GetAllDataBases2(string instanceName)
        {
            List<string> databases = new List<string>();
            string serverName = Environment.MachineName;

            Server server = new Microsoft.SqlServer.Management.Smo.Server(serverName + instanceName);
            foreach (Database db in server.Databases)
            {
                databases.Add(db.Name);
            }
            return databases;
        }

        public List<string> GetAllTableNames()
        {
            List<string> tableNames = new List<string>();
            _connection.Open();

            var schema = _connection.GetSchema("Tables");
            foreach (DataRow s in schema.Rows)
            {
                string tablename = (string)s[2];
                tableNames.Add(tablename);
            }
            _connection.Close();
            return tableNames;
        }

        /// <summary>
        /// this function accessed from within threads other than the application main thread.
        /// It takes a name of a particular table within a tatabase amd returns 
        /// a Dictionary<string, string> which keys are names of the columnes of the table and the values are their type names.
        /// </summary>
        /// <param name="tableName">Name of table within accesses database</param>
        /// <returns></returns>        
        public Dictionary<string, string> GetColumnNamesInATable(string tableName)
        {
            Dictionary<string, string> columnsNames = new Dictionary<string, string>();
            //lock: threaf-safe ptotection
            lock (this)
            {
                _connection.Open();
                _command.CommandText = $"SELECT * FROM {tableName}";
                using (DbDataReader reader = _command.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columnsNames.Add(reader.GetName(i), reader.GetFieldType(i).Name);
                    }
                }
                _connection.Close();
            }
            return columnsNames;
        }
    }
}
