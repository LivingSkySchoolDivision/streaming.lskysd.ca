using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LSKYStreamingCore;

namespace LSKYStreamingManager
{
    class LoginSessionRepository
    {
        private LoginSession sqlDataReaderToLoginSession(SqlDataReader dataReader)
        {
            return new LoginSession()
            {
                Thumbprint = dataReader["id_hash"].ToString(),
                Username = dataReader["username"].ToString(),
                IPAddress = dataReader["ip"].ToString(),
                BrowserUserAgent = dataReader["useragent"].ToString(),
                SessionStarted = Parsers.ParseDate(dataReader["sessionstarts"].ToString()),
                SessionExpires = Parsers.ParseDate(dataReader["sessionends"].ToString())
            };
        }

        public List<LoginSession> GetAll()
        {
            List<LoginSession> returnMe = new List<LoginSession>();
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT * FROM manager_logon_sessions;";
                    sqlCommand.Connection.Open();
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            returnMe.Add(sqlDataReaderToLoginSession(dataReader));
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }
            return returnMe;
        }
        
        public LoginSession Get(string hash, string ip, string useragent)
        {
            LoginSession returnMe = null;
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
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
                            returnMe = sqlDataReaderToLoginSession(dataReader);
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }
            return returnMe;

        }

        public bool Delete(string hash)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = "DELETE FROM manager_logon_sessions WHERE id_hash=@Hash;";
                        sqlCommand.Parameters.AddWithValue("@Hash", hash);
                        sqlCommand.Connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Connection.Close();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public  List<LoginSession> GetActive()
        {
            List<LoginSession> returnMe = new List<LoginSession>();

            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
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
                            returnMe.Add(sqlDataReaderToLoginSession(dataReader));
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }

            return returnMe;
        }

        private string generateNewSessionID(string seed)
        {
            return Settings.getMD5("LSKY" + DateTime.Now.ToString("ffffff") + seed);
        }

        public  string Create(string username, string remoteIP, string useragent)
        {

            // Check to see if this user is in the correct security group for access to this site
            List<string> SecurityGroupMembers = Settings.getGroupMembers("lskysd.ca", Settings.adminGroupName);

            if (SecurityGroupMembers.Contains(username.ToLower()))
            {
                // Create the session
                using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_ReadWrite))
                {
                    // Generate a session ID
                    string newSessionID = generateNewSessionID(username + remoteIP + useragent);

                    TimeSpan sessionDuration = new TimeSpan(2, 0, 0);
                    if ((DateTime.Now.Hour > 7) && (DateTime.Now.Hour < 13))
                    {
                        sessionDuration = new TimeSpan(6, 0, 0);
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

        private static void PurgeExpired(SqlConnection connection)
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