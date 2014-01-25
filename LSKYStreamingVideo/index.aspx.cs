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
            List<Stream> UpcomingStreams = new List<Stream>();
            List<Stream> CurrentlyLiveStreams = new List<Stream>();
            
            using (SqlConnection connection = new SqlConnection(LSKYCommon.dbConnectionString_ReadOnly))
            {
                NewestVideos = Video.LoadNewestVideos(connection);
                FeaturedVideos = Video.LoadFeaturedVideos(connection);
                UpcomingStreams = Stream.LoadUpcomingStreams(connection);
                CurrentlyLiveStreams = Stream.LoadCurrentlyBroadcasting(connection);

            }

            // Display live streams currently broadcasting
            if (CurrentlyLiveStreams.Count > 0)
            {
                //title_live.Visible = true; ;
                litLiveStreams.Visible = true;
                litLiveStreams.Text = BuildLiveStreamDisplay(CurrentlyLiveStreams);
            }
            else
            {
                title_live.Visible = false;
                litLiveStreams.Visible = false;
            }


            // Display upcoming streams
            if (UpcomingStreams.Count > 0)
            {
                title_upcoming.Visible = true;
                litUpcomingStreams.Visible = true;
                //litUpcomingStreams.Text = 
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
            }
            else
            {
                title_featured.Visible = false;
            }


            // Display newest videos (if any exist)
            if (NewestVideos.Count > 0)
            {
                title_newest.Visible = true;
            }
            else
            {
                title_newest.Visible = false;
            }
        }


        private string LiveStreamBox(Stream stream)
        {
            int thumb_width = 300;
            int thumb_height = 225;
            string player_url = "#";

            TimeSpan time_since_start = DateTime.Now.Subtract(stream.StreamStartTime);
            TimeSpan expected_duration = stream.StreamEndTime.Subtract(stream.StreamStartTime);
            TimeSpan time_until_finish = stream.StreamEndTime.Subtract(DateTime.Now);
            
            int minutes_since_start = (int)Math.Round(time_since_start.TotalMinutes);
            int hours_since_start = (int)Math.Round(time_since_start.TotalHours);

            int minutes_until_finish = (int)Math.Round(time_until_finish.TotalMinutes);
            int hours_until_finish = (int)Math.Round(time_until_finish.TotalHours);

            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<table border=0 cellpadding=5 cellspacing=0 style=\"margin-left: auto; margin-right: auto;\">");

            returnMe.Append("<tr>");

            returnMe.Append("<td align=\"left\" valign=\"top\" width=\"" + thumb_width + "\">");
            returnMe.Append("<a style=\"text-decoration: none;\" href=\"" + player_url + "\">");            
            returnMe.Append("<div style=\"background-color: white; width: " + thumb_width + "px; height: " + thumb_height +"px; border: 1px solid black;\">&nbsp;</div>");
            returnMe.Append("</a>");
            returnMe.Append("</td>");

            returnMe.Append("</tr>");
            returnMe.Append("<tr>");


            returnMe.Append("<td align=\"left\" valign=\"top\">");
            returnMe.Append("<div class=\"stream_title\"><a style=\"text-decoration: none;\" href=\"" + player_url + "\">" + stream.Name + "</a> <div class=\"live_indicator\">LIVE</div></div>");
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
            returnMe.Append("</div>");
            
            returnMe.Append("<br/><div class=\"stream_description\">" + stream.DescriptionSmall + "</div>");
            returnMe.Append("</td>");



            returnMe.Append("</tr>");
            returnMe.Append("</table>");

            return returnMe.ToString();
        }

        private string BuildLiveStreamDisplay(List<Stream> LiveStreams)
        {
            StringBuilder returnMe = new StringBuilder();

            // Put in columns
            int numDisplayed = 0;
            int numColumns = 3;
            double numRows = Math.Ceiling((double)((double)LiveStreams.Count / (double)numColumns));
            
            returnMe.Append("<table border=0 cellpadding=5 cellspacing=0 width=\"900\" style=\"margin-left: auto; margin-right: auto;\">");
            for (int rowCount = 0; rowCount < numRows; rowCount++)
            {
                returnMe.Append("<tr>");
                for (int colCount = 0; colCount < numColumns; colCount++)
                {
                    if (numDisplayed < LiveStreams.Count)
                    {
                        returnMe.Append("<td valign=\"top\">");
                        returnMe.Append(LiveStreamBox(LiveStreams[numDisplayed]));
                        returnMe.Append("</td>");
                        numDisplayed++;
                    }
                }
                returnMe.Append("</tr>");
            }
            returnMe.Append("</table>");
                        
            return returnMe.ToString();
        }
    }
}