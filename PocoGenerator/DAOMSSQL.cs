using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace PocoGenerator
{
    class DAOMSSQL : DAOBase, IDAO
    {
        public DAOMSSQL(): base()
        {
            _connection = new SqlConnection();
            _command = new SqlCommand();            
            _command.CommandType = System.Data.CommandType.Text;
            _command.Connection = _connection;            
        }        
        public void SetConnectionString(string dataBaseName, string instanceName)
        {
            string connStr = $"Data Source={Environment.MachineName}{instanceName};Initial Catalog={dataBaseName};Integrated Security=True";
            SetConnectionStringInternal(connStr);
        }
    }
}
