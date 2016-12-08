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
        private string addMenuChildren(List<VideoCategory> children)
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<ul>");

            foreach (VideoCategory category in children.Where(c => !c.IsHidden))
            {
                returnMe.Append("<li>");
                if (category.VideoCount > 0) { returnMe.Append("<a href=\"/Videos/?category=" + category.ID + "\">"); }
                returnMe.Append(category.Name);
                if (category.VideoCount > 0) { returnMe.Append("</a>"); }

                returnMe.Append("</li>");
                if (category.HasChildren)
                {
                    returnMe.Append(addMenuChildren(category.Children));
                }
            }

            returnMe.Append("</ul>");

            return returnMe.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {         
            // If the user hasn't selected a category, display a list of categories
            // If the user has selected a category, display all videos in that category
            
            // Always list categories 
            VideoCategoryRepository videoCategoryRepo = new VideoCategoryRepository();
            List<VideoCategory> VideoCategories = videoCategoryRepo.GetTopLevel();


            litCategories.Text = addMenuChildren(VideoCategories);

            // If given a category ID, display all videos from that category
            if (!string.IsNullOrEmpty(Request.QueryString["category"]))
            {
                string parsedCatID = Sanitizers.SanitizeGeneralInputString(Request.QueryString["category"].ToString().Trim());
                
                if (!string.IsNullOrEmpty(parsedCatID))
                {
                    VideoCategory selectedCategory = videoCategoryRepo.Get(parsedCatID);
                    if (selectedCategory != null)
                    {
                        // Determine if the viewer is viewing from inside the network
                        string clientIP = Request.ServerVariables["REMOTE_ADDR"];
                        bool canUserAccessPrivateContent = Config.CanAccessPrivate(clientIP);

                        
                        VideoRepository videoRepository = new VideoRepository();
                        List<Video> CategoryVideos = videoRepository.GetFromCategory(selectedCategory, canUserAccessPrivateContent);

                        StringBuilder VideoListHTML = new StringBuilder();
                        foreach (Video video in CategoryVideos)
                        {
                            VideoListHTML.Append(videoListItem(video));
                        }
                        litVideos.Text = VideoListHTML.ToString();
                        
                    }
                }
            }
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Sanitize input string
            string SanitizedInputString = Sanitizers.SanitizeSearchString(txtSearchTerms.Text);

            // Determine if the viewer is viewing from inside the network
            string clientIP = Request.ServerVariables["REMOTE_ADDR"];
            bool canUserAccessPrivateContent = Config.CanAccessPrivate(clientIP);

            VideoRepository videoRepository = new VideoRepository();
            List<Video> foundVideos = videoRepository.Find(SanitizedInputString, canUserAccessPrivateContent);
            
            searchResultsTitle.Visible = true;
            litSearchResults.Visible = true;             
            if (foundVideos.Count > 0)
            {
                litSearchResults.Text = "";
                foreach (Video video in foundVideos)
                {
                    litSearchResults.Text += videoListItem(video);
                }
            }
            else
            {
                litSearchResults.Text = "No videos found matching the term '" + SanitizedInputString + "'";
            }

            litCategories.Visible = false;
            litVideos.Visible = false;
        }

        public static string videoListItem(Video video)
        {
            StringBuilder returnMe = new StringBuilder();

            string thumbnailURL = "none.png";
            string playerURL = "/player/?i=" + video.ID;

            if (!string.IsNullOrEmpty(video.ThumbnailURL))
            {
                thumbnailURL = video.ThumbnailURL;
            }

            returnMe.Append("<div class=\"SmallVideoListItem\">");
            
            returnMe.Append("<div class=\"VideoListThumb\" width=\"128\">");
            returnMe.Append("<a href=\"" + playerURL + "\">");
            //returnMe.Append("<div style=\"width: 200px;\">");
            returnMe.Append("<img border=\"0\" src=\"/thumbnails/videos/" + thumbnailURL + "\" class=\"video_thumbnail_list_item_container_image\">");
            returnMe.Append("</a>");
            returnMe.Append("</div>");
            
            returnMe.Append("<div class=\"video_list_info_container\">");
            returnMe.Append("<ul>");
            returnMe.Append("<li class=\"VideoListDescTitle\"> <a style=\"text-decoration: none;\" href=\"" + playerURL + "\"><div class=\"video_list_name\">" + video.Name + "</div></a> </li>");
            returnMe.Append("<li class=\"VideoListDescDuration\"> <div class=\"video_list_info\"><b>Duration:</b> " + video.DurationInEnglish + "</div></li>");
            returnMe.Append("<li class=\"VideoListDescSubmitted\"> <div class=\"video_list_info\"><b>Submitted by:</b> " + video.Author + "</div></li>");
            returnMe.Append("</ul>");

            if (video.IsPrivate)
            {
                returnMe.Append("<div class=\"video_list_info\"><b>This video is flagged as private</b></div>");
            }
            
            if (!string.IsNullOrEmpty(video.DownloadURL))
            {
                returnMe.Append("<div class=\"video_list_info\">Download available</div>");
            }
            returnMe.Append("<br/><div class=\"video_list_description\">" + video.Description + "</div>");

            returnMe.Append("</div></td>");


            returnMe.Append("</ul>");
            returnMe.Append("</div>");

            return returnMe.ToString();

        }
    }
}