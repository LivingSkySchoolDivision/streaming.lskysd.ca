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
        private string ListCategoryWithChildren(VideoCategory cat)
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<li>" + cat.Name + " (" + cat.VideoCount + ") <small>(" + cat.Children.Count + ")</small></li>");
            if (cat.HasChildren())
            {
                returnMe.Append("<ul>");
                foreach (VideoCategory child in cat.Children)
                {
                    returnMe.Append(ListCategoryWithChildren(child));
                }
                returnMe.Append("</ul>");
            }

            return returnMe.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {         
            // If the user hasn't selected a category, display a list of categories
            // If the user has selected a category, display all videos in that category
            
            // Always list categories 
            List<VideoCategory> VideoCategories = new List<VideoCategory>();
            using (SqlConnection connection = new SqlConnection(LSKYCommon.dbConnectionString_ReadOnly))
            {
                VideoCategories = VideoCategory.LoadAll(connection);
            }

            StringBuilder CategoryListHTML = new StringBuilder();

            CategoryListHTML.Append("<ul>");

            
            foreach (VideoCategory category in VideoCategories)
            {
                if ((!category.IsHidden) && (!category.IsPrivate))
                {
                    CategoryListHTML.Append(ListCategoryWithChildren(category));
                }
            }

            CategoryListHTML.Append("</ul>");
            litCategories.Text = CategoryListHTML.ToString();
            
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

            litCategories.Visible = false;
            litVideos.Visible = false;

        }
    }
}