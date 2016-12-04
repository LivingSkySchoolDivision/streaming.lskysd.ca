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
        public string FileURL_H264 { get; set; }
        public string FileURL_THEORA { get; set; }
        public string FileURL_VP8 { get; set; }
        public string DownloadURL { get; set; }
        public string YoutubeURL { get; set; }
        public bool IsAlwaysAvailable { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }
        public List<string> Tags { get; set; }
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
        
        public bool IsYoutubeAvailable
        {
            get
            {
                return !string.IsNullOrEmpty(this.YoutubeURL);
            }
        }
        public bool IsHTML5Available
        {
            get
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
        }
        
        public string DurationInEnglish
        {
            get
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

        public string CategoryName
        {
            get
            {
                if (this.Category != null)
                {
                    return this.Category.FullName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }       

    }
}
