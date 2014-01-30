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
                    litPlayer.Text = LSKYCommonHTMLParts.BuildVideoPlayerHTML(selectedVideo, selectedPlayer);
                    litVideoInfo.Text = LSKYCommonHTMLParts.BuildVideoInfoHTML(selectedVideo);
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