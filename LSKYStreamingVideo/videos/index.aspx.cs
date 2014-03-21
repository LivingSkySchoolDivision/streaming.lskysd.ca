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
            returnMe.Append("<li>");
            if (cat.VideoCount > 0)
            {
                returnMe.Append("<a href=\"/Videos/?category=" + cat.ID + "\">");
            }
            returnMe.Append(cat.Name);
            if (cat.VideoCount > 0) 
            {
                returnMe.Append("</a>");
                //returnMe.Append(" <div class=\"video_category_meta\">(" + cat.VideoCount + " videos)</div>");
            }
            returnMe.Append("</li>");
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
                VideoCategories = VideoCategory.LoadAll(connection, true);
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

            // If given a category ID, display all videos from that category
            if (Request.QueryString["category"] != null)
            {
                string parsedCatID = LSKYCommon.SanitizeGeneralInputString(Request.QueryString["category"].ToString().Trim());

                if (!string.IsNullOrEmpty(parsedCatID))
                {
                    VideoCategory SelectedCategory = null;
                    List<Video> CategoryVideos = new List<Video>();
                    using (SqlConnection connection = new SqlConnection(LSKYCommon.dbConnectionString_ReadOnly))
                    {
                        SelectedCategory = VideoCategory.Load(connection, parsedCatID);
                        if (SelectedCategory != null)
                        {
                            CategoryVideos = Video.LoadFromCategory(connection, SelectedCategory);
                        }
                    }

                    StringBuilder VideoListHTML = new StringBuilder();
                    foreach (Video video in CategoryVideos)
                    {
                        VideoListHTML.Append(LSKYCommonHTMLParts.SmallVideoListItem(video, true));
                    }
                    litVideos.Text = VideoListHTML.ToString();
                }
            }
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Sanitize input string
            string SanitizedInputString = LSKYCommon.SanitizeSearchString(txtSearchTerms.Text);

            List<Video> foundVideos = new List<Video>();

            // Try to find videos
            using (SqlConnection connection = new SqlConnection(LSKYCommon.dbConnectionString_ReadOnly))
            {
                foundVideos = Video.Find(connection, SanitizedInputString);
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