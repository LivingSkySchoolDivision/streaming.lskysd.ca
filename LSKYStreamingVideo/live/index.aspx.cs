using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYStreamingCore.Repositories;
using LSKYStreamingVideo.CommonHTMLParts;

namespace LSKYStreamingVideo.live
{
    public partial class index : System.Web.UI.Page
    {
        private void displayError(string errorMessage)
        {
            tblContainer.Visible = false;
            tblErrorMessage.Visible = true;
            litErrorMessage.Text = "<h1>Error loading stream</h1><br/><p>" + errorMessage + "</p>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["i"]))
            {
                // Sanitize the video ID
                string requestedID = Sanitizers.SanitizeQueryStringID(Request.QueryString["i"]);

                // See if this video exists
                LiveBroadcastRepository liveBroadcastRepository = new LiveBroadcastRepository();
                LiveBroadcast liveStream = liveBroadcastRepository.Get(requestedID);

                if (liveStream != null)
                {
                    // Determine if the viewer is viewing from inside the network
                    string clientIP = Request.ServerVariables["REMOTE_ADDR"];
                    bool canUserAccessPrivateContent = Config.CanAccessPrivate(clientIP);

                    // Set the page title
                    string originalTitle = Page.Header.Title;
                    Page.Header.Title = liveStream.Name + " - " + originalTitle;

                    if (liveStream.IsEnded && !liveStream.ForcedLive)
                    {
                        displayError("This live stream has ended.");
                    }
                    else
                    {
                        if (
                            ((liveStream.IsPrivate) && (canUserAccessPrivateContent)) ||
                            (!liveStream.IsPrivate))
                        {
                            tblContainer.Visible = true;
                            litPlayer.Text = YoutubeLiveBroadcastPlayer.GetHTML(liveStream);
                            litStreamInfo.Text = streamInfoBox(liveStream);
                        }
                        else
                        {
                            displayError("This live stream is marked as private. You can only watch from within the LSKYSD network.");
                        }
                    }
                }
                else
                {
                    displayError("A live stream with that ID does not exist.");
                }

            }
            else
            {
                displayError("Stream ID not specified.");
            }
        }

        public static string streamInfoBox(LiveBroadcast stream)
        {
            int container_width = stream.Width;
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div style=\"max-width: " + container_width + "px; margin-left: auto; margin-right: auto;\">");
            returnMe.Append("<div class=\"video_list_name\">" + stream.Name + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Broadcasting from:</b> " + stream.Location + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Scheduled start:</b> " + stream.StartTime.ToLongDateString() + " " + stream.StartTime.ToLongTimeString() + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Scheduled end:</b> " + stream.EndTime.ToLongDateString() + " " + stream.EndTime.ToLongTimeString() + "</div>");
            returnMe.Append("<br/><div class=\"video_list_description\">" + stream.Description + "</div>");
            returnMe.Append("</div>");
            return returnMe.ToString();
        }
    }
}