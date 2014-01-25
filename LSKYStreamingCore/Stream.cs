﻿using System;
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
        public bool DisplaySidebar { get; set; }
        public bool DisplayThumbnail { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }


        public Stream(string id, string name, string location, string descriptionSmall, string descriptionLarge, string thumbnailSmall, string thumbnailLarge, int width, 
            int height, string ismurl, DateTime starts, DateTime ends, bool displaySidebar, bool displayThumbnail, bool hidden, bool isprivate) 
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
                LSKYCommon.ParseDatabaseBool(dbDataReader["private"].ToString(), false)
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
            sqlCommand.CommandText = "SELECT * FROM live_streams WHERE stream_start > @CURRENTDATETIME AND override_times=0 ORDER BY stream_start ASC, name ASC;";
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
            sqlCommand.CommandText = "SELECT * FROM live_streams WHERE ((stream_start < @CURRENTDATETIME AND stream_end > @CURRENTDATETIME) OR (override_times=1)) ORDER BY stream_start ASC, name ASC;";
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


    }
}
