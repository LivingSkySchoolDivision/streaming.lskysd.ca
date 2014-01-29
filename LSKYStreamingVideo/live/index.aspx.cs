using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingVideo.live
{
    public partial class index : System.Web.UI.Page
    {
        private string BuildNotStreamingMessage(Stream stream)
        {
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div class=\"large_infobox\" style=\"\">Stream not currently broadcasting</div> ");
            return returnMe.ToString();

        }
        
        private float getInternetExplorerVersion()
        {
            // Returns the version of Internet Explorer or a -1
            // (indicating the use of another browser).
            float rv = -1;
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            if ((browser.Browser == "IE") || (browser.Browser == "InternetExplorer"))
                rv = (float)(browser.MajorVersion + browser.MinorVersion);            
            return rv;
        }

        private bool isClientWindows()
        {
            System.Web.HttpBrowserCapabilities browser = Request.Browser;

            if ((browser.Platform.ToLower() == "winnt"))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private string BuildStreamInfoHTML(Stream stream)
        {
            int container_width = stream.Width;
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div style=\"max-width: " + container_width + "px; margin-left: auto; margin-right: auto;\">");
            returnMe.Append("<div class=\"video_list_name\">" + stream.Name + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Broadcasting from:</b> " + stream.Location + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Scheduled start:</b> " + stream.StreamStartTime.ToLongDateString() + " " + stream.StreamStartTime.ToLongTimeString() + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Scheduled end:</b> " + stream.StreamEndTime.ToLongDateString() + " " + stream.StreamEndTime.ToLongTimeString() + "</div>");
            returnMe.Append("<br/><div class=\"video_list_description\">" + stream.DescriptionLarge + "</div>");
            returnMe.Append("</div>");
            return returnMe.ToString();
        }
        
        private string BuildPlayerHTML(Stream stream, LSKYStreamingCore.LSKYCommonHTMLParts.Player player)
        {
            if (player == LSKYStreamingCore.LSKYCommonHTMLParts.Player.HTML5)
            {
                return LSKYCommonHTMLParts.BuildHTML5LiveStreamPlayer(stream);
            }

            if (player == LSKYStreamingCore.LSKYCommonHTMLParts.Player.Silverlight)
            {
                return LSKYCommonHTMLParts.BuildSilverlightLiveStreamPlayer(stream);
            }
            return string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Stream selectedStream = null;
            LSKYStreamingCore.LSKYCommonHTMLParts.Player selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.HTML5;

            // Check to see which player we should be using based on the client's browser
            //if ((getInternetExplorerVersion() < 10) && (getInternetExplorerVersion() != -1))
            if (isClientWindows())
            {
                selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.Silverlight;
            }
            else
            {
                selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.HTML5;
            }

            // Check to see if the user has specified a player to use
            if (!string.IsNullOrEmpty(Request.QueryString["html5"]))
            {
                selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.HTML5;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["silverlight"]))
            {
                selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.Silverlight;
            }


            if (!string.IsNullOrEmpty(Request.QueryString["i"]))
            {
                // Sanitize the video ID
                string requestedID = LSKYCommon.SanitizeQueryStringID(Request.QueryString["i"]);

                // See if this video exists
                using (SqlConnection connection = new SqlConnection(LSKYCommon.dbConnectionString_ReadOnly))
                {
                    if (Stream.DoesStreamIDExist(connection, requestedID))
                    {
                        selectedStream = Stream.LoadThisStream(connection, requestedID);
                    }
                }
            }

            if (selectedStream != null)
            {
                string originalTitle = Page.Header.Title;
                Page.Header.Title = selectedStream.Name + " - " + originalTitle;

                if (!Request.IsSecureConnection)
                {

                    // Display player
                    if (selectedStream.IsStreamLive())
                    {
                        litPlayer.Text = BuildPlayerHTML(selectedStream, selectedPlayer);
                    }
                    else
                    {
                        litPlayer.Text = BuildNotStreamingMessage(selectedStream);
                    }
                    litStreamInfo.Text = BuildStreamInfoHTML(selectedStream);

                }
                else
                {
                    litPlayer.Text = LSKYCommonHTMLParts.BuildErrorMessage("Streaming video does not work over secure connections - please use a non-encrypted connection");
                }
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