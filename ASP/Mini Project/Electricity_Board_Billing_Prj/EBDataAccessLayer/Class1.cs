using System.Data.SqlClient;
using System.Configuration;

namespace EBDataAccessLayer
{
    public class DBHandler
    {
        public static SqlConnection GetConnection()
        {
           
            string cs = ConfigurationManager.ConnectionStrings["EBDB"].ConnectionString;
            return new SqlConnection(cs);
        }
    }
}
