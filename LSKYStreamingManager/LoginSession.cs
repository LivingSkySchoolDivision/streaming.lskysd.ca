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
        public string username { get; set; }
        public string ip { get; set; }
        public string hash { get; set; }
        public string useragent { get; set; }
        public DateTime starts { get; set; }
        public DateTime ends { get; set; }

        public LoginSession(string username, string ip, string hash, string useragent, DateTime starts, DateTime ends)
        {
            this.username = username;
            this.ip = ip;
            this.hash = hash;
            this.useragent = useragent;
            this.starts = starts;
            this.ends = ends;
        }

        /// <summary>
        /// Loads a list of all sessions in the system, including expired ones
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<LoginSession> loadAllSessions(SqlConnection connection)
        {
            List<LoginSession> returnMe = new List<LoginSession>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM manager_logon_sessions;";
            sqlCommand.Connection.Open();
            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {                    
                    returnMe.Add(new LoginSession(
                                dataReader["username"].ToString(),
                                dataReader["ip"].ToString(),
                                dataReader["id_hash"].ToString(),
                                dataReader["useragent"].ToString(),
                                DateTime.Parse(dataReader["sessionstarts"].ToString()),
                                DateTime.Parse(dataReader["sessionends"].ToString())
                            ));
                }
            }

            sqlCommand.Connection.Close();
            return returnMe;

        }

        /// <summary>
        /// Loads the specified session, if it exists and if it is valid
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="hash"></param>
        /// <param name="ip"></param>
        /// <param name="useragent"></param>
        /// <returns></returns>
        public static LoginSession loadThisSession(SqlConnection connection, string hash, string ip, string useragent)
        {
            LoginSession returnMe = null;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM manager_logon_sessions WHERE id_hash=@Hash AND ip=@IP AND useragent=@UA AND sessionstarts < {fn NOW()} AND sessionends > {fn NOW()};";
            sqlCommand.Parameters.AddWithValue("@Hash", hash);
            sqlCommand.Parameters.AddWithValue("@IP", ip);
            sqlCommand.Parameters.AddWithValue("@UA", useragent);
            sqlCommand.Connection.Open();
            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    returnMe = new LoginSession(
                                dataReader["username"].ToString(),
                                dataReader["ip"].ToString(),
                                dataReader["id_hash"].ToString(),
                                dataReader["useragent"].ToString(),
                                DateTime.Parse(dataReader["sessionstarts"].ToString()),
                                DateTime.Parse(dataReader["sessionends"].ToString())
                            );
                }
            }

            sqlCommand.Connection.Close();
            return returnMe;

        }

        /// <summary>
        /// Deletes a session from the session table
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool expireThisSession(SqlConnection connection, string hash)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "DELETE FROM manager_logon_sessions WHERE id_hash=@Hash;";
                sqlCommand.Parameters.AddWithValue("@Hash", hash);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads all sessions who's dates and times are currently within the active period
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static List<LoginSession> loadActiveSessions(SqlConnection connection)
        {
            List<LoginSession> returnMe = new List<LoginSession>();

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "SELECT * FROM manager_logon_sessions WHERE sessionstarts < {fn NOW()} AND sessionends > {fn NOW()};";

                sqlCommand.Connection.Open();
                SqlDataReader dataReader = sqlCommand.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        returnMe.Add(new LoginSession(
                                    dataReader["username"].ToString(),
                                    dataReader["ip"].ToString(),
                                    dataReader["id_hash"].ToString(),
                                    dataReader["useragent"].ToString(),
                                    DateTime.Parse(dataReader["sessionstarts"].ToString()),
                                    DateTime.Parse(dataReader["sessionends"].ToString())
                                ));
                    }
                }
                sqlCommand.Connection.Close();
            }

            return returnMe;
        }

        /// <summary>
        /// Generates a new random string to use as a session ID
        /// </summary>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string generateNewSessionID(string seed)
        {
            return LSKYStreamingManagerCommon.getMD5("LSKY" + DateTime.Now.ToString("ffffff") + seed);
        }

        /// <summary>
        /// Creates a new session and returns the session ID. This assumes that the username and password were valid.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="remoteIP"></param>
        /// <param name="useragent"></param>
        /// <returns></returns>
        public static string createSession(string username, string remoteIP, string useragent)
        {
            string returnMe = string.Empty;

            // Check to see if this user is in the correct security group for access to this site
            List<string> SecurityGroupMembers = LSKYStreamingManagerCommon.getGroupMembers("lskysd.ca", LSKYStreamingManagerCommon.adminGroupName);

            if (SecurityGroupMembers.Contains(username.ToLower()))
            {
                // Create the session
                using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                {
                    // Generate a session ID
                    string newSessionID = generateNewSessionID(username + remoteIP + useragent);

                    // Determine a timespan for this session based on the current time of day
                    // If logging in during the work day, make a session last 8 hours
                    // If logging in after hours, make the session only last 2 hours
                    TimeSpan workDayStart = new TimeSpan(7, 30, 0);
                    TimeSpan workDayEnd = new TimeSpan(15, 00, 0);
                    TimeSpan now = DateTime.Now.TimeOfDay;
                    TimeSpan sessionDuration;
                    if ((now >= workDayStart) && (now <= workDayEnd))
                    {
                        sessionDuration = new TimeSpan(8, 0, 0);
                    }
                    else
                    {
                        sessionDuration = new TimeSpan(2, 0, 0);
                    }


                    // Create a session in the database 
                    // Also while we are querying the database, clear out expired sessions that are lingering, and clear any existing sessions for
                    // this user, limiting the site to one session per user (per site code)
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = "DELETE FROM manager_logon_sessions WHERE sessionends < {fn NOW()};DELETE FROM manager_logon_sessions WHERE username=@USERNAME;INSERT INTO manager_logon_sessions(id_hash,username,ip,useragent,sessionstarts,sessionends) VALUES(@ID, @USERNAME, @IP, @USERAGENT, @SESSIONSTART, @SESSIONEND);";
                        sqlCommand.Parameters.AddWithValue("@ID", newSessionID);
                        sqlCommand.Parameters.AddWithValue("@USERNAME", username);
                        sqlCommand.Parameters.AddWithValue("@IP", remoteIP);
                        sqlCommand.Parameters.AddWithValue("@USERAGENT", useragent);
                        sqlCommand.Parameters.AddWithValue("@SESSIONSTART", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("@SESSIONEND", DateTime.Now.Add(sessionDuration));
                        sqlCommand.Connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Connection.Close();
                    }

                    return newSessionID;
                }
            }
            else
            {
                return string.Empty;
            }

        }
               

        /// <summary>
        /// Purges expires sessions from the database
        /// </summary>
        /// <param name="connection"></param>
        public static void purgeExpiredSessions(SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "DELETE FROM manager_logon_sessions WHERE sessionends < {fn NOW()};";
            sqlCommand.Connection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Connection.Close();
        }

    }
}