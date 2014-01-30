using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    public class Alert
    {
        public enum importance
        {
            High,
            Normal
        }

        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime DisplayFrom { get; set; }
        public DateTime DisplayTo { get; set; }
        public importance Importance { get; set; }

        public Alert(int id, string content, DateTime datefrom, DateTime dateto, importance importance)
        {
            this.ID = id;
            this.Content = content;
            this.DisplayFrom = datefrom;
            this.DisplayTo = dateto;
            this.Importance = importance;
        }

        private static Alert dbDataReaderToAlert(SqlDataReader dbDataReader)
        {
            int parsedImportance = LSKYCommon.ParseDatabaseInt(dbDataReader["importance"].ToString());

            importance AlertImportance = importance.Normal;

            if (parsedImportance > 0)
            {
                AlertImportance = importance.High;
            }

            return new Alert(
                            LSKYCommon.ParseDatabaseInt(dbDataReader["id"].ToString()),
                            dbDataReader["text"].ToString(),
                            LSKYCommon.ParseDatabaseDateTime(dbDataReader["display_from"].ToString()),
                            LSKYCommon.ParseDatabaseDateTime(dbDataReader["display_to"].ToString()),
                            AlertImportance
                            );
        }


        public static List<Alert> LoadAllAlerts(SqlConnection connection)
        {
            List<Alert> ReturnedAlerts = new List<Alert>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM alerts ORDER BY display_from ASC;";
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    ReturnedAlerts.Add(dbDataReaderToAlert(dbDataReader));
                }
            }

            sqlCommand.Connection.Close();

            return ReturnedAlerts;
        }

        public static List<Alert> LoadActiveAlerts(SqlConnection connection)
        {
            List<Alert> ReturnedAlerts = new List<Alert>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM alerts WHERE (display_from < @CURDATE) AND (display_to > @CURDATE) ORDER BY display_from ASC;";
            sqlCommand.Parameters.AddWithValue("CURDATE", DateTime.Now);
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    ReturnedAlerts.Add(dbDataReaderToAlert(dbDataReader));
                }
            }

            sqlCommand.Connection.Close();

            return ReturnedAlerts;
        }

        public static List<Alert> DeleteAlert(SqlConnection connection, int alertID)
        {
            List<Alert> ReturnedAlerts = new List<Alert>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "DELETE FROM alerts WHERE id=@ID;";
            sqlCommand.Parameters.AddWithValue("ID", alertID);
            sqlCommand.Connection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Connection.Close();
            return ReturnedAlerts;
        }

        public static List<Alert> InsertNewAlert(SqlConnection connection, Alert alert)
        {
            List<Alert> ReturnedAlerts = new List<Alert>();

            // Calculate importance value
            int importance = 0;
            if (alert.Importance == Alert.importance.High)
            {
                importance = 1;
            }

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "INSERT INTO alerts(text, display_from, display_to, importance) VALUES(@TEXT, @DISPLAYFROM, @DISPLAYTO, @IMPORTANCE)";
            sqlCommand.Parameters.AddWithValue("TEXT", alert.Content);
            sqlCommand.Parameters.AddWithValue("DISPLAYFROM", alert.DisplayFrom);
            sqlCommand.Parameters.AddWithValue("DISPLAYTO", alert.DisplayTo);
            sqlCommand.Parameters.AddWithValue("IMPORTANCE", importance);
            sqlCommand.Connection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Connection.Close();
            return ReturnedAlerts;
        }
    }
}
