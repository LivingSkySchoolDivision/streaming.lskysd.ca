using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYStreamingVideo.CommonHTMLParts;

namespace LSKYStreamingVideo.player
{
    public partial class index : System.Web.UI.Page
    {
        private void displayError(string errorMessage)
        {
            tblContainer.Visible = false;
            tblErrorMessage.Visible = true;
            litErrorMessage.Text = "<h1>Error loading video</h1><br/><p>" + errorMessage + "</p>";
        }

        public static string videoInfoSection(Video video)
        {
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div class=\"video_list_name\">" + video.Name + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Duration:</b> " + video.DurationInEnglish + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Submitted by:</b> " + video.Author + "</div>");
            if (video.IsPrivate)
            {
                returnMe.Append("<div class=\"video_list_info\"><b>This video is flagged as private</b></div>");
            }
            if (!string.IsNullOrEmpty(video.DownloadURL))
            {
                returnMe.Append("<div class=\"video_list_info\"><a href=\"/video_files/" + video.DownloadURL + "\">Download available</a></div>");
            }

            returnMe.Append("<br/><div class=\"video_list_description\">" + video.Description + "</div>");
            return returnMe.ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["i"]))
            {
                // Sanitize the video ID
                string requestedID = Sanitizers.SanitizeQueryStringID(Request.QueryString["i"]);

                VideoRepository videoRepository = new VideoRepository();
                Video video = videoRepository.Get(requestedID);

                if (video != null)
                {
                    // Determine if the viewer is viewing from inside the network
                    string clientIP = Request.ServerVariables["REMOTE_ADDR"];
                    bool canUserAccessPrivateContent = Config.CanAccessPrivate(clientIP);

                    if (
                        (video.IsPrivate && canUserAccessPrivateContent) || 
                        (!video.IsPrivate)
                        )
                    {
                        // Set the page title
                        string originalTitle = Page.Header.Title;
                        Page.Header.Title = video.Name + " - " + originalTitle;

                        // Determine which player to display the video in
                        if (video.IsYoutubeAvailable)
                        {
                            litPlayer.Text = YoutubeVideoPlayer.GetHTML(video);
                        }
                        else
                        {
                            litPlayer.Text = HTML5VideoPlayer.GetHTML(video);
                        }
                        tblContainer.Visible = true;

                        litVideoInfo.Text = videoInfoSection(video);
                    }
                    else
                    {
                        displayError("This video is marked as private. you can only watch from within the LSKYSD network.");
                    }
                }
                else
                {
                    displayError("A video with that ID was not found.");
                }
            }
        }
    }
}