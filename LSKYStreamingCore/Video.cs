using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LSKYStreamingCore
{
    public class Video
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Location { get; set; }
        public string DescriptionSmall { get; set; }
        public string DescriptionLarge { get; set; }
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

        public Video(string id, string name, string author, string location, string descriptionsmall, string descriptionlarge, int width, int height, string thumbnail, DateTime added, DateTime aired,
            int duration, string file_ism, string file_mp4, string file_ogv, string file_webm, string downloadurl, bool displayairdate, bool displaythumbnail, bool ishidden, bool isprivate, bool islivestreamrecording, bool allowembed,
            string tags, DateTime dateavailable, DateTime dateexpires, bool isalwaysavailable, string legacy_video_id, string category_id)
        {
            this.ID = id;
            this.Name = name;
            this.Author = author;
            this.Location = location;
            this.DescriptionSmall = descriptionsmall;
            this.DescriptionLarge = descriptionlarge;
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

        private static Video dbDataReaderToVideo(SqlDataReader dbDataReader)
        {
            return new Video(
                dbDataReader["id"].ToString(),
                dbDataReader["name"].ToString(),
                dbDataReader["author"].ToString(),
                dbDataReader["location"].ToString(),
                dbDataReader["description_small"].ToString(),
                dbDataReader["description_large"].ToString(),
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

        public static Video LoadThisVideo(SqlConnection connection, string videoID)
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

            return ReturnedVideo;
        }

        public static List<Video> LoadAllVideos(SqlConnection connection)
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

            return ReturnedVideos;
        }

        public static List<Video> LoadVideosFromCategory(SqlConnection connection, VideoCategory category, int maxVideos)
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

            return ReturnedVideos;
        }

        public static List<Video> SearchVideos(SqlConnection connection, string searchTerms)
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

            return ReturnedVideos;
        }

        public static List<Video> LoadFeaturedVideos(SqlConnection connection)
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

            return ReturnedVideos;
        }

        public static List<Video> LoadPublicVideos(SqlConnection connection)
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

            return ReturnedVideos;
        }

        public static List<Video> LoadNewestVideos(SqlConnection connection)
        {
            List<Video> ReturnedVideos = new List<Video>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT TOP 25 * FROM videos WHERE hidden=0 AND ((available_from < @CURRENTDATETIME AND available_to > @CURRENTDATETIME) OR (always_available=1)) ORDER BY date_added DESC;";
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

            return ReturnedVideos;
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
    

    }
}
