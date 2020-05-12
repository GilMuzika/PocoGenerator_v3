using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace PocoGenerator
{
    class DAOSqlite : DAOBase, IDAO
    {
        public DAOSqlite()
        {
            _connection = new SQLiteConnection();
            _command = new SQLiteCommand();            
            _command.CommandType = System.Data.CommandType.Text;
            _command.Connection = _connection;
        }
        public void SetConnectionString(string dataBaseFullPath, string instanceName)
        {
            string connStr = $"Data source ={dataBaseFullPath}; Version = 3;";            
            SetConnectionStringInternal(connStr);
        }
    }
}
