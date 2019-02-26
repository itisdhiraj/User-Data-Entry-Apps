using System.Configuration;

namespace Data_Entry_App.Data.DataAccess
{

    public abstract class ConnectionConfiguration
    {
        
        protected string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["UsersDataDBConnection"].ToString();
            }
        }
    }
}
