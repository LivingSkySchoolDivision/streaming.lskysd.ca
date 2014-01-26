using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingVideo.player
{
    public partial class index : System.Web.UI.Page
    {
        enum Player
        {
            Silverlight,
            HTML5
        }

        private string BuildErrorMessage(string message)
        {
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div class=\"large_infobox\" style=\"\">" + message + "</div> ");
            return returnMe.ToString();

        }

        private string BuildSilverlightPlayer(Video video)
        {
            if (!string.IsNullOrEmpty(video.FileURL_ISM))
            {
                int width = video.Width;
                int height = video.Height;

                string playerXapFile = "LSKYSmoothStreamPlayer_PreRecorded.xap";

                StringBuilder returnMe = new StringBuilder();
                returnMe.Append("<div style=\"border: 0px solid black; width: " + width + "px; height: " + height + "px;\">");
                returnMe.Append("<object data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" width=\"" + width + "\" height=\"" + height + "\">");
                returnMe.Append("<param name=\"source\" value=\"" + playerXapFile + "\"/>");
                returnMe.Append("<param name=\"onError\" value=\"onSilverlightError\" />");
                returnMe.Append("<param name=\"background\" value=\"white\" />");
                returnMe.Append("<param name=\"minRuntimeVersion\" value=\"4.0.50826.0\" />");
                returnMe.Append("<param name=\"autoUpgrade\" value=\"true\" />");
                returnMe.Append("<param name=\"initParams\" value=\"streamuri=/video_files/" + video.FileURL_ISM + "/Manifest,width=" + width + ",height=" + height + "\" />");
                returnMe.Append("<a href=\"http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0\" style=\"text-decoration:none\">");
                returnMe.Append("<img src=\"http://go.microsoft.com/fwlink/?LinkId=161376\" alt=\"Get Microsoft Silverlight\" style=\"border-style:none\"/>");
                returnMe.Append("</a>");
                returnMe.Append("</object>");
                returnMe.Append("</div>");
                returnMe.Append("<div style=\"width: " + video.Width + "px; margin-left: auto; margin-right: auto; font-size: 8pt; color: #444444; text-align: right;\">");

                if (video.IsHTML5Available())
                {
                    returnMe.Append("Problems viewing the stream? <a href=\"?i=" + video.ID + "&html5=true\">click here to switch to HTML5 player</a>, or <a href=\"/help/\">click here for our help page</a> ");
                }
                else
                {
                    returnMe.Append("Problems viewing the stream? <a href=\"/help/\">Click here for our help page</a> ");                    
                }
                
                returnMe.Append("</div>");
                return returnMe.ToString();
            }
            else
            {
                return BuildErrorMessage("Video not supported by this player.");
            }
        }

        private string BuildHTML5PlayerHTML(Video video)
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<video autoplay class=\"html5_player\" width=\"" + video.Width + "\" height=\"" + video.Height + "\" controls poster=\"lsky_stream_poster.png\" >");
            if (!string.IsNullOrEmpty(video.FileURL_MP4))
            {
                returnMe.Append("<source src=\"/video_files/" + video.FileURL_MP4 + "\" type=\"video/mp4\" />");
            }
            if (!string.IsNullOrEmpty(video.FileURL_OGV))
            {
                returnMe.Append("<source src=\"/video_files/" + video.FileURL_OGV + "\" type=\"video/ogg\" />");
            }
            if (!string.IsNullOrEmpty(video.FileURL_WEBM))
            {
                returnMe.Append("<source src=\"/video_files/" + video.FileURL_WEBM + "\" type=\"video/webm\" />");
            }
            returnMe.Append("<em>Sorry, your browser doesn't support HTML5 video.</em>");
            returnMe.Append("</video>");

            returnMe.Append("<div style=\"width: " + video.Width + "px; margin-left: auto; margin-right: auto; font-size: 8pt; color: #444444; text-align: right;\">");
            if (video.IsSilverlightAvailable())
            {
                returnMe.Append("Problems viewing the stream? <a style=\"font-size:8pt;\" href=\"?i=" + video.ID + "&silverlight=true\">click here to switch to the Silverlight player</a>, or <a href=\"/help/\">click here for our help page</a> ");
            }
            else
            {
                returnMe.Append("Problems viewing the stream? <a href=\"/help/\">Click here for our help page</a> ");                
            }
            returnMe.Append("</div>");
            
            return returnMe.ToString();
        }

        private string BuildPlayerHTML(Video video, Player player)
        {
            if (player == Player.HTML5)
            {
                return BuildHTML5PlayerHTML(video);
            }

            if (player == Player.Silverlight)
            {
                return BuildSilverlightPlayer(video);
            }
            return string.Empty;
        }

        private string BuildVideoInfoHTML(Video video)
        {
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div class=\"video_list_name\">" + video.Name + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Duration:</b> " + video.GetDurationInEnglish() + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Submitted by:</b> " + video.Author + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Recorded at:</b> " + video.Location + "</div>");
            if (video.ShouldDisplayAirDate)
            {
                returnMe.Append("<div class=\"video_list_info\"><b>Original broadcast:</b> " + video.DateAired.ToLongDateString() + "</div>");
            }

            if (!string.IsNullOrEmpty(video.DownloadURL))
            {
                returnMe.Append("<div class=\"video_list_info\"><a href=\"" + video.DownloadURL + "\">Download available</a></div>");
            }

            returnMe.Append("<br/><div class=\"video_list_description\">" + video.DescriptionLarge + "</div>");            
            return returnMe.ToString();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Video selectedVideo = null;
            Player selectedPlayer = Player.HTML5;
            
            // Check to see if we should use the silverlight player
            if (!string.IsNullOrEmpty(Request.QueryString["silverlight"]))
            {
                selectedPlayer = Player.Silverlight;
            }

            // Check if we got a video ID in the querystring
            if (!string.IsNullOrEmpty(Request.QueryString["i"]))
            {
                // Sanitize the video ID
                string requestedID = LSKYCommon.SanitizeQueryStringID(Request.QueryString["i"]);

                // See if this video exists
                using (SqlConnection connection = new SqlConnection(LSKYCommon.dbConnectionString_ReadOnly))
                {
                    if (Video.DoesVideoIDExist(connection, requestedID))
                    {
                        selectedVideo = Video.LoadThisVideo(connection, requestedID);
                    }
                }
            }

            if (selectedVideo != null)
            {
                if (!Request.IsSecureConnection)
                {
                    // Check which player to use based on what this video supports

                    // File has no HTML5 capable videos associated with it - use silverlight
                    if (!selectedVideo.IsHTML5Available())
                    {
                        selectedPlayer = Player.Silverlight;
                    }

                    // Change the browser title                
                    string originalTitle = Page.Header.Title;
                    Page.Header.Title = selectedVideo.Name + " - " + originalTitle;

                    // Display player
                    litPlayer.Text = BuildPlayerHTML(selectedVideo, selectedPlayer);
                    litVideoInfo.Text = BuildVideoInfoHTML(selectedVideo);
                    tblContainer.Visible = true;
                    tblNotFound.Visible = false;
                }
                else
                {
                    litPlayer.Text = BuildErrorMessage("Streaming video does not work over secure connections - please use a non-encrypted connection");
                    tblContainer.Visible = true;
                    tblNotFound.Visible = false;
                }
            }
            else
            {
                tblContainer.Visible = false;
                tblNotFound.Visible = true;

            }

        }
    }
}