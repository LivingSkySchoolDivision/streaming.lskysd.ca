using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYStreamingCore;
using LSKYStreamingCore.ExtensionMethods;

namespace LSKYStreamingManager.Videos
{
    public partial class EditVideo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the video ID and attempt to load it

                if (!string.IsNullOrEmpty(Request.QueryString["i"]))
                {
                    string streamID = Request.QueryString["i"].ToString();
                    VideoRepository videoRepository = new VideoRepository();
                    Video video = videoRepository.Get(streamID);

                    if (video != null)
                    {
                        displayVideo(video);
                    }
                    else
                    {
                        displayError("A stream with that ID was not found.");
                    }

                }
            }
        }

        private void displayError(string errorText)
        {
            lblError.Text = errorText;
            lblError.Visible = true;
        }
        
        private Video parseVideo()
        {
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            string description = txtDescription.Text;
            string id = lblID.Text;

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
            bool ishidden = chkhidden.Checked;
            bool isprivate = chkPrivate.Checked;

            // Category
            string cateogryID = string.Empty;

            VideoCategoryRepository videoCategoryRepo = new VideoCategoryRepository();
            VideoCategory category = videoCategoryRepo.Get(drpCategory.SelectedValue);
            if (category != null)
            {
                cateogryID = category.ID;
            }

            // Validate
            if (string.IsNullOrEmpty(title)) { throw new Exception("Title cannot be empty."); }
            if (width <= 0) { throw new Exception("Width must be greater than zero"); }
            if (height <= 0) { throw new Exception("Height must be greater than zero"); }


            return new Video()
            {
                ID = id,
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

        private void displayVideo(Video video)
        {
            lblID.Text = video.ID;
            txtTitle.Text = video.Name;
            txtAuthor.Text = video.Author;
            txtDescription.Text = video.Description;
            txtTags.Text = video.Tags.ToSpaceSeparatedString();
            txtWidth.Text = video.Width.ToString();
            txtHeight.Text = video.Height.ToString();
            txtDuration.Text = video.DurationInSeconds.ToString();
            txtYoutubeURL.Text = video.YoutubeURL;
            txtYoutubeStartTime.Text = video.YoutubeStartTimeInSeconds.ToString();
            txtMP4URL.Text = video.FileURL_H264;
            txtOGVURL.Text = video.FileURL_THEORA;
            txtWEBM.Text = video.FileURL_VP8;
            txtDownloadURL.Text = video.DownloadURL;

            chkhidden.Checked = video.IsHidden;
            chkPrivate.Checked = video.IsPrivate;
            
            // Category
            VideoCategoryRepository videoCategoryRepo = new VideoCategoryRepository();
            drpCategory.Items.Clear();
            drpCategory.Items.Add(new ListItem() { Text=" - No Category -", Value = "0"} );
            foreach (VideoCategory category in videoCategoryRepo.GetAll().OrderBy(c => c.FullName))
            {
                bool selected = video.CategoryID == category.ID;
                drpCategory.Items.Add(new ListItem() { Selected = selected, Text = category.FullName, Value = category.ID });
            }


            // Thumbnail

            imgThumbnail.ImageUrl = "/thumbnails/videos/" + video.ThumbnailURL;
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
                    if (video.ThumbnailURL == file.Name)
                    {
                        Thumb.Selected = true;
                    }
                    drpThumbnail.Items.Add(Thumb);
                }
            }
        }

        protected void drpThumbnail_SelectedIndexChanged(object sender, EventArgs e)
        {
            imgThumbnail.ImageUrl = "/thumbnails/videos/" + drpThumbnail.SelectedValue;
        }

        protected void btnEdit_OnClick(object sender, EventArgs e)
        {
            string videoID = lblID.Text.Trim();
            VideoRepository videoRepository = new VideoRepository();
            Video video = videoRepository.Get(videoID);

            if (video != null)
            {
                try
                {
                    Video editedVideo = parseVideo();
                    videoRepository.Update(editedVideo);
                }
                catch (Exception ex)
                {
                    displayError(ex.Message);
                }
            }
        }

        private void RedirectToVideoList(string newVideoID)
        {
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + "/Videos/?highlight=" + newVideoID);
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            string videoID = lblID.Text.Trim();
            VideoRepository videoRepository = new VideoRepository();
            Video video = videoRepository.Get(videoID);

            if (video != null)
            {
                try
                {
                    videoRepository.Delete(video);
                    RedirectToVideoList(string.Empty);
                }
                catch (Exception ex)
                {
                    displayError(ex.Message);
                }
            }

        }
    }
}