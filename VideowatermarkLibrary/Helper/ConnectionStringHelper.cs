using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideowatermarkLibrary.Helper
{
    public class ConnectionStringHelper
    {
        public static System.Data.SqlClient.SqlConnection getSqlConnection(string CompanyInitial = "")
        {
            var databaseIP = ConfigurationManager.AppSettings["databaseIP"];
            var initialDatabaseCatalog = ConfigurationManager.AppSettings["initialDatabaseCatalog"];
            var databaseUserID = ConfigurationManager.AppSettings["databaseUserID"];
            var databasePassword = ConfigurationManager.AppSettings["databasePassword"];

            if (string.IsNullOrWhiteSpace(CompanyInitial))
                CompanyInitial = initialDatabaseCatalog.Trim();

            var conString = $"Data Source={databaseIP};Persist Security Info=False;Initial Catalog={CompanyInitial};User Id={databaseUserID};Password={databasePassword};pooling=true";

            return new System.Data.SqlClient.SqlConnection(conString);
        }
    }
}
