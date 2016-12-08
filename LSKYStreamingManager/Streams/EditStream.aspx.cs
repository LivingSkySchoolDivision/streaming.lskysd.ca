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
    public partial class EditStream : System.Web.UI.Page
    {
        /// <summary>
        /// Parses a stream from the input fields on the page
        /// </summary>
        /// <returns></returns>
        private LiveBroadcast ParseStream()
        {
            string ID = Sanitizers.SanitizeGeneralInputString(lblID.Text);
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

            // Validate
            if (string.IsNullOrEmpty(name)) { throw new Exception("Name cannot be empty. "); }
            if (width <= 0) { throw new Exception("Width must be more than zero."); }
            if (height <= 0) { throw new Exception("Height must be more than zero."); }
            if (startDate == null) { throw new Exception("Start time cannot be null."); }
            if (endDate == null) { throw new Exception("End time cannot be null."); }

            // Return 
            return new LiveBroadcast()
            {
                ID = ID,
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
                IsCancelled = isCancelled
            };




        }

        /// <summary>
        /// 
        /// </summary>
        private void DisplayStream(LiveBroadcast SelectedBroadcast)
        {
            //thumbnail

            lblID.Text = SelectedBroadcast.ID;
            txtTitle.Text = SelectedBroadcast.Name;
            txtStreamLocation.Text = SelectedBroadcast.Location;
            txtDescription.Text = SelectedBroadcast.Description;
            txtWidth.Text = SelectedBroadcast.Width.ToString();
            txtHeight.Text = SelectedBroadcast.Height.ToString();
            txtID.Value = SelectedBroadcast.ID;
            
            txtYouTubeID.Text = SelectedBroadcast.YouTubeID;

            chkHidden.Checked = SelectedBroadcast.IsHidden;
            chkPrivate.Checked = SelectedBroadcast.IsPrivate;
            chkForce.Checked = SelectedBroadcast.ForcedLive;
            chkDelayed.Checked = SelectedBroadcast.IsDelayed;
            chkCancelled.Checked = SelectedBroadcast.IsCancelled;

            // Populate date dropdowns
            txtStartDay.Text = SelectedBroadcast.StartTime.Day.ToString();
            txtEndDay.Text = SelectedBroadcast.EndTime.Day.ToString();
            txtStartHour.Text = SelectedBroadcast.StartTime.Hour.ToString();
            txtEndHour.Text = SelectedBroadcast.EndTime.Hour.ToString();
            txtStartMinute.Text = SelectedBroadcast.StartTime.Minute.ToString().PadLeft(2, '0');
            txtEndMinute.Text = SelectedBroadcast.EndTime.Minute.ToString().PadLeft(2, '0');

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
                if (month == SelectedBroadcast.StartTime.Month)
                {
                    newItem.Selected = true;
                }
                drpStartMonth.Items.Add(newItem);

                newItem = new ListItem();
                newItem.Text = Months[month - 1];
                newItem.Value = month.ToString();
                if (month == SelectedBroadcast.EndTime.Month)
                {
                    newItem.Selected = true;
                }
                drpEndMonth.Items.Add(newItem);
            }

            // Years
            drpStartYear.Items.Clear();
            drpEndYear.Items.Clear();
            for (int year = (SelectedBroadcast.StartTime.Year - 10); year < (SelectedBroadcast.StartTime.Year + 10); year++)
            {
                ListItem newItem = null;
                newItem = new ListItem();
                newItem.Text = year.ToString();
                newItem.Value = year.ToString();
                if (year == SelectedBroadcast.StartTime.Year)
                {
                    newItem.Selected = true;
                }
                drpStartYear.Items.Add(newItem);


                newItem = new ListItem();
                newItem.Text = year.ToString();
                newItem.Value = year.ToString();
                if (year == SelectedBroadcast.EndTime.Year)
                {
                    newItem.Selected = true;
                }
                drpEndYear.Items.Add(newItem);
            }

            // Populate thumbnail dropdown
            imgThumbnail.ImageUrl = "/thumbnails/broadcasts/" + SelectedBroadcast.ThumbnailURL;

            DirectoryInfo ThumbnailDirectory = new DirectoryInfo(Server.MapPath("/thumbnails/broadcasts"));
            foreach (FileInfo file in ThumbnailDirectory.GetFiles())
            {
                if (
                    (file.Extension.ToLower() == ".png") ||
                    (file.Extension.ToLower() == ".jpg") ||
                    (file.Extension.ToLower() == ".gif")
                    )
                {
                    ListItem NewThumb = new ListItem(file.Name, file.Name);
                    if (file.Name == SelectedBroadcast.ThumbnailURL)
                    {
                        NewThumb.Selected = true;
                    }
                    drpThumbnail.Items.Add(NewThumb);




                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the stream ID and attempt to load it

                if (!string.IsNullOrEmpty(Request.QueryString["i"]))
                {
                    string streamID = Request.QueryString["i"].ToString();
                    LiveBroadcastRepository liveBroadcastRepository = new LiveBroadcastRepository();
                    LiveBroadcast lb = liveBroadcastRepository.Get(streamID);

                    if (lb != null)
                    {
                        DisplayStream(lb);
                    }
                    else
                    {
                        displayError("A stream with that ID was not found.");
                    }

                }
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
            imgThumbnail.ImageUrl = "/thumbnails/broadcasts/" + drpThumbnail.SelectedValue;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string streamID = lblID.Text.Trim();
            LiveBroadcastRepository liveBroadcastRepository = new LiveBroadcastRepository();
            LiveBroadcast lb = liveBroadcastRepository.Get(streamID);

            if (lb != null)
            {
                try
                {
                    liveBroadcastRepository.Update(ParseStream());
                }
                catch (Exception ex)
                {
                    displayError(ex.Message);
                }
                
            }
        }

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            string streamID = lblID.Text.Trim();
            LiveBroadcastRepository liveBroadcastRepository = new LiveBroadcastRepository();
            LiveBroadcast lb = liveBroadcastRepository.Get(streamID);

            if (lb != null)
            {
                try
                {
                    liveBroadcastRepository.Delete(ParseStream());
                }
                catch (Exception ex)
                {
                    displayError(ex.Message);
                }

            }
        }
    }
}