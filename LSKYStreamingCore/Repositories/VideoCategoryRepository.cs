using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    public class VideoCategoryRepository
    {
        private const int videoCategoryIDLength = 5;
        private readonly Dictionary<string, VideoCategory> _cache;

        private VideoCategory dbDataReaderToVideoCategory(SqlDataReader dataReader)
        {
            return new VideoCategory()
            {
                ID = dataReader["id"].ToString().Trim(),
                Name = dataReader["name"].ToString().Trim(),
                ParentCategoryID = dataReader["parent"].ToString().Trim(),
                IsHidden = Parsers.ParseBool(dataReader["hidden"].ToString().Trim()),
                IsPrivate = Parsers.ParseBool(dataReader["private"].ToString().Trim()),
                VideoCount = Parsers.ParseInt(dataReader["Count"].ToString().Trim()),
            };
        }

        public VideoCategoryRepository()
        {
            _cache = new Dictionary<string, VideoCategory>();
            
            using (SqlConnection connection = new SqlConnection(GlobalStreamingSettings.dbConnectionString_ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT * FROM vCategoriesWithCount;";
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            VideoCategory parsedCategory = dbDataReaderToVideoCategory(dbDataReader);
                            _cache.Add(parsedCategory.ID, parsedCategory);
                        }
                    }
                    sqlCommand.Connection.Close();
                }
            }

            // Link categories to their children
            foreach(VideoCategory cat in _cache.Values)
            {
                if (!string.IsNullOrEmpty(cat.ParentCategoryID))
                {
                    if (_cache.ContainsKey(cat.ParentCategoryID))
                    {
                        cat.ParentCategory = _cache[cat.ParentCategoryID];
                        _cache[cat.ParentCategoryID].Children.Add(cat);
                    }
                }
            }
        }

        private VideoCategory nullCategory()
        {
            return new VideoCategory()
            {
                ID = string.Empty,
                Name = "Uncategorized",
                IsHidden = false,
                IsPrivate = false,
            };
        }

        public VideoCategory Get(string id)
        {
            return _cache.ContainsKey(id) ? _cache[id] : nullCategory();
        }

        public List<VideoCategory> GetAll()
        {
            return _cache.Values.ToList();
        }

        public List<VideoCategory> GetTopLevel()
        {
            return _cache.Values.Where(c => !c.HasParent).ToList();

        }

        public string CreateNewVideoCategoryID()
        {
            string returnMe = string.Empty;
            do
            {
                returnMe = Crypto.GenerateID(videoCategoryIDLength);
            } while ((returnMe != string.Empty) && (!IsCategoryIDAvailable(returnMe)));
            return returnMe;
        }

        private bool IsCategoryIDAvailable(string categoryID)
        {
            if (string.IsNullOrEmpty(categoryID.Trim()))
            {
                return false;
            }

            bool foundCategoryWithGivenID = false;
            using (SqlConnection connection = new SqlConnection(GlobalStreamingSettings.dbConnectionString_ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT id FROM vCategoriesWithCount WHERE (id=@ID)";
                    sqlCommand.Parameters.AddWithValue("ID", categoryID);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();
                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            foundCategoryWithGivenID = true;
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return !foundCategoryWithGivenID;
        }

        public void Insert(VideoCategory category)
        {
            if (string.IsNullOrEmpty(category.ID))
            {
                category.ID = CreateNewVideoCategoryID();
            }

            using (SqlConnection connection = new SqlConnection(GlobalStreamingSettings.dbConnectionString_ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
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
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        public void Update(VideoCategory category)
        {
            using (SqlConnection connection = new SqlConnection(GlobalStreamingSettings.dbConnectionString_ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "UPDATE video_categories SET name=@NAME, hidden=@HIDDEN, private=@PRIVATE, parent=@PARENT WHERE id=@ID";

                    sqlCommand.Parameters.AddWithValue("ID", category.ID);
                    sqlCommand.Parameters.AddWithValue("NAME", category.Name);
                    sqlCommand.Parameters.AddWithValue("HIDDEN", category.IsHidden);
                    sqlCommand.Parameters.AddWithValue("PRIVATE", category.IsPrivate);
                    sqlCommand.Parameters.AddWithValue("PARENT", category.ParentCategoryID);

                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        public void Delete(VideoCategory category)
        {
            using (SqlConnection connection = new SqlConnection(GlobalStreamingSettings.dbConnectionString_ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "DELETE FROM video_categories WHERE id=@ID";
                    sqlCommand.Parameters.AddWithValue("ID", category.ID);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }
    }
}
