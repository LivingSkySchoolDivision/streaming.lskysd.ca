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
        private string BuildNotStreamingMessage(LiveBroadcast stream)
        {
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div class=\"large_infobox\" style=\"\">Stream not currently broadcasting</div> ");
            return returnMe.ToString();

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

        protected void Page_Load(object sender, EventArgs e)
        {
            LiveBroadcast selectedStream = null;
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
                    if (LiveBroadcast.DoesStreamIDExist(connection, requestedID))
                    {
                        selectedStream = LiveBroadcast.LoadThisStream(connection, requestedID);
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
                        litPlayer.Text = LSKYCommonHTMLParts.BuildLiveStreamPlayerHTML(selectedStream, selectedPlayer);
                    }
                    else
                    {
                        litPlayer.Text = BuildNotStreamingMessage(selectedStream);
                    }
                    litStreamInfo.Text = LSKYCommonHTMLParts.BuildLiveStreamInfoHTML(selectedStream);

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