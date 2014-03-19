using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LSKYStreamingCore
{
    public class LiveBroadcast
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string DescriptionSmall { get; set; }
        public string DescriptionLarge { get; set; }
        public string ThumbnailURL { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ISM_URL { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool ForcedLive { get; set; }
        public bool DisplaySidebar { get; set; }
        public bool DisplayThumbnail { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }
        public string SidebarContent { get; set; }

        public TimeSpan GetTimeUntilStarts()
        {
            return this.StartTime.Subtract(DateTime.Now);
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
        
        public LiveBroadcast(string id, string name, string location, string descriptionSmall, string descriptionLarge, string thumbnail, int width, 
            int height, string ismurl, DateTime starts, DateTime ends, bool displaySidebar, bool displayThumbnail, bool hidden, bool isprivate, bool forcelive ,string sidebarcontent) 
        {
            this.ID = id;
            this.Name = name;
            this.Location = location;
            this.DescriptionSmall = descriptionSmall;
            this.DescriptionLarge = descriptionLarge;
            this.ThumbnailURL = thumbnail;
            this.Width = width;
            this.Height = height;
            this.ISM_URL = ismurl;
            this.StartTime = starts;
            this.EndTime = ends;
            this.DisplaySidebar = displaySidebar;
            this.DisplayThumbnail = displayThumbnail;
            this.IsHidden = hidden;
            this.IsPrivate = IsPrivate;
            this.SidebarContent = sidebarcontent;
            this.ForcedLive = forcelive;
        }

        public static bool DoesIDExist(SqlConnection connection, string streamID)
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

        private static LiveBroadcast dbDataReaderToStream(SqlDataReader dbDataReader)
        {
            return new LiveBroadcast(
                dbDataReader["id"].ToString(),
                dbDataReader["name"].ToString(),
                dbDataReader["location"].ToString(),
                dbDataReader["description_small"].ToString(),
                dbDataReader["description_large"].ToString(),
                dbDataReader["thumbnail_url"].ToString(),
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

        public static List<LiveBroadcast> LoadAll(SqlConnection connection)
        {
            List<LiveBroadcast> ReturnedStreams = new List<LiveBroadcast>();

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

            return ReturnedStreams.OrderByDescending(c => c.StartTime).ThenByDescending(c => c.EndTime).ToList<LiveBroadcast>();
        }

        public static List<LiveBroadcast> LoadAll(SqlConnection connection, int Max)
        {
            List<LiveBroadcast> ReturnedStreams = new List<LiveBroadcast>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT TOP " + Max + " * FROM live_streams ORDER BY stream_start DESC, stream_end DESC;";
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

            return ReturnedStreams.OrderByDescending(c => c.StartTime).ThenByDescending(c => c.EndTime).ToList<LiveBroadcast>();
        }

        public static List<LiveBroadcast> LoadUpcoming(SqlConnection connection)
        {
            List<LiveBroadcast> ReturnedStreams = new List<LiveBroadcast>();

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

        public static List<LiveBroadcast> LoadCurrentlyBroadcasting(SqlConnection connection, int minutesAhead)
        {
            List<LiveBroadcast> ReturnedStreams = new List<LiveBroadcast>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "SELECT * FROM live_streams WHERE ((stream_start < @CURRENTDATETIME AND stream_end > @CURRENTDATETIME) OR (force_online=1)) AND hidden=0 AND private=0 ORDER BY stream_start ASC, name ASC;";
            sqlCommand.Parameters.AddWithValue("@CURRENTDATETIME", DateTime.Now.AddMinutes(minutesAhead));
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

        public static LiveBroadcast LoadThisBroadcast(SqlConnection connection, string streamID)
        {
            LiveBroadcast ReturnedStream = null;

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

        public static bool InsertNewBroadcast(SqlConnection connection, LiveBroadcast newBroadcast)
        {
            bool returnMe = false;

            List<LiveBroadcast> ReturnedStreams = new List<LiveBroadcast>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "INSERT INTO live_streams(id,name,location,description_small,description_large,thumbnail_url,width,height,isml_url,stream_start,stream_end,display_sidebar,display_thumbnail,hidden,private,force_online,sidebar_content)"
                                    + "VALUES(@ID,@NAME,@LOC,@DESCSMALL,@DESCLARGE,@THUMB,@WIDTH,@HEIGHT,@ISML,@STARTS,@ENDS,@SHOWSIDEBAR,@SHOWTHUMB,@ISHIDDEN,@ISPRIVATE,@FORCEONLINE,@SIDEBAR)";
            sqlCommand.Parameters.AddWithValue("ID", newBroadcast.ID);
            sqlCommand.Parameters.AddWithValue("NAME", newBroadcast.Name);
            sqlCommand.Parameters.AddWithValue("LOC", newBroadcast.Location);
            sqlCommand.Parameters.AddWithValue("DESCSMALL", newBroadcast.DescriptionSmall);
            sqlCommand.Parameters.AddWithValue("DESCLARGE", newBroadcast.DescriptionLarge);
            sqlCommand.Parameters.AddWithValue("THUMB", newBroadcast.ThumbnailURL);
            sqlCommand.Parameters.AddWithValue("WIDTH", newBroadcast.Width);
            sqlCommand.Parameters.AddWithValue("HEIGHT", newBroadcast.Height);
            sqlCommand.Parameters.AddWithValue("ISML", newBroadcast.ISM_URL);
            sqlCommand.Parameters.AddWithValue("STARTS", newBroadcast.StartTime);
            sqlCommand.Parameters.AddWithValue("ENDS", newBroadcast.EndTime);
            sqlCommand.Parameters.AddWithValue("SHOWSIDEBAR", newBroadcast.DisplaySidebar);
            sqlCommand.Parameters.AddWithValue("SHOWTHUMB", newBroadcast.DisplayThumbnail);
            sqlCommand.Parameters.AddWithValue("ISHIDDEN", newBroadcast.IsHidden);
            sqlCommand.Parameters.AddWithValue("ISPRIVATE", newBroadcast.IsPrivate);
            sqlCommand.Parameters.AddWithValue("FORCEONLINE", newBroadcast.ForcedLive);
            sqlCommand.Parameters.AddWithValue("SIDEBAR", newBroadcast.SidebarContent);
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

        public static bool UpdateBroadcast(SqlConnection connection, LiveBroadcast newBroadcast)
        {
            bool returnMe = false;

            List<LiveBroadcast> ReturnedStreams = new List<LiveBroadcast>();

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = "UPDATE live_streams SET name=@NAME, location=@LOC, description_small=@DESCSMALL, description_large=@DESCLARGE, thumbnail_url=@THUMB, width=@WIDTH, height=@HEIGHT,"
                                        + " isml_url=@ISML, stream_start=@STARTS, stream_end=@ENDS, display_sidebar=@SHOWSIDEBAR, display_thumbnail=@SHOWTHUMB, hidden=@ISHIDDEN, private=@ISPRIVATE, force_online=@FORCEONLINE, sidebar_content=@SIDEBAR"
                                        + " WHERE id=@ID";
            sqlCommand.Parameters.AddWithValue("ID", newBroadcast.ID);
            sqlCommand.Parameters.AddWithValue("NAME", newBroadcast.Name);
            sqlCommand.Parameters.AddWithValue("LOC", newBroadcast.Location);
            sqlCommand.Parameters.AddWithValue("DESCSMALL", newBroadcast.DescriptionSmall);
            sqlCommand.Parameters.AddWithValue("DESCLARGE", newBroadcast.DescriptionLarge);
            sqlCommand.Parameters.AddWithValue("THUMB", newBroadcast.ThumbnailURL);
            sqlCommand.Parameters.AddWithValue("WIDTH", newBroadcast.Width);
            sqlCommand.Parameters.AddWithValue("HEIGHT", newBroadcast.Height);
            sqlCommand.Parameters.AddWithValue("ISML", newBroadcast.ISM_URL);
            sqlCommand.Parameters.AddWithValue("STARTS", newBroadcast.StartTime);
            sqlCommand.Parameters.AddWithValue("ENDS", newBroadcast.EndTime);
            sqlCommand.Parameters.AddWithValue("SHOWSIDEBAR", newBroadcast.DisplaySidebar);
            sqlCommand.Parameters.AddWithValue("SHOWTHUMB", newBroadcast.DisplayThumbnail);
            sqlCommand.Parameters.AddWithValue("ISHIDDEN", newBroadcast.IsHidden);
            sqlCommand.Parameters.AddWithValue("ISPRIVATE", newBroadcast.IsPrivate);
            sqlCommand.Parameters.AddWithValue("FORCEONLINE", newBroadcast.ForcedLive);
            sqlCommand.Parameters.AddWithValue("SIDEBAR", newBroadcast.SidebarContent);
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

        public bool IsLive()
        {
            if ((DateTime.Now > this.StartTime) && (DateTime.Now < this.EndTime))
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

        public bool IsLiveWithinThisManyMinutes(int minutes)
        {
            if ((DateTime.Now.AddMinutes(minutes) > this.StartTime) && (DateTime.Now < this.EndTime))
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

        public bool IsComplete()
        {
            if (DateTime.Now > this.EndTime)
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
            TimeSpan streamDuration = this.EndTime.Subtract(this.StartTime);

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
