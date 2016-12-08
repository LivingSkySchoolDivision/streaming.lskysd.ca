using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSKYStreamingCore.ExtensionMethods;

namespace LSKYStreamingCore
{
    public class VideoRepository
    {
        private const int videoIDLength = 5;
        private readonly VideoCategoryRepository categoryRepo;

        public VideoRepository()
        {
            categoryRepo = new VideoCategoryRepository();
        }
                                       
        private Video dbDataReaderToVideo(SqlDataReader dbDataReader)
        {
            return new Video()
            {
                ID = dbDataReader["id"].ToString(),
                Name = dbDataReader["name"].ToString(),
                Author = dbDataReader["author"].ToString(),
                Description = dbDataReader["description"].ToString(),
                Width = Parsers.ParseInt(dbDataReader["width"].ToString()),
                Height = Parsers.ParseInt(dbDataReader["height"].ToString()),
                DateAdded = Parsers.ParseDate(dbDataReader["date_added"].ToString()),
                DurationInSeconds = Parsers.ParseInt(dbDataReader["duration_in_seconds"].ToString()),
                FileURL_H264 = dbDataReader["file_mp4"].ToString(),
                FileURL_THEORA = dbDataReader["file_ogv"].ToString(),
                FileURL_VP8 = dbDataReader["file_webm"].ToString(),
                DownloadURL = dbDataReader["download_url"].ToString(),
                YoutubeURL = dbDataReader["youtube_url"].ToString(),
                IsHidden = Parsers.ParseBool(dbDataReader["hidden"].ToString()),
                IsPrivate = Parsers.ParseBool(dbDataReader["private"].ToString()),
                Tags = dbDataReader["tags"].ToString().Split(';').ToList(),
                LegacyVideoID = dbDataReader["legacy_video_id"].ToString(),
                ThumbnailURL = dbDataReader["thumbnail_url"].ToString(),
                CategoryID = dbDataReader["category_id"].ToString(),
                Category = categoryRepo.Get(dbDataReader["category_id"].ToString().Trim()),
                YoutubeStartTimeInSeconds = Parsers.ParseInt(dbDataReader["youtube_start_time_in_seconds"].ToString().Trim())
            };
            
        }


        public Video Get(string videoID)
        {
            Video ReturnedVideo = null;
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT * FROM videos WHERE (id=@VIDEOID) OR (legacy_video_id=@VIDEOID)";
                    sqlCommand.Parameters.AddWithValue("VIDEOID", videoID);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            //throw new Exception("'" + videoID + "'");
                            ReturnedVideo = dbDataReaderToVideo(dbDataReader);
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }
            

            return ReturnedVideo;
        }

        public List<Video> GetAll()
        {
            List<Video> ReturnedVideos = new List<Video>();

            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT * FROM Videos ORDER BY date_added DESC;";
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            ReturnedVideos.Add(dbDataReaderToVideo(dbDataReader));
                        }
                    }
                    sqlCommand.Connection.Close();

                }
            }
            
            return ReturnedVideos;
        }

        public List<Video> GetNewest(bool includePrivateVideos)
        {
            return GetNewest(includePrivateVideos, int.MaxValue);
        }

        public List<Video> GetNewest(bool includePrivateVideos, int max)
        {
            string SQL = "SELECT TOP " + max + " * FROM videos WHERE ";
            if (!includePrivateVideos)
            {
                SQL += "private=0 AND ";
            }
            SQL += "hidden=0 ORDER BY date_added DESC;";

            List <Video> ReturnedVideos = new List<Video>();
            if (max > 0)
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = SQL;
                        sqlCommand.Parameters.AddWithValue("CURRENTDATETIME", DateTime.Now);
                        sqlCommand.Connection.Open();
                        SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                        if (dbDataReader.HasRows)
                        {
                            while (dbDataReader.Read())
                            {
                                ReturnedVideos.Add(dbDataReaderToVideo(dbDataReader));
                            }
                        }
                        sqlCommand.Connection.Close();

                    }
                }
            }
            return ReturnedVideos;
        }

        public List<Video> GetNewest(bool includePrivateVideos, TimeSpan thisFarBack, int max)
        {
            string SQL = "SELECT TOP " + max + " * FROM videos WHERE ";
            if (!includePrivateVideos)
            {
                SQL += "private=0 AND ";
            }
            SQL += "hidden=0 AND (date_added <= @THISFAR) ORDER BY date_added DESC;";

            List<Video> ReturnedVideos = new List<Video>();
            if (max > 0)
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
                {
                    using (SqlCommand sqlCommand = new SqlCommand())
                    {
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.CommandText = SQL;
                        sqlCommand.Parameters.AddWithValue("CURRENTDATETIME", DateTime.Now);
                        sqlCommand.Parameters.AddWithValue("THISFAR", DateTime.Now.Subtract(thisFarBack));
                        sqlCommand.Connection.Open();
                        SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                        if (dbDataReader.HasRows)
                        {
                            while (dbDataReader.Read())
                            {
                                ReturnedVideos.Add(dbDataReaderToVideo(dbDataReader));
                            }
                        }
                        sqlCommand.Connection.Close();

                    }
                }
            }
            return ReturnedVideos;
        }

        public List<Video> GetFeatured(bool includePrivateVideos)
        {
            List<Video> ReturnedVideos = new List<Video>();
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT * FROM vFeatured_videos WHERE hidden=0 AND FeaturedFrom  < @CURRENTDATETIME AND FeaturedTo > @CURRENTDATETIME ORDER BY date_added DESC;";
                    sqlCommand.Parameters.AddWithValue("CURRENTDATETIME", DateTime.Now);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            ReturnedVideos.Add(dbDataReaderToVideo(dbDataReader));
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return ReturnedVideos;
        }

        public List<Video> GetFromCategory(VideoCategory category, bool includePrivateVideos)
        {
            List<Video> returnMe = new List<Video>();

            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT * FROM videos WHERE category_id='" + category.ID + "' ORDER BY date_added DESC;";
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            returnMe.Add(dbDataReaderToVideo(dbDataReader));
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return returnMe;
        }
        
        public List<Video> Find(string searchTerms, bool includePrivateVideos)
        {
            List<Video> ReturnedVideos = new List<Video>();

            // Sanitize the input string
            string sanitizedSearchString = Sanitizers.SanitizeSearchString(searchTerms.Trim()).ToLower().Trim();

            // Build an SQL query
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT TOP 25 * FROM videos WHERE hidden=0 AND (");
            sqlQuery.Append("name like '%" + sanitizedSearchString + "%' OR ");
            sqlQuery.Append("author like '%" + sanitizedSearchString + "%' OR ");
            sqlQuery.Append("location like '%" + sanitizedSearchString + "%' OR ");
            sqlQuery.Append("description like '%" + sanitizedSearchString + "%' OR ");
            sqlQuery.Append("tags like '%" + sanitizedSearchString + "%')");
            if (!includePrivateVideos)
            {
                sqlQuery.Append(" AND private=0 ");
            }
            sqlQuery.Append(" ORDER BY date_added DESC;");

            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = sqlQuery.ToString();
                    sqlCommand.Parameters.AddWithValue("CURRENTDATETIME", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("SEARCHTERM", sanitizedSearchString);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {

                            ReturnedVideos.Add(dbDataReaderToVideo(dbDataReader));
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }
            
            return ReturnedVideos;
        }


        public string CreateNewVideoID()
        {
            string returnMe = string.Empty;
            do
            {
                returnMe = Crypto.GenerateID(videoIDLength);
            } while ((returnMe != string.Empty) && (!IsVideoIDAvailable(returnMe)));
            return returnMe;
        }

        private bool IsVideoIDAvailable(string videoID)
        {
            if (string.IsNullOrEmpty(videoID.Trim()))
            {
                return false;
            }

            bool foundVideoWithGivenID = false;
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT id FROM videos WHERE (id=@VIDEOID) OR (legacy_video_id=@VIDEOID)";
                    sqlCommand.Parameters.AddWithValue("VIDEOID", videoID.Trim());
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();
                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            foundVideoWithGivenID = true;
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return !foundVideoWithGivenID;
        }

        public bool Insert(Video video)
        {
            bool returnMe = false;

            if (string.IsNullOrEmpty(video.ID))
            {
                video.ID = CreateNewVideoID();
            }

            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadWrite))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "INSERT INTO videos(id, name, author, description, width, height, thumbnail_url, date_added, duration_in_seconds, file_mp4, file_ogv, file_webm, download_url, hidden, private, tags, category_id, youtube_url)"
                                            + "VALUES(@ID, @NAME, @AUTHOR, @DESCRIPTION, @WIDTH, @HEIGHT, @THUMB, @DATEADD, @DURATION, @MP4, @OGV, @WEBM, @DOWNLOADURL, @HIDDEN, @PRIVATE, @TAGS, @CATEGORY, @YOUTUBEURL)";

                    sqlCommand.Parameters.AddWithValue("ID", video.ID);
                    sqlCommand.Parameters.AddWithValue("NAME", video.Name);
                    sqlCommand.Parameters.AddWithValue("AUTHOR", video.Author);
                    sqlCommand.Parameters.AddWithValue("DESCRIPTION", video.Description);
                    sqlCommand.Parameters.AddWithValue("WIDTH", video.Width);
                    sqlCommand.Parameters.AddWithValue("HEIGHT", video.Height);
                    sqlCommand.Parameters.AddWithValue("DATEADD", video.DateAdded);
                    sqlCommand.Parameters.AddWithValue("DURATION", video.DurationInSeconds);
                    sqlCommand.Parameters.AddWithValue("MP4", video.FileURL_H264);
                    sqlCommand.Parameters.AddWithValue("OGV", video.FileURL_THEORA);
                    sqlCommand.Parameters.AddWithValue("WEBM", video.FileURL_VP8);
                    sqlCommand.Parameters.AddWithValue("DOWNLOADURL", video.DownloadURL);
                    sqlCommand.Parameters.AddWithValue("YOUTUBEURL", video.YoutubeURL);
                    sqlCommand.Parameters.AddWithValue("HIDDEN", video.IsHidden);
                    sqlCommand.Parameters.AddWithValue("PRIVATE", video.IsPrivate);
                    sqlCommand.Parameters.AddWithValue("TAGS", video.Tags.ToSemicolenSeparatedString());
                    sqlCommand.Parameters.AddWithValue("CATEGORY", video.CategoryID);
                    sqlCommand.Parameters.AddWithValue("THUMB", video.ThumbnailURL);

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

        public bool Update(Video video)
        {
            bool returnMe = false;

            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadWrite))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "UPDATE videos SET name=@NAME, author=@AUTHOR, description=@DESCRIPTION, width=@WIDTH, height=@HEIGHT, thumbnail_url=@THUMB, duration_in_seconds=@DURATION, file_mp4=@MP4, file_ogv=@OGV, file_webm=@WEBM, download_url=@DOWNLOADURL, hidden=@HIDDEN, private=@PRIVATE, tags=@TAGS, youtube_url=@YOUTUBEURL, category_id=@CATEGORY WHERE id=@ID";

                    sqlCommand.Parameters.AddWithValue("ID", video.ID);
                    sqlCommand.Parameters.AddWithValue("NAME", video.Name);
                    sqlCommand.Parameters.AddWithValue("AUTHOR", video.Author);
                    sqlCommand.Parameters.AddWithValue("DESCRIPTION", video.Description);
                    sqlCommand.Parameters.AddWithValue("WIDTH", video.Width);
                    sqlCommand.Parameters.AddWithValue("HEIGHT", video.Height);
                    sqlCommand.Parameters.AddWithValue("THUMB", video.ThumbnailURL);
                    sqlCommand.Parameters.AddWithValue("DURATION", video.DurationInSeconds);
                    sqlCommand.Parameters.AddWithValue("MP4", video.FileURL_H264);
                    sqlCommand.Parameters.AddWithValue("OGV", video.FileURL_THEORA);
                    sqlCommand.Parameters.AddWithValue("WEBM", video.FileURL_VP8);
                    sqlCommand.Parameters.AddWithValue("DOWNLOADURL", video.DownloadURL);
                    sqlCommand.Parameters.AddWithValue("HIDDEN", video.IsHidden);
                    sqlCommand.Parameters.AddWithValue("PRIVATE", video.IsPrivate);
                    sqlCommand.Parameters.AddWithValue("TAGS", video.Tags.ToSemicolenSeparatedString());
                    sqlCommand.Parameters.AddWithValue("CATEGORY", video.CategoryID);
                    sqlCommand.Parameters.AddWithValue("YOUTUBEURL", video.YoutubeURL);

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
                }
            }
            return returnMe;
        }

        public bool Delete(Video video)
        {
            bool returnMe = false;

            using (SqlConnection connection = new SqlConnection(DatabaseConnectionStrings.ReadWrite))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "DELETE FROM videos WHERE id=@ID";
                    sqlCommand.Parameters.AddWithValue("ID", video.ID);
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
                }
            }
            return returnMe;
        }

    }
}
