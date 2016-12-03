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
        public string YoutubeURL { get; set; }
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

        

    }
}
