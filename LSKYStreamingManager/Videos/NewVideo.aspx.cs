using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.Videos
{
    public partial class NewVideo : System.Web.UI.Page
    {
        private Video parseVideo()
        {
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string description = txtDescription.Text;

            
            string tagsRaw = txtTags.Text;
            List<string> tags = new List<string>();
            foreach (string tag in tagsRaw.Split(' '))
            {
                string tagSanitized = tag.Trim();
                if (!string.IsNullOrEmpty(tagSanitized))
                {
                    if (!tags.Contains(tagSanitized))
                    {
                        tags.Add(tagSanitized);
                    }
                }
            }

            int width = Parsers.ParseInt(txtWidth.Text);
            int height = Parsers.ParseInt(txtHeight.Text);
            string thumbnail = drpThumbnail.SelectedValue;
            int durationInSeconds = Parsers.ParseInt(txtDuration.Text);
            string youtubeURL = txtYoutubeURL.Text;
            int youtubeStartTimeInSeconds = Parsers.ParseInt(txtYoutubeStartTime.Text);
            string file_mp4 = txtMP4URL.Text;
            string file_ogg = txtOGVURL.Text;
            string file_webm = txtWEBM.Text;
            string downloadURL = txtDownloadURL.Text;
            string cateogryID = drpCategory.SelectedValue;
            bool ishidden = chkhidden.Checked;
            bool isprivate = chkPrivate.Checked;

            // Validate
            if (string.IsNullOrEmpty(title)) { throw new Exception("Title cannot be empty."); }
            if (width <= 0) { throw new Exception("Width must be greater than zero"); }
            if (height <= 0) { throw new Exception("Height must be greater than zero"); }
            

            return new Video()
            {
                Name = title,
                Author = author,
                Description = description,
                Width = width,
                Height = height,
                DateAdded = DateTime.Now,
                DurationInSeconds = durationInSeconds,
                FileURL_H264 = file_mp4,
                FileURL_THEORA = file_ogg,
                FileURL_VP8 = file_webm,
                DownloadURL = downloadURL,
                YoutubeURL = youtubeURL,
                YoutubeStartTimeInSeconds = youtubeStartTimeInSeconds,
                IsHidden = ishidden,
                IsPrivate = isprivate,
                Tags = tags,
                ThumbnailURL = thumbnail,
                CategoryID = cateogryID,

            };

        }

        private void RedirectToVideoList(string newVideoID)
        {
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + "/Videos/?highlight=" + newVideoID);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate thumbnail dropdown
                DirectoryInfo ThumbnailDirectory = new DirectoryInfo(Server.MapPath("/thumbnails/videos"));
                foreach (FileInfo file in ThumbnailDirectory.GetFiles())
                {
                    if (
                        (file.Extension.ToLower() == ".png") ||
                        (file.Extension.ToLower() == ".jpg") ||
                        (file.Extension.ToLower() == ".gif")
                        )
                    {
                        ListItem Thumb = new ListItem(file.Name, file.Name);
                        if (file.Name == "blank.png")
                        {
                            Thumb.Selected = true;
                        }
                        drpThumbnail.Items.Add(Thumb);
                    }
                }

                // Populate list of categories
                VideoCategoryRepository videoCategoryRepository = new VideoCategoryRepository();
                List<VideoCategory> VideoCategories = videoCategoryRepository.GetAll();
                
                drpCategory.Items.Clear();
                drpCategory.Items.Add(new ListItem(" - No Category -", string.Empty));
                foreach (VideoCategory cat in VideoCategories.OrderBy(c => c.FullName).ToList<VideoCategory>())
                {
                    drpCategory.Items.Add(new ListItem(cat.FullName, cat.ID));
                }
            }
        }

        protected void drpThumbnail_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set the preview thumbnail to the selected one
            imgThumbnail.ImageUrl = "/thumbnails/videos/" + drpThumbnail.SelectedValue;
        }

        private void displayError(string errorText)
        {
            lblError.Text = errorText;
            lblError.Visible = true;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            VideoRepository videoRepository = new VideoRepository();
            try
            {
                Video parsedVideo = parseVideo();
                videoRepository.Insert(parsedVideo);
                RedirectToVideoList(parsedVideo.ID);
            }
            catch (Exception ex)
            {
                displayError(ex.Message);
            }
        }
        
    }
}