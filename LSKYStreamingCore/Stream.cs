using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LSKYStreamingCore
{
    public class Stream
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string DescriptionSmall { get; set; }
        public string DescriptionLarge { get; set; }
        public string ThumbnailURLSmall { get; set; }
        public string ThumbnailURLLarge { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ISM_URL { get; set; }
        public DateTime StreamStartTime { get; set; }
        public DateTime StreamEndTime { get; set; }
        public bool ForcedLive { get; set; }
        public bool DisplaySidebar { get; set; }
        public bool DisplayThumbnail { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }
        public string SidebarContent { get; set; }

        public TimeSpan GetTimeUntilStarts()
        {
            return this.StreamStartTime.Subtract(DateTime.Now);
        }

        public string GetTimeUntilStartsInEnglish()
        {
            double totalMinutes = this.GetTimeUntilStarts().TotalMinutes;
            if (totalMinutes == 1)
            {
                return "1 minute";
            }
            else if (totalMinutes <= 120)
            {
                return Math.Round(totalMinutes, 0) + " minutes";
            }
            else
            {
                double totalHours = this.GetTimeUntilStarts().TotalHours;
                if (totalHours == 1)
                {
                    return "1 hour";
                }
                else
                {
                    if ((totalHours % 1) == 0)
                    {

                        return Math.Round(totalHours, 0) + " hours";
                    }
                    else
                    {

                        return Math.Round(totalHours, 1) + " hours";
                    }
                }
            }
        }
        
        public Stream(string id, string name, string location, string descriptionSmall, string descriptionLarge, string thumbnailSmall, string thumbnailLarge, int width, 
            int height, string ismurl, DateTime starts, DateTime ends, bool displaySidebar, bool displayThumbnail, bool hidden, bool isprivate, bool forcelive ,string sidebarcontent) 
        {
            this.ID = id;
            this.Name = name;
            this.Location = location;
            this.DescriptionSmall = descriptionSmall;
            this.DescriptionLarge = descriptionLarge;
            this.ThumbnailURLSmall = thumbnailSmall;
            this.ThumbnailURLLarge = thumbnailLarge;
            this.Width = width;
            this.Height = height;
            this.ISM_URL = ismurl;
            this.StreamStartTime = starts;
            this.StreamEndTime = ends;
            this.DisplaySidebar = displaySidebar;
            this.DisplayThumbnail = displayThumbnail;
            this.IsHidden = hidden;
            this.IsPrivate = IsPrivate;
            this.SidebarContent = sidebarcontent;
            this.ForcedLive = forcelive;
        }

        public static bool DoesStreamIDExist(SqlConnection connection, string streamID)
        {
            bool returnMe = false;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT id FROM live_streams WHERE (id=@STREAMID)";
            sqlCommand.Parameters.AddWithValue("STREAMID", streamID);
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

        private static Stream dbDataReaderToStream(SqlDataReader dbDataReader)
        {
            return new Stream(
                dbDataReader["id"].ToString(),
                dbDataReader["name"].ToString(),
                dbDataReader["location"].ToString(),
                dbDataReader["description_small"].ToString(),
                dbDataReader["description_large"].ToString(),
                dbDataReader["thumbnail_url_small"].ToString(),
                dbDataReader["thumbnail_url_large"].ToString(),
                LSKYCommon.ParseDatabaseInt(dbDataReader["Width"].ToString()),
                LSKYCommon.ParseDatabaseInt(dbDataReader["Height"].ToString()),
                dbDataReader["isml_url"].ToString(),
                LSKYCommon.ParseDatabaseDateTime(dbDataReader["stream_start"].ToString()),
                LSKYCommon.ParseDatabaseDateTime(dbDataReader["stream_end"].ToString()),
                LSKYCommon.ParseDatabaseBool(dbDataReader["display_sidebar"].ToString(), false),
                LSKYCommon.ParseDatabaseBool(dbDataReader["display_thumbnail"].ToString(), false),
                LSKYCommon.ParseDatabaseBool(dbDataReader["hidden"].ToString(), false),
                LSKYCommon.ParseDatabaseBool(dbDataReader["private"].ToString(), false),
                LSKYCommon.ParseDatabaseBool(dbDataReader["force_online"].ToString(), false),
                dbDataReader["sidebar_content"].ToString()
                );
        }

        public static List<Stream> LoadAllStreams(SqlConnection connection)
        {
            List<Stream> ReturnedStreams = new List<Stream>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM live_streams;";
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    ReturnedStreams.Add(dbDataReaderToStream(dbDataReader));
                }
            }

            sqlCommand.Connection.Close();

            return ReturnedStreams;
        }

        public static List<Stream> LoadUpcomingStreams(SqlConnection connection)
        {
            List<Stream> ReturnedStreams = new List<Stream>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM live_streams WHERE stream_start > @CURRENTDATETIME AND force_online=0 AND private=0 AND hidden=0 ORDER BY stream_start ASC, name ASC;";
            sqlCommand.Parameters.AddWithValue("@CURRENTDATETIME", DateTime.Now);
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    ReturnedStreams.Add(dbDataReaderToStream(dbDataReader));
                }
            }

            sqlCommand.Connection.Close();

            return ReturnedStreams;
        }

        public static List<Stream> LoadCurrentlyBroadcasting(SqlConnection connection)
        {
            List<Stream> ReturnedStreams = new List<Stream>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM live_streams WHERE ((stream_start < @CURRENTDATETIME AND stream_end > @CURRENTDATETIME) OR (force_online=1)) AND hidden=0 AND private=0 ORDER BY stream_start ASC, name ASC;";
            sqlCommand.Parameters.AddWithValue("@CURRENTDATETIME", DateTime.Now.AddMinutes(10));
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    ReturnedStreams.Add(dbDataReaderToStream(dbDataReader));
                }
            }

            sqlCommand.Connection.Close();

            return ReturnedStreams;
        }

        public static Stream LoadThisStream(SqlConnection connection, string streamID)
        {
            Stream ReturnedStream = null;

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM live_streams WHERE id=@STREAMID;";
            sqlCommand.Parameters.AddWithValue("STREAMID", streamID);
            sqlCommand.Connection.Open();
            SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

            if (dbDataReader.HasRows)
            {
                while (dbDataReader.Read())
                {
                    ReturnedStream = dbDataReaderToStream(dbDataReader);
                }
            }

            sqlCommand.Connection.Close();

            return ReturnedStream;
        }

        public bool IsStreamLive()
        {
            if ((DateTime.Now > this.StreamStartTime) && (DateTime.Now < this.StreamEndTime))
            {
                return true;
            }
            else if (this.ForcedLive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetExpectedDuration()
        {
            TimeSpan streamDuration = this.StreamEndTime.Subtract(this.StreamStartTime);

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

                        return Math.Round(streamDuration_Hours,0) + " hours";
                    }
                    else
                    {

                        return Math.Round(streamDuration_Hours,1) + " hours";
                    }
                }
            }

        }
    }
}
