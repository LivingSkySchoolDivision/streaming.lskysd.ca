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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate date dropdowns
                txtAirDateDay.Text = txtAvailFromDay.Text = txtAvailToDay.Text = DateTime.Today.Day.ToString();
                txtAirDateHour.Text = txtAvailFromHour.Text = txtAvailToHour.Text = (DateTime.Now.Hour + 1).ToString();
                txtAirDateMin.Text = txtAvailFromMin.Text = txtAvailToMin.Text = "00";
                
                // Months
                String[] Months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                drpAirDateMonth.Items.Clear();
                drpAvailFromMonth.Items.Clear();
                drpAvailToMonth.Items.Clear();
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
                    drpAirDateMonth.Items.Add(newItem);

                    newItem = new ListItem();
                    newItem.Text = Months[month - 1];
                    newItem.Value = month.ToString();
                    if (month == DateTime.Now.Month)
                    {
                        newItem.Selected = true;
                    }
                    drpAvailFromMonth.Items.Add(newItem);

                    newItem = new ListItem();
                    newItem.Text = Months[month - 1];
                    newItem.Value = month.ToString();
                    if (month == DateTime.Now.Month)
                    {
                        newItem.Selected = true;
                    }
                    drpAvailToMonth.Items.Add(newItem);
                }

                // Years
                drpAirDateYear.Items.Clear();
                drpAvailFromYear.Items.Clear();
                drpAvailToYear.Items.Clear();
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
                    drpAirDateYear.Items.Add(newItem);
                    
                    newItem = new ListItem();
                    newItem.Text = year.ToString();
                    newItem.Value = year.ToString();
                    if (year == DateTime.Now.Year)
                    {
                        newItem.Selected = true;
                    }
                    drpAvailFromYear.Items.Add(newItem);

                    newItem = new ListItem();
                    newItem.Text = year.ToString();
                    newItem.Value = year.ToString();
                    if (year == DateTime.Now.Year)
                    {
                        newItem.Selected = true;
                    }
                    drpAvailToYear.Items.Add(newItem);
                }

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
                List<VideoCategory> VideoCategories = new List<VideoCategory>();
                using (SqlConnection connection = new SqlConnection(Settings.dbConnectionString_ReadWrite))
                {
                    VideoCategories = VideoCategory.LoadAll(connection, false);
                }

                drpCategory.Items.Clear();
                foreach (VideoCategory cat in VideoCategories.OrderBy(c => c.GetFullName()).ToList<VideoCategory>())
                {
                    drpCategory.Items.Add(new ListItem(cat.GetFullName(), cat.ID));
                }
            }
        }

        protected void drpThumbnail_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set the preview thumbnail to the selected one
            imgThumbnail.ImageUrl = "/thumbnails/videos/" + drpThumbnail.SelectedValue;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected void chkAlwaysAvailable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAlwaysAvailable.Checked)
            {
                drpAvailFromMonth.Enabled = false;
                drpAvailFromYear.Enabled = false;
                drpAvailToMonth.Enabled = false;
                drpAvailToYear.Enabled = false;
                txtAvailFromDay.Enabled = false;
                txtAvailFromHour.Enabled = false;
                txtAvailFromMin.Enabled = false;
                txtAvailToDay.Enabled = false;
                txtAvailToHour.Enabled = false;
                txtAvailToMin.Enabled = false;
            }
            else
            {
                drpAvailFromMonth.Enabled = true;
                drpAvailFromYear.Enabled = true;
                drpAvailToMonth.Enabled = true;
                drpAvailToYear.Enabled = true;
                txtAvailFromDay.Enabled = true;
                txtAvailFromHour.Enabled = true;
                txtAvailFromMin.Enabled = true;
                txtAvailToDay.Enabled = true;
                txtAvailToHour.Enabled = true;
                txtAvailToMin.Enabled = true;

            }
        }
    }
}