using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    public class AlertRepository
    {
        private readonly Dictionary<int, Alert> _cache;

        public AlertRepository()
        {
            this._cache = new Dictionary<int, Alert>();

            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT * FROM alerts ORDER BY display_from ASC;";
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            Alert parsedAlert = dbDataReaderToAlert(dbDataReader);
                            _cache.Add(parsedAlert.ID, parsedAlert);
                        }
                    }
                }
            }
        }

        private Alert dbDataReaderToAlert(SqlDataReader dbDataReader)
        {
            int parsedImportance = Parsers.ParseInt(dbDataReader["importance"].ToString());

            AlertImportance AlertImportance = AlertImportance.Normal;

            if (parsedImportance > 0)
            {
                AlertImportance = AlertImportance.High;
            }

            return new Alert()
            {
                ID = Parsers.ParseInt(dbDataReader["id"].ToString()),
                Content = dbDataReader["text"].ToString(),
                DisplayFrom = Parsers.ParseDate(dbDataReader["display_from"].ToString()),
                DisplayTo = Parsers.ParseDate(dbDataReader["display_to"].ToString()),
                Importance = AlertImportance
            };              
        }

        public List<Alert> GetAll()
        {
            return _cache.Values.ToList();
        }

        public List<Alert> GetActive()
        {
            return _cache.Values.Where(alert => alert.DisplayFrom <= DateTime.Now && alert.DisplayTo >= DateTime.Now).ToList();
        }

        public Alert Get(int id)
        {
            if (_cache.ContainsKey(id))
            {
                return _cache[id];
            } else
            {
                return null;
            }
        }

        public void Delete(int alertID)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadWrite))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "DELETE FROM alerts WHERE id=@ID;";
                    sqlCommand.Parameters.AddWithValue("ID", alertID);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        public void Delete(Alert alert)
        {
            Delete(alert.ID);
        }
        
        public void Insert(Alert alert)
        {
            // Calculate importance value
            int importance = 0;
            if (alert.Importance == AlertImportance.High)
            {
                importance = 1;
            }

            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadWrite))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
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
                }
            }
        }
    }
}
