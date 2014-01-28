using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingVideo.videos
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int videos_to_load_for_each_category = 4;

            // Load categories list, and the top videos 
            List<VideoCategory> VideoCategories = new List<VideoCategory>();

            using (SqlConnection connection = new SqlConnection(LSKYCommon.dbConnectionString_ReadOnly))
            {
                VideoCategories = VideoCategory.LoadAllCategories(connection);

                foreach (VideoCategory category in VideoCategories)
                {
                    category.Videos = Video.LoadVideosFromCategory(connection, category, videos_to_load_for_each_category);
                }
            }

            int column_width = 100;

            if (videos_to_load_for_each_category > 0)
            {
                column_width = 100 / videos_to_load_for_each_category;
            }

            litCategories.Text += "";
            StringBuilder CategoryListHTML = new StringBuilder();
                     
            foreach (VideoCategory category in VideoCategories)
            {
                if (category.Videos.Count > 0)
                {
                    CategoryListHTML.Append("<h3>" + category.Name + " (" + category.Videos.Count + ")</h3>");

                    // Videos go here
                    CategoryListHTML.Append("<div style=\"margin-left :10px;\">");
                    CategoryListHTML.Append("<table border=0 cellpadding=0 cellspacing=0 style=\"width: 100%;\">");
                    CategoryListHTML.Append("<tr>");
                    
                    for (int x = 0; x < videos_to_load_for_each_category; x++)
                    {
                        if (category.Videos.Count > x)
                        {
                            CategoryListHTML.Append("<td width=\"" + column_width + "%\" valign=\"top\">" + LSKYCommonHTMLParts.VideoThumbnailListItem(category.Videos[x]) + "</td>");
                        }
                        else
                        {
                            CategoryListHTML.Append("<td width=\"" + column_width + "%\" valign=\"top\">&nbsp;</td>");
                        }
                    } 
                    
                    CategoryListHTML.Append("</tr>");
                    CategoryListHTML.Append("</table></div><br/><br/>");

                }
            }

            litCategories.Text = CategoryListHTML.ToString();


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