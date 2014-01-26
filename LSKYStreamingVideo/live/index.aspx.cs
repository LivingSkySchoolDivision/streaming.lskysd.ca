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

        private string BuildHTML5Player(Stream stream)
        {
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div style=\"width: " + stream.Width + "; margin-left: auto; margin-right: auto;\">");
            returnMe.Append("<video class=\"html5_player\" width=\"" + stream.Width + "\" ");
            returnMe.Append("height=\"" + stream.Height + "\" ");
            returnMe.Append("src=\"/isml/" + stream.ISM_URL + "/manifest(format=m3u8-aapl).m3u8\" ");
            returnMe.Append("poster=\"lsky_stream_poster.png\" ");
            returnMe.Append("autoplay=\"true\" ");
            returnMe.Append("style=\"background-color: white;\" ");
            returnMe.Append("controls=\"true\" >Your browser does not appear to support this streaming video format</video>");
            returnMe.Append("</div>");
            return returnMe.ToString();

        }

        private string BuildSilverlightPlayer(Stream stream)
        {
            // The hight here needs to be 20 pixels higher than the video, so that the controls fit
            int width = stream.Width;
            int height = stream.Height + 20;
            
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div style=\"border: 0px solid black; width: " + width + "px; height: " + height + "px;\">");
            returnMe.Append("<object data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" width=\"" + width + "\" height=\"" + height + "\">");
            returnMe.Append("<param name=\"source\" value=\"StrendinSmoothStream.xap\"/>");
            returnMe.Append("<param name=\"onError\" value=\"onSilverlightError\" />");
            returnMe.Append("<param name=\"background\" value=\"white\" />");
            returnMe.Append("<param name=\"minRuntimeVersion\" value=\"4.0.50826.0\" />");
            returnMe.Append("<param name=\"autoUpgrade\" value=\"true\" />");
            returnMe.Append("<param name=\"initParams\" value=\"streamuri=/isml/" + stream.ISM_URL + "/Manifest\" />");
            returnMe.Append("<a href=\"http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0\" style=\"text-decoration:none\">");
            returnMe.Append("<img src=\"http://go.microsoft.com/fwlink/?LinkId=161376\" alt=\"Get Microsoft Silverlight\" style=\"border-style:none\"/>");
            returnMe.Append("</a>");
            returnMe.Append("</object>");
            returnMe.Append("</div>");
            return returnMe.ToString();
        }

        private bool CanClientBrowserSupportSilverlight()
        {
            return false;
        }

        enum Player
        {
            Silverlight,
            HTML5
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
        
        private string BuildPlayerHTML(Stream stream, Player player)
        {
            if (player == Player.HTML5)
            {
                return BuildHTML5Player(stream);
            }

            if (player == Player.Silverlight)
            {
                return BuildSilverlightPlayer(stream);
            }
            return string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Stream selectedStream = null;
            Player selectedPlayer = Player.HTML5;

            // Check to see which player we should be using based on the client's browser
            if ((getInternetExplorerVersion() < 10) && (getInternetExplorerVersion() != -1))
            {
                selectedPlayer = Player.Silverlight;
            }

            // Check to see if the user has specified a player to use
            if (!string.IsNullOrEmpty(Request.QueryString["html"]))
            {
                selectedPlayer = Player.HTML5;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["silverlight"]))
            {
                selectedPlayer = Player.Silverlight;
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