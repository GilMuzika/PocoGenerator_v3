using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoGenerator
{
    interface IDAO
    {                     
        List<string> GetAllTableNames();
        List<string> GetAllDataBases();
        List<string> GetAllDataBases2(string instanceName);
        Dictionary<string, string> GetColumnNamesInATable(string tableName);
        void SetConnectionString(string dataBasePathName, string instanceName);
    }
}
