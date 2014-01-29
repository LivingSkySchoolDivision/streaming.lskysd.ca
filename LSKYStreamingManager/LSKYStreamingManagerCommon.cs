using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LSKYStreamingManager
{
    public static class LSKYStreamingManagerCommon
    {
        public static string adminGroupName = "Streaming Site Managers";
        public static string dbConnectionString_ReadWrite = ConfigurationManager.ConnectionStrings["StreamingDatabaseReadWrite"].ConnectionString;
        
        // What to call the login session cookie on the end user's computer
        public static string logonCookieName = "LSKYStreamManager";

        // Application name for use in links, etc
        public static string applicationName = string.Empty;

        // URL to the login page. If not logged in, users are redirected here
        public static string loginURL = applicationName + "/Login/index.aspx";

        // URL to the index page - this is where users get sent when they log in
        public static string indexURL = applicationName + "/index.aspx";

        // URL to a page explaining that the site can only be accessed within a specific network
        public static string outsideErrorMessage = applicationName + "/Login/outside.html";

        // Only IP addresses starting with this are allowed to access the site        
        public static string localNetworkChunk = "10.177.";
        
        /// <summary>
        /// Returns the name of the server
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static string getServerName(HttpRequest Request)
        {
            return Request.ServerVariables["SERVER_NAME"].ToString().Trim();
        }

        /// <summary>
        /// Returns a List of members of the specified group (in lower case)
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static List<String> getGroupMembers(string domain, string groupName)
        {
            List<string> returnMe = new List<string>();

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                using (GroupPrincipal grp = GroupPrincipal.FindByIdentity(pc, IdentityType.Name, groupName))
                {
                    if (grp != null)
                    {
                        foreach (Principal p in grp.GetMembers(true))
                        {
                            returnMe.Add(p.SamAccountName.ToLower());
                        }
                    }
                }
            }
            return returnMe;
        }

        /// <summary>
        /// Retreives the session ID from the users's cookies
        /// </summary>
        /// <returns></returns>
        public static string getSessionIDFromCookies(string cookieName, HttpRequest request)
        {
            HttpCookie sessionCookie = request.Cookies[cookieName];
            if (sessionCookie != null)
            {
                return sessionCookie.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Validates a username and password against Active Directory and returns true if they match.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool validateADCredentials(string domain, string username, string password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                return pc.ValidateCredentials(username, password);
            }
        }

        /// <summary>
        /// Returns an MD5 hash of the specified string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getMD5(string input)
        {
            string returnMe = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }


        }
        

    }
}