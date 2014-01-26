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
        public string FileURL_MP4 { get; set; }
        public string FileURL_OGV { get; set; }
        public string FileURL_WEBM { get; set; }
        public string DownloadURL { get; set; }
        public bool IsAlwaysAvailable { get; set; }
        public bool ShouldDisplayAirDate { get; set; }
        public bool ShouldDisplayThumbnail { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsLiveStreamRecording { get; set; }
        public bool AllowEmbedding { get; set; }
        public string CategoryID { get; set; }
        public string LegacyVideoID { get; set; }
        public DateTime DateAvailable { get; set; }
        public DateTime DateExpires { get; set; }
        public string ThumbnailURLSmall { get; set; }
        public string ThumbnailURLLarge { get; set; }
        public TimeSpan Duration { 
            get {
                return new TimeSpan(0,0,this.DurationInSeconds);
                } 

            set {
                this.DurationInSeconds = (int)value.TotalSeconds;
                } 
        }

        public Video(string id, string name, string author, string location, string descriptionsmall, string descriptionlarge, int width, int height, string thumbnailsmall, string thumbnaillarge, DateTime added, DateTime aired,
            int duration, string file_ism, string file_mp4, string file_ogv, string file_webm, string downloadurl, bool displayairdate, bool displaythumbnail, bool ishidden, bool isprivate, bool islivestreamrecording, bool allowembed,
            string categoryid, DateTime dateavailable, DateTime dateexpires, bool isalwaysavailable, string legacy_video_id)
        {
            this.ID = id;
            this.Name = name;
            this.Author = author;
            this.Location = location;
            this.DescriptionSmall = descriptionsmall;
            this.DescriptionLarge = descriptionlarge;
            this.Width = width;
            this.Height = height;
            this.ThumbnailURLSmall = thumbnailsmall;
            this.ThumbnailURLLarge = thumbnaillarge;
            this.DateAdded = added;
            this.DateAired = aired;
            this.DurationInSeconds = DurationInSeconds;
            this.FileURL_ISM = file_ism;
            this.FileURL_MP4 = file_mp4;
            this.FileURL_OGV = file_ogv;
            this.FileURL_WEBM = file_webm;
            this.DownloadURL = downloadurl;
            this.ShouldDisplayAirDate = displayairdate;
            this.ShouldDisplayThumbnail = displaythumbnail;
            this.IsHidden = ishidden;
            this.IsPrivate = isprivate;
            this.IsLiveStreamRecording = islivestreamrecording;
            this.AllowEmbedding = allowembed;
            this.CategoryID = categoryid;
            this.DateAvailable = dateavailable;
            this.DateExpires = dateexpires;
            this.IsAlwaysAvailable = isalwaysavailable;
            this.LegacyVideoID = legacy_video_id;
        }

        public bool IsHTML5Available()
        {
            if (
                (string.IsNullOrEmpty(this.FileURL_MP4)) &&
                (string.IsNullOrEmpty(this.FileURL_OGV)) &&
                (string.IsNullOrEmpty(this.FileURL_WEBM)))
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
                dbDataReader["thumbnail_url_small"].ToString(),
                dbDataReader["thumbnail_url_large"].ToString(),
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
                dbDataReader["category_id"].ToString(),
                LSKYCommon.ParseDatabaseDateTime(dbDataReader["available_from"].ToString()),
                LSKYCommon.ParseDatabaseDateTime(dbDataReader["available_to"].ToString()),
                LSKYCommon.ParseDatabaseBool(dbDataReader["always_available"].ToString(), false),
                dbDataReader["legacy_video_id"].ToString()
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

        public static List<Video> LoadVideosFromCategory(SqlConnection connection, string category_id)
        {
            List<Video> ReturnedVideos = new List<Video>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM videos WHERE category_id=@CATID;";
            sqlCommand.Parameters.AddWithValue("CATID", category_id);
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
            sqlCommand.CommandText = "SELECT * FROM vFeatured_videos WHERE hidden=0 AND FeaturedFrom  < @CURRENTDATETIME AND FeaturedTo > @CURRENTDATETIME;";
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
            TimeSpan streamDuration = this.Duration;

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
