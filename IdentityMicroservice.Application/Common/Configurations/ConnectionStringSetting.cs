using System.Collections.Generic;

namespace IdentityMicroservice.Application.Common.Configurations
{
  
    public class ConnectionStringSetting
    {
        public const string NAME = "ConnectionStrings";
        public IDictionary<DatabaseIdentifier, DbConfig> ConnectionStringConfigs { get; set; }
    }

    public enum DatabaseIdentifier
    {
        IdentityDatabase,
 
    }

    public class DbConfig
    {
        public string ConnectionString { get; set; }
        public int TimeoutSeconds { get; set; }
       

    }
 


}
