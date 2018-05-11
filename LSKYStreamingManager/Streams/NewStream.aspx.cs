using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.Streams
{
    public partial class NewStream : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate date dropdowns
                txtStartDay.Text = DateTime.Today.Day.ToString();
                txtEndDay.Text = DateTime.Today.Day.ToString();
                txtStartHour.Text = (DateTime.Now.Hour + 1).ToString();
                txtEndHour.Text = (DateTime.Now.Hour + 2).ToString();
                txtStartMinute.Text = "00";
                txtEndMinute.Text = "00";
                
                // Months
                String[] Months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                drpStartMonth.Items.Clear();
                drpEndMonth.Items.Clear();
                for (int month = 1; month <= 12; month++)
                {
                    ListItem newItem = null;
                    newItem = new ListItem();
                    newItem.Text = Months[month - 1];
                    newItem.Value = month.ToString();
                    if (month == DateTime.Now.Month)
                    {
                        newItem.Selected = true;
                    }
                    drpStartMonth.Items.Add(newItem);

                    newItem = new ListItem();
                    newItem.Text = Months[month - 1];
                    newItem.Value = month.ToString();
                    if (month == DateTime.Now.Month)
                    {
                        newItem.Selected = true;
                    }
                    drpEndMonth.Items.Add(newItem);
                }

                // Years
                drpStartYear.Items.Clear();
                drpEndYear.Items.Clear();
                for (int year = (DateTime.Now.Year - 1); year < (DateTime.Now.Year + 5); year++)
                {
                    ListItem newItem = null;
                    newItem = new ListItem();
                    newItem.Text = year.ToString();
                    newItem.Value = year.ToString();
                    if (year == DateTime.Now.Year)
                    {
                        newItem.Selected = true;
                    }
                    drpStartYear.Items.Add(newItem);


                    newItem = new ListItem();
                    newItem.Text = year.ToString();
                    newItem.Value = year.ToString();
                    if (year == DateTime.Now.Year)
                    {
                        newItem.Selected = true;
                    }
                    drpEndYear.Items.Add(newItem);
                }
                

                // Populate thumbnail dropdown
                /*
                DirectoryInfo ThumbnailDirectory = new DirectoryInfo(Server.MapPath("/thumbnails/broadcasts"));
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
                }*/
            }

        }

        private void RedirectToStreamList(string newStreamID)
        {
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + "/Streams/?highlight=" + newStreamID);
        }

        private void displayError(string errorText)
        {
            lblError.Text = errorText;
            lblError.Visible = true;
        }

        protected void drpThumbnail_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set the preview thumbnail to the selected one
            imgThumbnail.ImageUrl = Settings.ThumbnailPath + "/broadcasts/" + drpThumbnail.SelectedValue;
        }

        /// <summary>
        /// Parses a stream from the input fields on the page
        /// </summary>
        /// <returns></returns>
        private LiveBroadcast ParseStream()
        {
            string name = Sanitizers.SanitizeGeneralInputString(txtTitle.Text);
            string location = Sanitizers.SanitizeGeneralInputString(txtStreamLocation.Text);
            string description = Sanitizers.SanitizeGeneralInputString(txtDescription.Text);
            string thumbnail = Sanitizers.SanitizeGeneralInputString(drpThumbnail.SelectedValue);
            int width = Parsers.ParseInt(txtWidth.Text);
            int height = Parsers.ParseInt(txtHeight.Text);
            string YouTubeID = Sanitizers.SanitizeGeneralInputString(txtYouTubeID.Text);

            DateTime? startDate = Parsers.ParseDateFromUser(drpStartYear.SelectedValue, drpStartMonth.SelectedValue, txtStartDay.Text, txtStartHour.Text, txtStartMinute.Text, "00");
            DateTime? endDate = Parsers.ParseDateFromUser(drpEndYear.SelectedValue, drpEndMonth.SelectedValue, txtEndDay.Text, txtEndHour.Text, txtEndMinute.Text, "00");
            
            bool ishidden = chkHidden.Checked;
            bool isprivate = chkPrivate.Checked;
            bool forceonline = chkForce.Checked;
            bool isDelayed = chkDelayed.Checked;
            bool isCancelled = chkCancelled.Checked;
            bool embed = chkEmbed.Checked;

            // Validate
            if (string.IsNullOrEmpty(name)) { throw new Exception("Name cannot be empty. "); }
            if (width <= 0) { throw new Exception("Width must be more than zero.");}
            if (height <= 0) { throw new Exception("Height must be more than zero.");}
            if (startDate == null) { throw new Exception("Start time cannot be null."); }
            if (endDate == null) { throw new Exception("End time cannot be null."); }

            // Return 
            return new LiveBroadcast()
            {
                Name = name,
                Location = location,
                Description = description,
                ThumbnailURL = thumbnail,
                Width = width,
                Height = height,
                YouTubeID = YouTubeID,
                StartTime = startDate.Value,
                EndTime = endDate.Value,
                ForcedLive = forceonline,
                IsPrivate = isprivate,
                IsHidden = ishidden,
                IsDelayed = isDelayed,
                IsCancelled = isCancelled,
                EmbedInsteadOfLink = embed                
            };
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                LiveBroadcastRepository liveBroadcastRepository = new LiveBroadcastRepository();
                LiveBroadcast stream = ParseStream();
                liveBroadcastRepository.Insert(stream);
                RedirectToStreamList(stream.ID);
            }
            catch (Exception ex)
            {
                displayError(ex.Message);
            }
        }
    }
}