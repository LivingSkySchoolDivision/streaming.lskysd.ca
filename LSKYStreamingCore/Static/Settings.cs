using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    static class Settings
    { 
        // Database connection strings
        public static string dbConnectionString_ReadOnly = ConfigurationManager.ConnectionStrings["StreamingDatabaseReadOnly"].ConnectionString;

    }
}
