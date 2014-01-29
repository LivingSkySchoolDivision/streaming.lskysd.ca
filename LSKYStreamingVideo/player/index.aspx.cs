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
        private string BuildPlayerHTML(Video video, LSKYStreamingCore.LSKYCommonHTMLParts.Player player)
        {
            if (player == LSKYStreamingCore.LSKYCommonHTMLParts.Player.HTML5)
            {
                return LSKYCommonHTMLParts.BuildHTML5VideoPlayerHTML(video);
            }

            if (player == LSKYStreamingCore.LSKYCommonHTMLParts.Player.Silverlight)
            {
                return LSKYCommonHTMLParts.BuildSilverlightVideoPlayer(video);
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
                returnMe.Append("<div class=\"video_list_info\"><a href=\"/video_files/" + video.DownloadURL + "\">Download available</a></div>");
            }

            returnMe.Append("<br/><div class=\"video_list_description\">" + video.DescriptionLarge + "</div>");

            returnMe.Append("<br/><br/><div class=\"video_list_info\"><b>Browser compatibility chart for this video:</b> <div style=\"margin-left: 10px;\">" + LSKYCommon.GenerateBrowserCompatibilityChart(video) + "</div></div>");
            return returnMe.ToString();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Video selectedVideo = null;
            LSKYStreamingCore.LSKYCommonHTMLParts.Player selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.HTML5;
            
            // Check to see if we should use the silverlight player
            if (!string.IsNullOrEmpty(Request.QueryString["silverlight"]))
            {
                selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.Silverlight;
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
                        selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.Silverlight;
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
                    litPlayer.Text = LSKYCommonHTMLParts.BuildErrorMessage("Streaming video does not work over secure connections - please use a non-encrypted connection");
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