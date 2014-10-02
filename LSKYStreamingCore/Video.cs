using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace LSKYStreamingCore
{
    public class Video
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateAired { get; set; }
        public int DurationInSeconds { get; set; }
        public string FileURL_ISM { get; set; }
        public string FileURL_H264 { get; set; }
        public string FileURL_THEORA { get; set; }
        public string FileURL_VP8 { get; set; }
        public string DownloadURL { get; set; }
        public bool IsAlwaysAvailable { get; set; }
        public bool ShouldDisplayAirDate { get; set; }
        public bool ShouldDisplayThumbnail { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsLiveStreamRecording { get; set; }
        public bool AllowEmbedding { get; set; }
        public string Tags { get; set; }
        public string LegacyVideoID { get; set; }
        public DateTime DateAvailable { get; set; }
        public DateTime DateExpires { get; set; }
        public string ThumbnailURL { get; set; }
        public string CategoryID { get; set; }
        public VideoCategory Category { get; set; }
        public TimeSpan Duration { 
            get {
                return new TimeSpan(0,0,this.DurationInSeconds);
                } 

            set {
                this.DurationInSeconds = (int)value.TotalSeconds;
                } 
        }
        public bool Expires
        {
            get
            {
                return !this.IsAlwaysAvailable;
            }
        }

        public Video(string id, string name, string author, string location, string description, int width, int height, string thumbnail, DateTime added, DateTime aired,
            int duration, string file_ism, string file_mp4, string file_ogv, string file_webm, string downloadurl, bool displayairdate, bool displaythumbnail, bool ishidden, bool isprivate, bool islivestreamrecording, bool allowembed,
            string tags, DateTime dateavailable, DateTime dateexpires, bool isalwaysavailable, string legacy_video_id, string category_id)
        {
            this.ID = id;
            this.Name = name;
            this.Author = author;
            this.Location = location;
            this.Description = description;
            this.Width = width;
            this.Height = height;
            this.ThumbnailURL = thumbnail;
            this.DateAdded = added;
            this.DateAired = aired;
            this.DurationInSeconds = duration;
            this.FileURL_ISM = file_ism;
            this.FileURL_H264 = file_mp4;
            this.FileURL_THEORA = file_ogv;
            this.FileURL_VP8 = file_webm;
            this.DownloadURL = downloadurl;
            this.ShouldDisplayAirDate = displayairdate;
            this.ShouldDisplayThumbnail = displaythumbnail;
            this.IsHidden = ishidden;
            this.IsPrivate = isprivate;
            this.IsLiveStreamRecording = islivestreamrecording;
            this.AllowEmbedding = allowembed;
            this.Tags = tags;
            this.DateAvailable = dateavailable;
            this.DateExpires = dateexpires;
            this.IsAlwaysAvailable = isalwaysavailable;
            this.LegacyVideoID = legacy_video_id;
            this.CategoryID = category_id;
        }

        public bool IsHTML5Available()
        {
            if (
                (string.IsNullOrEmpty(this.FileURL_H264)) &&
                (string.IsNullOrEmpty(this.FileURL_THEORA)) &&
                (string.IsNullOrEmpty(this.FileURL_VP8)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsSilverlightAvailable()
        {
            if (string.IsNullOrEmpty(this.FileURL_ISM))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        private static List<VideoCategory> VideoCategoryCache;
        private static DateTime VideoCategoryCacheLastUpdated;
        private static List<VideoCategory> GetVideoCategoryCache(SqlConnection connection)
        {
            if ((VideoCategoryCache == null) || (DateTime.Now.Subtract(VideoCategoryCacheLastUpdated) > new TimeSpan(0, 5, 0)))
            {
                VideoCategoryCacheLastUpdated = DateTime.Now;
                VideoCategoryCache = VideoCategory.LoadAll(connection, false);
            }
            
            return VideoCategoryCache;            
        }

        private static Video dbDataReaderToVideo(SqlDataReader dbDataReader)
        {
            Video returnedVideo = new Video(
                dbDataReader["id"].ToString(),
                dbDataReader["name"].ToString(),
                dbDataReader["author"].ToString(),
                dbDataReader["location"].ToString(),
                dbDataReader["description"].ToString(),
                LSKYCommon.ParseDatabaseInt(dbDataReader["width"].ToString()),
                LSKYCommon.ParseDatabaseInt(dbDataReader["height"].ToString()),
                dbDataReader["thumbnail_url"].ToString(),
                LSKYCommon.ParseDatabaseDateTime(dbDataReader["date_added"].ToString()),
                LSKYCommon.ParseDatabaseDateTime(dbDataReader["date_aired"].ToString()),
                LSKYCommon.ParseDatabaseInt(dbDataReader["duration_in_seconds"].ToString()),
                dbDataReader["file_ism"].ToString(),
                dbDataReader["file_mp4"].ToString(),
                dbDataReader["file_ogv"].ToString(),
                dbDataReader["file_webm"].ToString(),
                dbDataReader["download_url"].ToString(),
                LSKYCommon.ParseDatabaseBool(dbDataReader["display_airdate"].ToString(), false),
                LSKYCommon.ParseDatabaseBool(dbDataReader["display_thumbnail"].ToString(), false),
                LSKYCommon.ParseDatabaseBool(dbDataReader["hidden"].ToString(), false),
                LSKYCommon.ParseDatabaseBool(dbDataReader["private"].ToString(), false),
                LSKYCommon.ParseDatabaseBool(dbDataReader["was_originally_live_stream"].ToString(), false),
                LSKYCommon.ParseDatabaseBool(dbDataReader["allow_embed"].ToString(), false),
                dbDataReader["tags"].ToString(),
                LSKYCommon.ParseDatabaseDateTime(dbDataReader["available_from"].ToString()),
                LSKYCommon.ParseDatabaseDateTime(dbDataReader["available_to"].ToString()),
                LSKYCommon.ParseDatabaseBool(dbDataReader["always_available"].ToString(), false),
                dbDataReader["legacy_video_id"].ToString(),
                dbDataReader["category_id"].ToString()
                );

            return returnedVideo;
        }

        public static bool DoesVideoIDExist(SqlConnection connection, string videoID)
        {
            bool returnMe = false;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT id FROM videos WHERE (id=@VIDEOID) OR (legacy_video_id=@VIDEOID)";
            sqlCommand.Parameters.AddWithValue("VIDEOID", videoID);
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
        
        public static Video Load(SqlConnection connection, string videoID)
        {
            Video ReturnedVideo = null;

            SqlCommand sqlCommand = new SqlCommand();
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

            // Get this video's category
            if (ReturnedVideo != null)
            {
                foreach (VideoCategory cat in GetVideoCategoryCache(connection))
                {
                    if (ReturnedVideo.CategoryID == cat.ID)
                    {
                        ReturnedVideo.Category = cat;
                    }
                }
            }

            return ReturnedVideo;
        }

        public static List<Video> LoadAll(SqlConnection connection)
        {
            List<Video> ReturnedVideos = new List<Video>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM videos;";
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

            // Associate video categories
            foreach(Video video in ReturnedVideos) 
            {
                foreach (VideoCategory cat in GetVideoCategoryCache(connection))
                {
                    if (video.CategoryID == cat.ID)
                    {
                        video.Category = cat;
                    }
                }
            }

            return ReturnedVideos;
        }

        public static List<Video> LoadFromCategory(SqlConnection connection, VideoCategory category, bool includePrivate)
        {
            return LoadFromCategory(connection, category, 10000, includePrivate);
        }
        
        public static List<Video> LoadFromCategory(SqlConnection connection, VideoCategory category, int maxVideos, bool includePrivate)
        {
            List<Video> ReturnedVideos = new List<Video>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT TOP " + maxVideos + " * FROM videos WHERE category_id='" + category.ID + "' ORDER BY date_added DESC;";
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

            // Should the list include private videos or not?
            List<Video> filteredVideos = new List<Video>();
            if (includePrivate)
            {
                filteredVideos = ReturnedVideos;
            }
            else
            {
                filteredVideos = ReturnedVideos.Where(v => v.IsPrivate == false).ToList();
            }

            // Associate video categories
            foreach (Video video in filteredVideos)
            {
                foreach (VideoCategory cat in GetVideoCategoryCache(connection))
                {
                    if (video.CategoryID == cat.ID)
                    {
                        video.Category = cat;
                    }
                }
            }

            return filteredVideos;
        }

        public static List<Video> Find(SqlConnection connection, string searchTerms, bool includePrivate)
        {
            List<Video> ReturnedVideos = new List<Video>();

            // Sanitize the input string
            string sanitizedSearchString = LSKYCommon.SanitizeSearchString(searchTerms.Trim()).ToLower().Trim();
            
            // Build an SQL query
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT TOP 25 * FROM videos WHERE hidden=0 AND private=0 AND ((available_from < @CURRENTDATETIME AND available_to > @CURRENTDATETIME) OR (always_available=1)) AND (");
            sqlQuery.Append("name like '%" + sanitizedSearchString + "%' OR ");
            sqlQuery.Append("author like '%" + sanitizedSearchString + "%' OR ");
            sqlQuery.Append("location like '%" + sanitizedSearchString + "%' OR ");
            sqlQuery.Append("description_small like '%" + sanitizedSearchString + "%' OR ");
            sqlQuery.Append("description_large like '%" + sanitizedSearchString + "%' OR ");
            sqlQuery.Append("tags like '%" + sanitizedSearchString + "%')");
            sqlQuery.Append(" ORDER BY date_added DESC;");

            SqlCommand sqlCommand = new SqlCommand();
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

            // Should the list include private videos or not?
            List<Video> filteredVideos = new List<Video>();
            if (includePrivate)
            {
                filteredVideos = ReturnedVideos;
            }
            else
            {
                filteredVideos = ReturnedVideos.Where(v => v.IsPrivate == false).ToList();
            }

            // Associate video categories
            foreach (Video video in filteredVideos)
            {
                foreach (VideoCategory cat in GetVideoCategoryCache(connection))
                {
                    if (video.CategoryID == cat.ID)
                    {
                        video.Category = cat;
                    }
                }
            }

            return filteredVideos;
        }

        public static List<Video> LoadFeatured(SqlConnection connection, bool loadPrivate)
        {
            List<Video> ReturnedVideos = new List<Video>();

            SqlCommand sqlCommand = new SqlCommand();
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

            // Should the list include private videos or not?
            List<Video> filteredVideos = new List<Video>();
            if (loadPrivate)
            {
                filteredVideos = ReturnedVideos;
            }
            else
            {
                filteredVideos = ReturnedVideos.Where(v => v.IsPrivate == false).ToList();
            }

            // Associate video categories
            foreach (Video video in filteredVideos)
            {
                foreach (VideoCategory cat in GetVideoCategoryCache(connection))
                {
                    if (video.CategoryID == cat.ID)
                    {
                        video.Category = cat;
                    }
                }
            }

            return filteredVideos;
        }

        public static List<Video> LoadPublic(SqlConnection connection)
        {
            List<Video> ReturnedVideos = new List<Video>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM videos WHERE hidden=0 AND ((available_from < @CURRENTDATETIME AND available_to > @CURRENTDATETIME) OR (always_available=1)) ORDER BY date_added;";
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

            // Associate video categories
            foreach (Video video in ReturnedVideos)
            {
                foreach (VideoCategory cat in GetVideoCategoryCache(connection))
                {
                    if (video.CategoryID == cat.ID)
                    {
                        video.Category = cat;
                    }
                }
            }

            return ReturnedVideos;
        }

        public static List<Video> LoadNewest(SqlConnection connection, bool loadPrivate)
        {
            List<Video> ReturnedVideos = new List<Video>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT TOP 15 * FROM videos WHERE hidden=0 AND ((available_from < @CURRENTDATETIME AND available_to > @CURRENTDATETIME) OR (always_available=1)) ORDER BY date_added DESC;";
            sqlCommand.Parameters.AddWithValue("CURRENTDATETIME", DateTime.Now);
            sqlCommand.Connection.Open();            
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();
            //throw new Exception(sqlCommand.CommandText);
            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    ReturnedVideos.Add(dbDataReaderToVideo(dbDataReader));
                }
            }

            sqlCommand.Connection.Close();

            // Should the list include private videos or not?
            List<Video> filteredVideos = new List<Video>();
            if (loadPrivate)
            {
                filteredVideos = ReturnedVideos;
            }
            else
            {
                filteredVideos = ReturnedVideos.Where(v => v.IsPrivate == false).ToList();
            }

            // Associate video categories
            foreach (Video video in filteredVideos)
            {
                foreach (VideoCategory cat in GetVideoCategoryCache(connection))
                {
                    if (video.CategoryID == cat.ID)
                    {
                        video.Category = cat;
                    }
                }
            }

            return filteredVideos;
        }

        public string GetDurationInEnglish()
        {
            TimeSpan streamDuration = new TimeSpan(0, 0, this.DurationInSeconds);
            
            double streamDuration_Minutes = streamDuration.TotalMinutes;
            if (streamDuration_Minutes == 1)
            {
                return "1 minute";
            }
            else if (streamDuration_Minutes <= 120)
            {
                return Math.Round(streamDuration_Minutes, 0) + " minutes";
            }
            else
            {
                double streamDuration_Hours = streamDuration.TotalHours;
                if (streamDuration_Hours == 1)
                {
                    return "1 hour";
                }
                else
                {
                    if ((streamDuration_Hours % 1) == 0)
                    {

                        return Math.Round(streamDuration_Hours, 0) + " hours";
                    }
                    else
                    {

                        return Math.Round(streamDuration_Hours, 1) + " hours";
                    }
                }
            }

        }

        public string GetFullCategory()
        {
            if (this.Category != null)
            {
                return this.Category.GetFullName();
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool Insert(SqlConnection connection, Video video)
        {
            bool returnMe = false;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "INSERT INTO videos(id, name, author, location, description, width, height, thumbnail_url, date_added, date_aired, duration_in_seconds, file_ism, file_mp4, file_ogv, file_webm, display_airdate, download_url, display_thumbnail, hidden, private, was_originally_live_stream, allow_embed, tags, available_From, available_to, always_available, legacy_video_id, category_id)"
                                    + "VALUES(@ID, @NAME, @AUTHOR, @LOCATION, @DESCRIPTION, @WIDTH, @HEIGHT, @THUMB, @DATEADD, @DATEAIRED, @DURATION, @ISM, @MP4, @OGV, @WEBM, @DISPLAYAIRDATE, @DOWNLOADURL, @DISPLAYTHUMB, @HIDDEN, @PRIVATE, @ORIGINALLYLIVE, @ALLOWEMBED, @TAGS, @AVAILFROM, @AVAILTO, @ALWAYSAVAIL, @LEGACYID, @CATEGORY)";

            sqlCommand.Parameters.AddWithValue("ID", video.ID);
            sqlCommand.Parameters.AddWithValue("NAME", video.Name);
            sqlCommand.Parameters.AddWithValue("AUTHOR", video.Author);
            sqlCommand.Parameters.AddWithValue("LOCATION", video.Location);
            sqlCommand.Parameters.AddWithValue("DESCRIPTION", video.Description);
            sqlCommand.Parameters.AddWithValue("WIDTH", video.Width);
            sqlCommand.Parameters.AddWithValue("HEIGHT", video.Height);
            sqlCommand.Parameters.AddWithValue("THUMB", video.ThumbnailURL);
            sqlCommand.Parameters.AddWithValue("DATEADD", video.DateAdded);
            sqlCommand.Parameters.AddWithValue("DATEAIRED", video.DateAired);
            sqlCommand.Parameters.AddWithValue("DURATION", video.DurationInSeconds);
            sqlCommand.Parameters.AddWithValue("ISM", video.FileURL_ISM);
            sqlCommand.Parameters.AddWithValue("MP4", video.FileURL_H264);
            sqlCommand.Parameters.AddWithValue("OGV", video.FileURL_THEORA);
            sqlCommand.Parameters.AddWithValue("WEBM", video.FileURL_VP8);
            sqlCommand.Parameters.AddWithValue("DISPLAYAIRDATE", video.ShouldDisplayAirDate);
            sqlCommand.Parameters.AddWithValue("DOWNLOADURL", video.DownloadURL);
            sqlCommand.Parameters.AddWithValue("DISPLAYTHUMB", video.ShouldDisplayThumbnail);
            sqlCommand.Parameters.AddWithValue("HIDDEN", video.IsHidden);
            sqlCommand.Parameters.AddWithValue("PRIVATE", video.IsPrivate);
            sqlCommand.Parameters.AddWithValue("ORIGINALLYLIVE", video.IsLiveStreamRecording);
            sqlCommand.Parameters.AddWithValue("ALLOWEMBED", video.AllowEmbedding);
            sqlCommand.Parameters.AddWithValue("TAGS", video.Tags);
            sqlCommand.Parameters.AddWithValue("AVAILFROM", video.DateAvailable);
            sqlCommand.Parameters.AddWithValue("AVAILTO", video.DateExpires);
            sqlCommand.Parameters.AddWithValue("ALWAYSAVAIL", video.IsAlwaysAvailable);
            sqlCommand.Parameters.AddWithValue("LEGACYID", video.LegacyVideoID);
            sqlCommand.Parameters.AddWithValue("CATEGORY", video.CategoryID);

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

        public static bool Update(SqlConnection connection, Video video)
        {
            bool returnMe = false;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "UPDATE videos SET name=@NAME, author=@AUTHOR, location=@LOCATION, description=@DESCRIPTION, width=@WIDTH, height=@HEIGHT, thumbnail_url=@THUMB, date_added=@DATEADD, date_aired=@DATEAIRED, duration_in_seconds=@DURATION, file_ism=@ISM, file_mp4=@MP4, file_ogv=@OGV, file_webm=@WEBM, display_airdate=@DISPLAYAIRDATE, download_url=@DOWNLOADURL, display_thumbnail=@DISPLAYTHUMB, hidden=@HIDDEN, private=@PRIVATE, was_originally_live_stream=@ORIGINALLYLIVE, allow_embed=@ALLOWEMBED, tags=@TAGS, available_From=@AVAILFROM, available_to=@AVAILTO, always_available=@ALWAYSAVAIL, legacy_video_id=@LEGACYID, category_id=@CATEGORY WHERE id=@ID";

            sqlCommand.Parameters.AddWithValue("ID", video.ID);
            sqlCommand.Parameters.AddWithValue("NAME", video.Name);
            sqlCommand.Parameters.AddWithValue("AUTHOR", video.Author);
            sqlCommand.Parameters.AddWithValue("LOCATION", video.Location);
            sqlCommand.Parameters.AddWithValue("DESCRIPTION", video.Description);
            sqlCommand.Parameters.AddWithValue("WIDTH", video.Width);
            sqlCommand.Parameters.AddWithValue("HEIGHT", video.Height);
            sqlCommand.Parameters.AddWithValue("THUMB", video.ThumbnailURL);
            sqlCommand.Parameters.AddWithValue("DATEADD", video.DateAdded);
            sqlCommand.Parameters.AddWithValue("DATEAIRED", video.DateAired);
            sqlCommand.Parameters.AddWithValue("DURATION", video.DurationInSeconds);
            sqlCommand.Parameters.AddWithValue("ISM", video.FileURL_ISM);
            sqlCommand.Parameters.AddWithValue("MP4", video.FileURL_H264);
            sqlCommand.Parameters.AddWithValue("OGV", video.FileURL_THEORA);
            sqlCommand.Parameters.AddWithValue("WEBM", video.FileURL_VP8);
            sqlCommand.Parameters.AddWithValue("DISPLAYAIRDATE", video.ShouldDisplayAirDate);
            sqlCommand.Parameters.AddWithValue("DOWNLOADURL", video.DownloadURL);
            sqlCommand.Parameters.AddWithValue("DISPLAYTHUMB", video.ShouldDisplayThumbnail);
            sqlCommand.Parameters.AddWithValue("HIDDEN", video.IsHidden);
            sqlCommand.Parameters.AddWithValue("PRIVATE", video.IsPrivate);
            sqlCommand.Parameters.AddWithValue("ORIGINALLYLIVE", video.IsLiveStreamRecording);
            sqlCommand.Parameters.AddWithValue("ALLOWEMBED", video.AllowEmbedding);
            sqlCommand.Parameters.AddWithValue("TAGS", video.Tags);
            sqlCommand.Parameters.AddWithValue("AVAILFROM", video.DateAvailable);
            sqlCommand.Parameters.AddWithValue("AVAILTO", video.DateExpires);
            sqlCommand.Parameters.AddWithValue("ALWAYSAVAIL", video.IsAlwaysAvailable);
            sqlCommand.Parameters.AddWithValue("LEGACYID", video.LegacyVideoID);
            sqlCommand.Parameters.AddWithValue("CATEGORY", video.CategoryID);

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
