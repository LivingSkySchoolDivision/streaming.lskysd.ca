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


        private string LiveStreamListItem(Stream stream)
        {
            int thumb_width = 300;
            int thumb_height = 225;
            string player_url = "#";

            string thumbnailURL = "none.png";
            if (!string.IsNullOrEmpty(stream.ThumbnailURLLarge))
            {
                thumbnailURL = stream.ThumbnailURLLarge;
            }
            
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
            returnMe.Append("<div style=\"background-image: url('thumbnails/large/" + thumbnailURL + "'); background-size: " + thumb_width +"px " + thumb_height + "px; background-repeat: no-repeat; width: " + thumb_width + "px; height: " + thumb_height + "px; border: 0;\">");
            returnMe.Append("</div>");
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
                        returnMe.Append(LiveStreamListItem(LiveStreams[numDisplayed]));
                        returnMe.Append("</td>");
                        numDisplayed++;
                    }
                }
                returnMe.Append("</tr>");
            }
            returnMe.Append("</table><br/><br/>");
                        
            return returnMe.ToString();
        }
              
        private string BuildUpcomingStreamDisplay(List<Stream> UpcomingStreams)
        {
            // Only display this many sections
            int maxDatesToDisplay = 5;
            
            StringBuilder returnMe = new StringBuilder();

            // Sort into dates
            Dictionary<string, List<Stream>> StreamsByDate = new Dictionary<string, List<Stream>>();
              
            foreach (Stream stream in UpcomingStreams)
            {
                if (!StreamsByDate.ContainsKey(stream.StreamStartTime.ToLongDateString()))
                {
                    StreamsByDate.Add(stream.StreamStartTime.ToLongDateString(), new List<Stream>());
                }
                StreamsByDate[stream.StreamStartTime.ToLongDateString()].Add(stream);
            }

            int numDatesDisplayed = 0;
            foreach (KeyValuePair<string, List<Stream>> dates in StreamsByDate)
            {
                numDatesDisplayed++;
                if (numDatesDisplayed > maxDatesToDisplay)
                {
                    break;
                }

                returnMe.Append("<div class=\"index_date_display\">" + dates.Key + "</div>");
                returnMe.Append("<table border=0 cellpadding=0 cellspacing=0 style=\"width: 100%;\">");
                
                foreach (Stream stream in dates.Value)
                {
                    returnMe.Append("<tr>");
                    returnMe.Append("<td width=\"25%\" valign=\"top\">");
                    returnMe.Append("<div class=\"upcoming_stream_time\"><b>" + stream.StreamStartTime.ToShortTimeString() + "</b>");
                    if (stream.GetTimeUntilStarts().TotalMinutes <= 120)
                    {
                        // If the stream is about to start, draw attention to it
                        returnMe.Append("<div class=\"upcoming_shortly\">(" + stream.GetTimeUntilStartsInEnglish() + ") </div>");
                    }
                    returnMe.Append("</td>");
                    returnMe.Append("<td valign=\"top\">");
                    returnMe.Append("<div class=\"upcoming_stream_name\" >" + stream.Name + "</div>");
                    
                    returnMe.Append("<div class=\"upcoming_stream_info\">Expected duration: " + stream.GetExpectedDuration() + "</div>");
                    if (!string.IsNullOrEmpty(stream.Location))
                    {
                        returnMe.Append("<div class=\"upcoming_stream_info\">Streaming from: " + stream.Location + "</div>");
                    }
                    returnMe.Append("</br></td>");
                    
                    returnMe.Append("</tr>");
                }

                returnMe.Append("</table>");
                returnMe.Append("<br/>");
               
            }
            

            return returnMe.ToString();
        }

        private string SmallVideoListItem(Video video)
        {
            StringBuilder returnMe = new StringBuilder();

            string thumbnailURL = "none.png";
            string playerURL = "player/?i=" + video.ID;

            if (!string.IsNullOrEmpty(video.ThumbnailURLSmall))
            {
                thumbnailURL = video.ThumbnailURLSmall;
            }

            returnMe.Append("<table border=0 cellpadding=0 cellspacing=0 style=\"width: 100%\">");
            returnMe.Append("<tr>");

            returnMe.Append("<td valign=\"top\" width=\"128\">");
            returnMe.Append("<a href=\""+playerURL+"\">");
            returnMe.Append("<div style=\"width: 135px; text-align: right; height: 128px; background-color: white; background-image: url(thumbnails/small/" + thumbnailURL + ");background-size: 128px 128px; background-repeat: no-repeat;\"></div></td>");
            returnMe.Append("</a>");

            returnMe.Append("<td valign=\"top\"><div class=\"video_list_info_container\">");
            returnMe.Append("<a style=\"text-decoration: none;\" href=\""+playerURL+"\"><div class=\"video_list_name\">" + video.Name + "</div></a>");
            returnMe.Append("<div class=\"video_list_info\">Duration: " + video.GetDurationInEnglish() + "</div>");
            returnMe.Append("<div class=\"video_list_info\">Submitted by: " + video.Author + "</div>");
            returnMe.Append("<div class=\"video_list_info\">Recorded at: " + video.Location + "</div>");
            
            if (video.ShouldDisplayAirDate)
            {
                returnMe.Append("<div class=\"video_list_info\">Original broadcast: " + video.DateAired.ToLongDateString() + "</div>");
            }

            if (!string.IsNullOrEmpty(video.DownloadURL))
            {
                returnMe.Append("<div class=\"video_list_info\">Download available</div>");
            }
            returnMe.Append("<br/><div class=\"video_list_description\">" + video.DescriptionSmall + "</div>");

            returnMe.Append("</div></td>");


            returnMe.Append("</tr>");
            returnMe.Append("</table><br/>");

            return returnMe.ToString();

        }

        private string BuildFeaturedVideoDisplay(List<Video> videos)
        {
            int maxFeaturedVideos = 4;

            // Generate a list of featured videos to display
            // We want these to be randomized, in case the pool of featured videos is larger than the number that we can display here

            List<Video> VideosToDisplay = new List<Video>();

            // Shuffle the list
            var rnd = new Random(DateTime.Now.Millisecond);
            List<Video> ShuffledVideos = videos.OrderBy(x => rnd.Next()).ToList();


            // Get the top X videos from the list
            int numAdded = 0;
            foreach (Video video in ShuffledVideos)
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
                returnMe.Append(SmallVideoListItem(video));
            }

            return returnMe.ToString();

        }

        private string BuildNewVideosDisplay(List<Video> videos)
        {
            int numColumns = 2;

            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<table border=0 cellpadding=0 cellspacing=0 style=\"width: 100%\">");
            returnMe.Append("<tr>");

            int numDisplayed = 0;
            foreach (Video video in videos)
            {
                numDisplayed++;
                returnMe.Append("<td>");
                returnMe.Append(SmallVideoListItem(video));
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


    }
}