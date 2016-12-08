using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    public static class DatabaseConnectionStrings
    {
        public static string ReadOnly = ConfigurationManager.ConnectionStrings["StreamingDatabaseReadOnly"].ConnectionString;

        public static string ReadWrite
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["StreamingDatabaseReadWrite"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["StreamingDatabaseReadWrite"].ConnectionString;
                }
                else
                {
                    return string.Empty;
                }
            }
        } 
    }
}
