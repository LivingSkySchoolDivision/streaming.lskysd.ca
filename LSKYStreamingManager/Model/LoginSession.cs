using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace LSKYStreamingManager
{
    public class LoginSession
    {
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public string Thumbprint { get; set; }
        public string BrowserUserAgent { get; set; }
        public DateTime SessionStarted { get; set; }
        public DateTime SessionExpires { get; set; }

        public LoginSession() { }
    }
}