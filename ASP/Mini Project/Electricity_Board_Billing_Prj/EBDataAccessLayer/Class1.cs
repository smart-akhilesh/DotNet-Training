using System.Data.SqlClient;
using System.Configuration;

namespace EBDataAccessLayer
{
    public class DBHandler
    {
        public static SqlConnection GetConnection()
        {
            // Reads connection string from App.config (or Web.config in calling app)
            string cs = ConfigurationManager.ConnectionStrings["EBDB"].ConnectionString;
            return new SqlConnection(cs);
        }
    }
}
