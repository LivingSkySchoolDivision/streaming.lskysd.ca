using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingVideo
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Video> NewestVideos = new List<Video>();
            List<Video> FeaturedVideos = new List<Video>();
            List<LiveBroadcast> UpcomingStreams = new List<LiveBroadcast>();
            List<LiveBroadcast> CurrentlyLiveStreams = new List<LiveBroadcast>();
            
            using (SqlConnection connection = new SqlConnection(LSKYCommon.dbConnectionString_ReadOnly))
            {
                NewestVideos = Video.LoadNewest(connection, Config.CanAccessPrivate(Request.ServerVariables["REMOTE_ADDR"]));
                FeaturedVideos = Video.LoadFeatured(connection, Config.CanAccessPrivate(Request.ServerVariables["REMOTE_ADDR"]));
                UpcomingStreams = LiveBroadcast.LoadUpcoming(connection);
                CurrentlyLiveStreams = LiveBroadcast.LoadCurrentlyBroadcasting(connection, 20);

            }

            // Display live streams currently broadcasting
            if (CurrentlyLiveStreams.Count > 0)
            {
                if (CurrentlyLiveStreams.Count == 1)
                {

                    litPlayer.Visible = true;
                    litStreamInfo.Visible = true;
                    
                    litPlayer.Text = LSKYCommonHTMLParts.BuildLiveStreamPlayerHTML(CurrentlyLiveStreams.First(), LSKYCommonHTMLParts.Player.YouTube);
                    litStreamInfo.Text = LSKYCommonHTMLParts.BuildLiveStreamInfoHTML(CurrentlyLiveStreams.First());

                }
                else
                {
                    title_live.Visible = true; ;
                    litLiveStreams.Visible = true;
                    litLiveStreams.Text = BuildLiveStreamDisplay(CurrentlyLiveStreams);
                }
            }
            else
            {
                litPlayer.Visible = false;
                litStreamInfo.Visible = false;
                title_live.Visible = false;
                litLiveStreams.Visible = false;
            }


            // Display upcoming streams
            if (UpcomingStreams.Count > 0)
            {
                title_upcoming.Visible = true;
                litUpcomingStreams.Visible = true;
                litUpcomingStreams.Text = BuildUpcomingStreamDisplay(UpcomingStreams);
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
                litNewestVideos.Text = BuildNewVideosDisplay(NewestVideos);
                
            }
            else
            {
                title_newest.Visible = false;
                litNewestVideos.Visible = false;
            }
        }
        
        private string LiveStreamListItem(LiveBroadcast stream)
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
            if (stream.IsLive())
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
            if (stream.IsLive())
            {
                returnMe.Append("<div class=\"stream_title\"><a style=\"text-decoration: none;\" href=\"" + player_url + "\">" + stream.Name + "</a> <div class=\"live_indicator\">LIVE</div></div>");
            }
            else
            {
                returnMe.Append("<div class=\"stream_title\">" + stream.Name + "</div><div class=\"upcoming_indicator\">Starts in " + stream.GetTimeUntilStartsInEnglish() + "</div>");
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

            if (stream.IsLive())
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

        private string BuildLiveStreamDisplay(List<LiveBroadcast> LiveStreams)
        {
            StringBuilder returnMe = new StringBuilder();

            // Put in columns
            int numDisplayed = 0;
            int numColumns = 3;
            double numRows = Math.Ceiling((double)((double)LiveStreams.Count / (double)numColumns));
            
            //returnMe.Append("<table border=0 cellpadding=5 cellspacing=0 width=\"900\" style=\"margin-left: auto; margin-right: auto;\">");
            returnMe.Append("<div class=\"test2\">"); //@todo 
            for (int rowCount = 0; rowCount < numRows; rowCount++)
            {
                returnMe.Append("<div class=\"test3\">");
                for (int colCount = 0; colCount < numColumns; colCount++)
                {
                    if (numDisplayed < LiveStreams.Count)
                    {
                        //returnMe.Append("<td valign=\"top\">");
                        returnMe.Append(LiveStreamListItem(LiveStreams[numDisplayed]));
                        //returnMe.Append("</td>");
                        numDisplayed++;
                    }
                }
                //returnMe.Append("</tr>");
                returnMe.Append("</div>");  // div.test3
            }
            returnMe.Append("</div>");  // div.test2
                        
            return returnMe.ToString();
        }
              
        private string BuildUpcomingStreamDisplay(List<LiveBroadcast> UpcomingStreams)
        {
            // Only display this many sections
            int maxDatesToDisplay = 5;
            
            StringBuilder returnMe = new StringBuilder();

            // Sort into dates
            Dictionary<string, List<LiveBroadcast>> StreamsByDate = new Dictionary<string, List<LiveBroadcast>>();
              
            foreach (LiveBroadcast stream in UpcomingStreams)
            {
                if (!StreamsByDate.ContainsKey(stream.StartTime.ToLongDateString()))
                {
                    StreamsByDate.Add(stream.StartTime.ToLongDateString(), new List<LiveBroadcast>());
                }
                StreamsByDate[stream.StartTime.ToLongDateString()].Add(stream);
            }

            int numDatesDisplayed = 0;
            foreach (KeyValuePair<string, List<LiveBroadcast>> dates in StreamsByDate)
            {
                numDatesDisplayed++;
                if (numDatesDisplayed > maxDatesToDisplay)
                {
                    break;
                }
                
                returnMe.Append("<div class=\"index_date_display\">" + dates.Key + "</div>");
                //returnMe.Append("<table border=0 cellpadding=0 cellspacing=0 style=\"width: 100%;\">");
                returnMe.Append("<div class=\"upcomingStream\">");

                returnMe.Append("<ul class=\"upcomingStreamUL\">");
                foreach (LiveBroadcast stream in dates.Value)
                {
                    returnMe.Append("<li>");
                    //returnMe.Append("<td width=\"25%\" valign=\"top\">");
                    returnMe.Append("<ul width=\"25%\" valign=\"top\">");
                        //returnMe.Append("<div class=\"upcoming_stream_time\"><b>" + stream.StartTime.ToShortTimeString() + "</b>");
                        returnMe.Append("<li class=\"upcoming_stream_time\"><b>" + stream.StartTime.ToShortTimeString() + "</b></li>");
                        if (stream.GetTimeUntilStarts().TotalMinutes <= 120)
                        {
                            // If the stream is about to start, draw attention to it
                            returnMe.Append("<div class=\"upcoming_shortly\">(" + stream.GetTimeUntilStartsInEnglish() + ") </div>");
                        }
                    returnMe.Append("</ul>");
                    //returnMe.Append("<td valign=\"top\">");
                    returnMe.Append("<ul valign=\"top\">");

                        returnMe.Append("<div class=\"upcoming_stream_name\" >" + stream.Name + "</div>");
                        
                        returnMe.Append("<div class=\"upcoming_stream_info\">Expected duration: " + stream.GetExpectedDuration() + "</div>");
                        if (!string.IsNullOrEmpty(stream.Location))
                        {
                            returnMe.Append("<div class=\"upcoming_stream_info\">Streaming from: " + stream.Location + "</div>");
                        }
                    returnMe.Append("</br></li>");
                    
                    returnMe.Append("</ul>");
                }

                returnMe.Append("</ul>");
                returnMe.Append("<br/>");
               
            }
            

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
                returnMe.Append(LSKYCommonHTMLParts.SmallVideoListItem(video, true));
            }

            return returnMe.ToString();

        }
        
        /*
         * Mike 12/23/2015 - 10:30 - previous code - changing structure out of tables
        private string BuildNewVideosDisplay(List<Video> videos)
        {
            int numColumns = 2;

            int columnWidthPercent = (int)Math.Round((double)100 / (double)numColumns,0);

            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<table border=0 cellpadding=5 cellspacing=5 style=\"width: 100%\">");
            returnMe.Append("<tr>");

            int numDisplayed = 0;
            foreach (Video video in videos)
            {
                numDisplayed++;
                returnMe.Append("<td valign=\"top\" width=\"" + columnWidthPercent + "%\">");
                returnMe.Append(LSKYCommonHTMLParts.SmallVideoListItem(video, true));
                returnMe.Append("</td>");
                if (numDisplayed >= numColumns)
                {
                    returnMe.Append("</tr><tr>");
                    numDisplayed = 0;
                }
            }

            returnMe.Append("</tr>");
            returnMe.Append("</table>");

            return returnMe.ToString();

        }
        */
       
        /*
         * Newest Videos - Main structure - 2 column table. 
         * 
         */
        private string BuildNewVideosDisplay(List<Video> videos)
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
                returnMe.Append(LSKYCommonHTMLParts.SmallVideoListItem(video, true));
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