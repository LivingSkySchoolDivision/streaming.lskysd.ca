using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYStreamingManager
{
    public static class Logging
    {
        public static void logLoginAttempt(string username, string remoteIP, string useragent, string status, string info)
        {
            LoginAttempt.logLoginAttempt(username, remoteIP, useragent, status, info);
        }
    
    }
}