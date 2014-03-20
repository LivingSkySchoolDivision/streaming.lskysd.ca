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
        public List<Video> Videos { get; set; }

        public VideoCategory(string id, string name, string parentcategory, bool hidden, bool isprivate)
        {
            this.Videos = new List<Video>();
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
            sqlCommand.CommandText = "SELECT * FROM video_categories ORDER BY name ASC;";
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

        public static bool InsertNewCategory(SqlConnection connection, VideoCategory category) 
        {
            bool returnMe = false;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "INSERT INTO video_categories(name, hidden, private, id, parent)"
                                    + "VALUES(@NAME, @HIDDEN, @PRIVATE, @ID, @PARENT)";

            sqlCommand.Parameters.AddWithValue("ID", category.ID);
            sqlCommand.Parameters.AddWithValue("NAME", category.Name);
            sqlCommand.Parameters.AddWithValue("HIDDEN", category.IsHidden);
            sqlCommand.Parameters.AddWithValue("PRIVATE", category.IsPrivate);
            sqlCommand.Parameters.AddWithValue("PARENT", category.ParentCategory);

            sqlCommand.Connection.Open();
            if (sqlCommand.ExecuteNonQuery() > 0)
            {
                returnMe = true;
            }
            else
            {
                returnMe = false;
            }
            sqlCommand.Connection.Close();

            return returnMe;
        }

        public static bool UpdateCategory(SqlConnection connection, VideoCategory category) 
        {
            bool returnMe = false;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "UPDATE video_categories SET name=@NAME, hidden=@HIDDEN, private=@PRIVATE, parent=@PARENT WHERE id=@ID";

            sqlCommand.Parameters.AddWithValue("ID", category.ID);
            sqlCommand.Parameters.AddWithValue("NAME", category.Name);
            sqlCommand.Parameters.AddWithValue("HIDDEN", category.IsHidden);
            sqlCommand.Parameters.AddWithValue("PRIVATE", category.IsPrivate);
            sqlCommand.Parameters.AddWithValue("PARENT", category.ParentCategory);

            sqlCommand.Connection.Open();
            if (sqlCommand.ExecuteNonQuery() > 0)
            {
                returnMe = true;
            }
            else
            {
                returnMe = false;
            }
            sqlCommand.Connection.Close();

            return returnMe;
        }

    }
}
