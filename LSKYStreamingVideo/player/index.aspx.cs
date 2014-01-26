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
        private string BuildHTML5PlayerHTML(Video video)
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<video class=\"html5_player\" width=\"" + video.Width + "\" height=\"" + video.Height + "\" controls poster=\"lsky_stream_poster.png\" >");
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

            return returnMe.ToString();
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
                // Display player
                litPlayer.Text = BuildHTML5PlayerHTML(selectedVideo);
                litVideoInfo.Text = BuildVideoInfoHTML(selectedVideo);
                tblContainer.Visible = true;
                tblNotFound.Visible = false;
            }
            else
            {
                tblContainer.Visible = false;
                tblNotFound.Visible = true;

            }

        }
    }
}