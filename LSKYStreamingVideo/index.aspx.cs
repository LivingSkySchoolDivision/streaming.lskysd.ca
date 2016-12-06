﻿using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYStreamingCore.ExtensionMethods;
using LSKYStreamingCore.Repositories;
using LSKYStreamingVideo.CommonHTMLParts;

namespace LSKYStreamingVideo
{
    public partial class index : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            VideoRepository videoRepository = new VideoRepository();
            LiveBroadcastRepository liveBroadcastRepository = new LiveBroadcastRepository();

            // Determine if the user can access private videos
            bool includePrivateContent = false;


            List<Video> NewestVideos = videoRepository.GetNewest(includePrivateContent, 60);
            List<Video> FeaturedVideos = videoRepository.GetFeatured(includePrivateContent);
            List<LiveBroadcast> UpcomingStreams = liveBroadcastRepository.GetUpcoming();
            List<LiveBroadcast> CurrentlyLiveStreams = liveBroadcastRepository.GetLive();
            
            // Display live streams currently broadcasting
            if (CurrentlyLiveStreams.Count > 0)
            {
                if (CurrentlyLiveStreams.Count == 1)
                {

                    litPlayer.Visible = true;
                    litStreamInfo.Visible = true;
                    
                    litPlayer.Text = YoutubeLiveBroadcastPlayer.GetHTML(CurrentlyLiveStreams.First());
                    litStreamInfo.Text = buildLiveStreamInfoHTML(CurrentlyLiveStreams.First());

                }
                else
                {
                    litLiveStreams.Visible = true;
                    litLiveStreams.Text = showMultipleLiveStreams(CurrentlyLiveStreams);
                }
            }
            else
            {
                litPlayer.Visible = false;
                litStreamInfo.Visible = false;
                litLiveStreams.Visible = false;
            }


            // Display upcoming streams
            if (UpcomingStreams.Count > 0)
            {
                title_upcoming.Visible = true;
                litUpcomingStreams.Visible = true;
                litUpcomingStreams.Text = upcomingStreamsSection(UpcomingStreams);
            }
            else
            {
                title_upcoming.Visible = false;
                litUpcomingStreams.Visible = false;
            }
            

            // If there are featured videos, display them on the front page
            if (FeaturedVideos.Count > 0)
            {
                title_featured.Visible = true;
                litFeaturedVideos.Visible = true;
                litFeaturedVideos.Text = BuildFeaturedVideoDisplay(FeaturedVideos);
            }
            else
            {
                title_featured.Visible = false;
                litFeaturedVideos.Visible = false;
            }


            // Display newest videos (if any exist)
            if (NewestVideos.Count > 0)
            {
                title_newest.Visible = true;
                litNewestVideos.Visible = true;
                litNewestVideos.Text = newestVideosSection(NewestVideos);
                
            }
            else
            {
                title_newest.Visible = false;
                litNewestVideos.Visible = false;
            }
        }

        private static string buildLiveStreamInfoHTML(LiveBroadcast stream)
        {
            int container_width = stream.Width;
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div style=\"max-width: " + container_width + "px; margin-left: auto; margin-right: auto;\">");
            returnMe.Append("<div class=\"video_list_name\">" + stream.Name + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Broadcasting from:</b> " + stream.Location + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Scheduled start:</b> " + stream.StartTime.ToLongDateString() + " " + stream.StartTime.ToLongTimeString() + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Scheduled end:</b> " + stream.EndTime.ToLongDateString() + " " + stream.EndTime.ToLongTimeString() + "</div>");
            returnMe.Append("<br/><div class=\"video_list_description\">" + stream.Description + "</div>");
            returnMe.Append("</div><br/><br/><br/>");
            return returnMe.ToString();
        }


        private string liveStreamListItem(LiveBroadcast stream)
        {
            int thumb_width = 360;
            int thumb_height = 240;
            string player_url = "live/?i=" + stream.ID;

            string thumbnailURL = "none.png";
            if (!string.IsNullOrEmpty(stream.ThumbnailURL))
            {
                thumbnailURL = stream.ThumbnailURL;
            }
            
            TimeSpan time_since_start = DateTime.Now.Subtract(stream.StartTime);
            TimeSpan expected_duration = stream.EndTime.Subtract(stream.StartTime);
            TimeSpan time_until_finish = stream.EndTime.Subtract(DateTime.Now);
            
            int minutes_since_start = (int)Math.Round(time_since_start.TotalMinutes);
            int hours_since_start = (int)Math.Round(time_since_start.TotalHours);

            int minutes_until_finish = (int)Math.Round(time_until_finish.TotalMinutes);
            int hours_until_finish = (int)Math.Round(time_until_finish.TotalHours);

            StringBuilder returnMe = new StringBuilder();

 
            //returnMe.Append("<table border=0 cellpadding=5 cellspacing=0 style=\"margin-left: auto; margin-right: auto;\">");
            //returnMe.Append("<tr>");
            //returnMe.Append("<td align=\"left\" valign=\"top\" width=\"" + thumb_width + "\">");
         
            returnMe.Append("<div class=\"liveStreamWrap\">");
            returnMe.Append("<div class=\"liveStream\">");
            if (stream.IsLive)
            {
                returnMe.Append("<a style=\"text-decoration: none;\" href=\"" + player_url + "\">");
            }
            returnMe.Append("<img src=\"/thumbnails/broadcasts/" + thumbnailURL + "\" width=\"" + thumb_width + "\" height=\"" + thumb_height + "\">");
            returnMe.Append("</div>"); //div.liveStream
            
            returnMe.Append("</a>");
            
            //returnMe.Append("</td>");

            //returnMe.Append("</tr>");
            //returnMe.Append("<tr>");


            returnMe.Append("<div class=\"featuredDesc\">");
            if (stream.IsLive)
            {
                returnMe.Append("<div class=\"stream_title\"><a style=\"text-decoration: none;\" href=\"" + player_url + "\">" + stream.Name + "</a> <div class=\"live_indicator\">LIVE</div></div>");
            }
            else
            {
                returnMe.Append("<div class=\"stream_title\">" + stream.Name + "</div><div class=\"upcoming_indicator\">Starts in " + stream.TimeUntilLive + "</div>");
            }
            if (!string.IsNullOrEmpty(stream.Location))
            {
                returnMe.Append("<div class=\"stream_info\">Location: " + stream.Location + "</div>");
            }

            if ((minutes_since_start > 0))
            {
                if (minutes_since_start > 120)
                {
                    returnMe.Append("<div class=\"stream_info\">Started: " + hours_since_start + " hours ago</div>");
                }
                else
                {
                    returnMe.Append("<div class=\"stream_info\">Started: " + minutes_since_start + " minutes ago</div>");
                }
            
            }
            else
            {
                returnMe.Append("<div class=\"stream_info\">Starts in " + (minutes_since_start * -1) + " minutes</div>");
                //returnMe.Append("<div class=\"stream_info\">Expected start: " + stream.StreamStartTime.ToShortDateString() + " " + stream.StreamStartTime.ToShortTimeString() + "</div>");
            }
            
            returnMe.Append("<div class=\"stream_info\">Expected duration: " + Math.Round(expected_duration.TotalMinutes).ToString() + " minutes");

            if (stream.IsLive)
            {
                if (minutes_until_finish > 0)
                {
                    if (minutes_until_finish > 120)
                    {
                        returnMe.Append(" (" + hours_until_finish + " hours to go)");
                    }
                    else
                    {
                        returnMe.Append(" (" + minutes_until_finish + " minutes to go)");
                    }
                }
            }
            returnMe.Append("</div>");
            
            //returnMe.Append("<br/><div class=\"stream_description\">" + stream.DescriptionSmall + "</div>");
            
            //returnMe.Append("</td>");

            returnMe.Append("</div>"); //div.liveStreamWrap

            //returnMe.Append("</tr>");
            //returnMe.Append("</table>");

            return returnMe.ToString();
        }

        public static string smallVideoListItem(Video video)
        {
            StringBuilder returnMe = new StringBuilder();

            string thumbnailURL = "none.png";
            string playerURL = "/player/?i=" + video.ID;

            if (!string.IsNullOrEmpty(video.ThumbnailURL))
            {
                thumbnailURL = video.ThumbnailURL;
            }

            returnMe.Append("<div class=\"SmallVideoListItem\">");
            
            returnMe.Append("<div class=\"VideoListThumb\" width=\"128\">");
            returnMe.Append("<a href=\"" + playerURL + "\">");
            returnMe.Append("<img border=\"0\" src=\"/thumbnails/videos/" + thumbnailURL + "\" class=\"video_thumbnail_list_item_container_image\">");
            returnMe.Append("</a>");
            returnMe.Append("</div>");
            
            returnMe.Append("<div class=\"video_list_info_container\">");
            returnMe.Append("<ul>");
            returnMe.Append("<li class=\"VideoListDescTitle\"> <a style=\"text-decoration: none;\" href=\"" + playerURL + "\"><div class=\"video_list_name\">" + video.Name + "</div></a> </li>");
            returnMe.Append("<li class=\"VideoListDescDuration\"> <div class=\"video_list_info\"><b>Duration:</b> " + video.DurationInEnglish + "</div></li>");
            returnMe.Append("<li class=\"VideoListDescSubmitted\"> <div class=\"video_list_info\"><b>Submitted by:</b> " + video.Author + "</div></li>");
            returnMe.Append("<li class=\"VideoListDescRecorded\"> <div class=\"video_list_info\"><b>Recorded at:</b> " + video.Location + "</div></li>");
            returnMe.Append("</ul>");

            if (video.IsPrivate)
            {
                returnMe.Append("<div class=\"video_list_info\"><b>This video is flagged as private</b></div>");
            }

            if (!string.IsNullOrEmpty(video.DownloadURL))
            {
                returnMe.Append("<div class=\"video_list_info\">Download available</div>");
            }
            returnMe.Append("<br/><div class=\"video_list_description\">" + video.Description + "</div>");

            returnMe.Append("</div></td>");


            returnMe.Append("</ul>");
            returnMe.Append("</div>");

            return returnMe.ToString();

        }

        private string showMultipleLiveStreams(List<LiveBroadcast> LiveStreams)
        {
            StringBuilder returnMe = new StringBuilder();

            // Put in columns
            int numDisplayed = 0;
            int numColumns = 2;
            double numRows = Math.Ceiling((double)((double)LiveStreams.Count / (double)numColumns));
            
            returnMe.Append("<table border=0 cellpadding=5 cellspacing=0 width=\"900\" style=\"margin-left: auto; margin-right: auto;\">");
            for (int rowCount = 0; rowCount < numRows; rowCount++)
            {
                returnMe.Append("<div class=\"test3\">");
                for (int colCount = 0; colCount < numColumns; colCount++)
                {
                    if (numDisplayed < LiveStreams.Count)
                    {
                        // On the last row, if the number of videos is an odd number, make it colspan = 2
                        if (LiveStreams.Count.IsOdd() && (numDisplayed + 1 == LiveStreams.Count))
                        {
                            returnMe.Append("<td valign=\"top\" align=\"Center\" colspan=2>");
                        }
                        else
                        {
                            returnMe.Append("<td valign=\"top\" align=\"Center\">");
                        }
                        returnMe.Append(liveStreamListItem(LiveStreams[numDisplayed]));
                        returnMe.Append("<br/><br/></td>");
                        numDisplayed++;


                    }
                }
                returnMe.Append("</tr>");
                returnMe.Append("</div>");
            }
            returnMe.Append("</div>");
            returnMe.Append("</table><br/><br/>");
            return returnMe.ToString();
        }
              
        private string upcomingStreamsSection(List<LiveBroadcast> UpcomingStreams)
        {
            int maxUpcomingStreams = 10;

            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<div class=\"upcomingBroadcasts\">&nbsp;");

            foreach (LiveBroadcast upcomingStream in UpcomingStreams.OrderBy(s => s.StartTime).ThenBy(s => s.Name))
            {
                returnMe.Append("<div class=\"upcomingBroadcast\">");
                returnMe.Append("<div class=\"upcomingBroadcastDate\">");
                returnMe.Append("<div class=\"upcomingBroadcastDate_Month\">" + upcomingStream.StartTime.MonthName() + "</div>");
                returnMe.Append("<div class=\"upcomingBroadcastDate_Day\">" + upcomingStream.StartTime.Day + "</div>");
                returnMe.Append("</div>");

                if (upcomingStream.TimeUntilLive.TotalMinutes <= 120)
                {
                    // If the stream is about to start, draw attention to it
                    returnMe.Append("<div class=\"upcomingBroadcastTime\">Starts in " + upcomingStream.TimeUntilLive + "</div>");
                }
                else
                {
                    returnMe.Append("<div class=\"upcomingBroadcastTime\">" + upcomingStream.StartTime.ToShortTimeString() + "</div>");
                }

                
                returnMe.Append("<div class=\"upcomingBroadcastDetails\">");
                returnMe.Append("<div class=\"upcomingBroadcastName\">" + upcomingStream.Name + "</div>");
                returnMe.Append("</div>");
                returnMe.Append("</div>");
            }
            returnMe.Append("</div><br/><br/>");

            return returnMe.ToString();
        }
        private string BuildFeaturedVideoDisplay(List<Video> videos)
        {
            int maxFeaturedVideos = 4;

            // Generate a list of featured videos to display
            List<Video> VideosToDisplay = new List<Video>();

            // Get the top X videos from the list
            int numAdded = 0;
            foreach (Video video in videos)
            {
                numAdded++;
                if (numAdded > maxFeaturedVideos)
                {
                    break;
                }
                VideosToDisplay.Add(video);
            }
                      
            
            StringBuilder returnMe = new StringBuilder();


            foreach (Video video in VideosToDisplay)
            {
                returnMe.Append(smallVideoListItem(video));
            }

            return returnMe.ToString();

        }
        
        private string newestVideosSection(List<Video> videos)
        {
            int numColumns = 2;

            int columnWidthPercent = (int)Math.Round((double)100 / (double)numColumns, 0);

            StringBuilder returnMe = new StringBuilder();

            //returnMe.Append("<tr>");
            returnMe.Append("<div class=\"section clearfix first\" >");

            int numDisplayed = 0;
            foreach (Video video in videos)
            {
                numDisplayed++;
                //returnMe.Append("<td valign=\"top\" width=\"" + columnWidthPercent + "%\">");
                returnMe.Append("<div class=\"col span_1_of_2\">");
                returnMe.Append(smallVideoListItem(video));
                //returnMe.Append("</td>");
                returnMe.Append("</div>");

                //if (numDisplayed >= numColumns).
                if (numDisplayed >= numColumns)
                {
                    //returnMe.Append("</tr><tr>");
                    //(next row)
                    returnMe.Append("</div><div class=\"section clearfix\">");
                    numDisplayed = 0;
                }
            }

            //returnMe.Append("</tr>");
            returnMe.Append("</div>");

            //returnMe.Append("</table>");

            return returnMe.ToString();

        }
    }
}