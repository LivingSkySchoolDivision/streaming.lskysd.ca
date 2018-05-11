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
        public string Description { get; set; }
        public string ThumbnailURL { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool ForcedLive { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }
        public string YouTubeID { get; set; }
        public bool EmbedInsteadOfLink { get; set; }

        public bool IsDelayed { get; set; }
        public bool IsCancelled { get; set; }

        public TimeSpan TimeUntilLive => this.StartTime.Subtract(DateTime.Now);

        public string TimeUntilStartsInEnglish
        {
            get
            {
                double totalMinutes = this.TimeUntilLive.TotalMinutes;
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
                    double totalHours = this.TimeUntilLive.TotalHours;
                    if (totalHours == 1)
                    {
                        return "1 hour";
                    }
                    else
                    {
                        if ((totalHours%1) == 0)
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
        }
        
        public LiveBroadcast() { }

        public bool IsLive
        {
            get
            {
                return ((DateTime.Now > this.StartTime) && (DateTime.Now < this.EndTime)) || (this.ForcedLive);
            }
        }

        public bool IsEnded
        {
            get { return DateTime.Now >= this.EndTime; }
        }
        
        public string ExpectedDuration
        {
            get
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
}
