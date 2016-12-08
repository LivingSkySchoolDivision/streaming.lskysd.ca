using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSKYStreamingManager
{
    public class LoginAttempt : IComparable
    {
        public DateTime eventTime { get; set; }
        public string enteredUserName { get; set; }
        public string ipAddress { get; set; }
        public string userAgent { get; set; }
        public string status { get; set; }
        public string info { get; set; }

        public static void logLoginAttempt(string username, string remoteIP, string useragent, string status, string info)
        {
            using (SqlConnection dbConnection = new SqlConnection(Settings.dbConnectionString_ReadWrite))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = dbConnection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "INSERT INTO audit_loginAttempts(eventTime,enteredUsername,ipaddress,useragent,status,info) VALUES(@CurrentTime, @Username, @IP, @UserAgent, @Status, @Info);";
                    sqlCommand.Parameters.AddWithValue("@CurrentTime", DateTime.Now.ToString());
                    sqlCommand.Parameters.AddWithValue("@Username", username);
                    sqlCommand.Parameters.AddWithValue("@IP", remoteIP);
                    sqlCommand.Parameters.AddWithValue("@UserAgent", useragent);
                    sqlCommand.Parameters.AddWithValue("@Status", status);
                    sqlCommand.Parameters.AddWithValue("@Info", info);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }

        }

        public LoginAttempt(DateTime time, string username, string ip, string uagent, string stat, string nfo)
        {
            this.eventTime = time;
            this.enteredUserName = username;
            this.ipAddress = ip;
            this.userAgent = uagent;
            this.info = nfo;
            this.status = stat;
        }

        public static List<LoginAttempt> getRecentLoginEvents(SqlConnection connection, DateTime from, DateTime to, int maxRecords = 1000)
        {
            List<LoginAttempt> returnMe = new List<LoginAttempt>();

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "SELECT TOP 100 * FROM audit_loginAttempts WHERE eventTime < @EventTo AND eventTime > @EventFrom ORDER BY eventTime DESC;";
                sqlCommand.Parameters.AddWithValue("@EventTo", to);
                sqlCommand.Parameters.AddWithValue("@EventFrom", from);

                sqlCommand.Connection.Open();
                SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                if (dbDataReader.HasRows)
                {
                    while (dbDataReader.Read())
                    {
                        returnMe.Add(new LoginAttempt(
                            DateTime.Parse(dbDataReader["eventTime"].ToString()),
                            dbDataReader["enteredUsername"].ToString(),
                            dbDataReader["ipaddress"].ToString(),
                            dbDataReader["useragent"].ToString(),
                            dbDataReader["status"].ToString(),
                            dbDataReader["info"].ToString()
                            ));
                    }
                }
                sqlCommand.Connection.Close();
            }

            return returnMe;
        }

        public override string ToString()
        {
            return "LoginAttempt (" + eventTime.ToShortDateString() + " " + eventTime.ToShortTimeString() + "," + enteredUserName + "," + ipAddress + "," + ipAddress + ")";
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            LoginAttempt obj2 = obj as LoginAttempt;

            if (obj2 != null)
            {
                return this.eventTime.CompareTo(obj2.eventTime);
            }
            else
            {
                throw new ArgumentException("Object is not a Student");
            }
        }

    }
}