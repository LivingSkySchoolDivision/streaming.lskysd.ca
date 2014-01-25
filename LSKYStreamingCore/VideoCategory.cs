using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LSKYStreamingCore
{
    public class VideoCategory
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ParentCategory { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }

        public VideoCategory(string id, string name, string parentcategory, bool hidden, bool isprivate)
        {
            this.ID = id;
            this.Name = name;
            this.ParentCategory = parentcategory;
            this.IsHidden = hidden;
        }

        public static List<VideoCategory> LoadAllCategories(SqlConnection connection)
        {
            List<VideoCategory> ReturnedCategories = new List<VideoCategory>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM video_categories;";
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    ReturnedCategories.Add(
                        new VideoCategory(
                            dbDataReader["id"].ToString(),
                            dbDataReader["name"].ToString(),
                            dbDataReader["parent"].ToString(),
                            LSKYCommon.ParseDatabaseBool(dbDataReader["hidden"].ToString(), false),
                            LSKYCommon.ParseDatabaseBool(dbDataReader["private"].ToString(), false)
                            )
                        );
                }
            }

            sqlCommand.Connection.Close();

            return ReturnedCategories;
        }


        


    }
}
