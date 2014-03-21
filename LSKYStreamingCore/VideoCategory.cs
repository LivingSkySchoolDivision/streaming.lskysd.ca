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
        public string ParentCategoryID { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }
        public List<Video> Videos { get; set; }
        public int VideoCount { get; set; }
        public List<VideoCategory> Children { get; set; }
        public VideoCategory ParentCategory { get; set; }

        public VideoCategory(string id, string name, string parentcategory, bool hidden, bool isprivate, int count)
        {
            this.Videos = new List<Video>();
            this.Children = new List<VideoCategory>();
            this.ID = id;
            this.Name = name;
            this.ParentCategoryID = parentcategory;
            this.IsHidden = hidden;
            this.IsPrivate = isprivate;
            this.VideoCount = count;
        }

        public bool HasChildren()
        {
            if (Children.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DoesIDExist(SqlConnection connection, string categoryID)
        {
            bool returnMe = false;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT id FROM video_categories WHERE (id=@ID)";
            sqlCommand.Parameters.AddWithValue("ID", categoryID);
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();
            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    returnMe = true;
                }
            }

            sqlCommand.Connection.Close();
            return returnMe;
        }

        public static List<VideoCategory> LoadAll(SqlConnection connection)
        {
            List<VideoCategory> ReturnedCategories = new List<VideoCategory>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM vCategoriesWithCount ORDER BY name ASC;";
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
                            LSKYCommon.ParseDatabaseBool(dbDataReader["private"].ToString(), false),
                            LSKYCommon.ParseDatabaseInt(dbDataReader["count"].ToString())
                            )
                        );
                }
            }

            sqlCommand.Connection.Close();

            // Sort into nested categories
            List<VideoCategory> TopLevelCategories = new List<VideoCategory>();

            // Set parents and children
            foreach (VideoCategory Parent in ReturnedCategories)
            {
                foreach (VideoCategory Child in ReturnedCategories)
                {
                    if (Child.ParentCategoryID == Parent.ID)
                    {
                        Parent.Children.Add(Child);
                        Child.ParentCategory = Parent;
                    }
                }
            }

            // Only return top level categories
            foreach (VideoCategory cat in ReturnedCategories)
            {
                if (string.IsNullOrEmpty(cat.ParentCategoryID))
                {
                    TopLevelCategories.Add(cat);
                }
            }

            return TopLevelCategories;
        }

        public static VideoCategory Load(SqlConnection connection, string categoryID)
        {
            VideoCategory ReturnedCategories = null;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM vCategoriesWithCount WHERE id=@ID;";
            sqlCommand.Parameters.AddWithValue("ID", categoryID);
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    ReturnedCategories = new VideoCategory(
                            dbDataReader["id"].ToString(),
                            dbDataReader["name"].ToString(),
                            dbDataReader["parent"].ToString(),
                            LSKYCommon.ParseDatabaseBool(dbDataReader["hidden"].ToString(), false),
                            LSKYCommon.ParseDatabaseBool(dbDataReader["private"].ToString(), false),
                            LSKYCommon.ParseDatabaseInt(dbDataReader["count"].ToString())
                            );
                }
            }

            sqlCommand.Connection.Close();
            
            return ReturnedCategories;
        }
        
        public List<VideoCategory> GetAllChildrenRecursively()
        {
            List<VideoCategory> returnMe = new List<VideoCategory>();

            returnMe.Add(this);

            foreach (VideoCategory Child in this.Children)
            {
                returnMe.AddRange(Child.GetAllChildrenRecursively());
            }

            return returnMe; 
        }
        
        public static bool InsertNew(SqlConnection connection, VideoCategory category) 
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
            sqlCommand.Parameters.AddWithValue("PARENT", category.ParentCategoryID);

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

        public static bool Update(SqlConnection connection, VideoCategory category) 
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
            sqlCommand.Parameters.AddWithValue("PARENT", category.ParentCategoryID);

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
