using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingVideo.videos
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Load categories list
            List<VideoCategory> VideoCategories = new List<VideoCategory>();

            
            // Load top videos for those categories

            // Don't display empty categories
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Sanitize input string
            string SanitizedInputString = LSKYCommon.SanitizeSearchString(txtSearchTerms.Text);

            List<Video> foundVideos = new List<Video>();

            // Try to find videos
            using (SqlConnection connection = new SqlConnection(LSKYCommon.dbConnectionString_ReadOnly))
            {
                foundVideos = Video.SearchVideos(connection, SanitizedInputString);
            }


            searchResultsTitle.Visible = true;
            litSearchResults.Visible = true;             
            if (foundVideos.Count > 0)
            {
                litSearchResults.Text = "";
                foreach (Video video in foundVideos)
                {
                    litSearchResults.Text += LSKYCommonHTMLParts.SmallVideoListItem(video, true);
                }
            }
            else
            {
                litSearchResults.Text = "No videos found matching the term '" + SanitizedInputString + "'";
            }

        }
    }
}