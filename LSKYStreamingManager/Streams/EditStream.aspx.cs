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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the stream ID and attempt to load it
                LiveBroadcast SelectedBroadcast = null;
                
                if (Request.QueryString["i"] != null)
                {
                    using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                    {
                        SelectedBroadcast = LiveBroadcast.LoadThisBroadcast(connection, Request.QueryString["i"].Trim().ToString());
                    }
                }

                // If unable to load, just display an error
                if (SelectedBroadcast != null)
                {
                    // Populate stream data fields
                    txtTitle.Text = SelectedBroadcast.Name;
                    txtDescription.Text = SelectedBroadcast.Description;
                    txtStreamLocation.Text = SelectedBroadcast.Location;
                    txtWidth.Text = SelectedBroadcast.Width.ToString();
                    txtHeight.Text = SelectedBroadcast.Height.ToString();
                    txtSidebar.Text = SelectedBroadcast.SidebarContent;
                    txtID.Value = SelectedBroadcast.ID;
                    lblID.Text = SelectedBroadcast.ID;
                    txtYouTubeID.Text = SelectedBroadcast.YouTubeID;

                    if (SelectedBroadcast.IsHidden)
                    {
                        chkHidden.Checked = true;
                    }
                    else
                    {
                        chkHidden.Checked = false;
                    }

                    if (SelectedBroadcast.IsPrivate)
                    {
                        chkPrivate.Checked = true;
                    }
                    else
                    {
                        chkPrivate.Checked = false;
                    }

                    if (SelectedBroadcast.ForcedLive)
                    {
                        chkForce.Checked = true;
                    }
                    else
                    {
                        chkForce.Checked = false;
                    }

                    if (SelectedBroadcast.DisplaySidebar)
                    {
                        chkSidebar.Checked = true;
                    }
                    else
                    {
                        chkSidebar.Checked = false;
                    }
                    
                    // Populate date dropdowns
                    txtStartDay.Text = SelectedBroadcast.StartTime.Day.ToString();
                    txtEndDay.Text = SelectedBroadcast.EndTime.Day.ToString();
                    txtStartHour.Text = SelectedBroadcast.StartTime.Hour.ToString();
                    txtEndHour.Text = SelectedBroadcast.EndTime.Hour.ToString();
                    txtStartMinute.Text = SelectedBroadcast.StartTime.Minute.ToString().PadLeft(2, '0');
                    txtEndMinute.Text = SelectedBroadcast.EndTime.Minute.ToString().PadLeft(2,'0');
                    

                    
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

                    // Populate isml dropdown
                    DirectoryInfo ISMLDirectory = new DirectoryInfo(Server.MapPath("/isml"));
                    foreach (FileInfo file in ISMLDirectory.GetFiles("*.isml"))
                    {
                        ListItem NewFileItem = new ListItem(file.Name, file.Name);
                        if (file.Name == SelectedBroadcast.ISM_URL)
                        {
                            NewFileItem.Selected = true;
                        }

                        drpISML.Items.Add(NewFileItem);
                    }

                }
                else
                {
                    // Broadcast couldn't be loaded
                    displayError("Broadcast could not be loaded");
                    tblControls.Visible = false;
                    tblControls2.Visible = false;
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
            // Text values
            string id = LSKYCommon.SanitizeGeneralInputString(txtID.Value);
            string name = LSKYCommon.SanitizeGeneralInputString(txtTitle.Text);
            string location = LSKYCommon.SanitizeGeneralInputString(txtStreamLocation.Text);
            string description = LSKYCommon.SanitizeGeneralInputString(txtDescription.Text);
            string thumbnail = LSKYCommon.SanitizeGeneralInputString(drpThumbnail.SelectedValue);
            int width = LSKYCommon.ParseDatabaseInt(txtWidth.Text);
            int height = LSKYCommon.ParseDatabaseInt(txtHeight.Text);
            string isml = LSKYCommon.SanitizeGeneralInputString(drpISML.SelectedValue);
            string sidebar = LSKYCommon.SanitizeGeneralInputString(txtSidebar.Text);
            string youtube_id = LSKYCommon.SanitizeGeneralInputString(txtYouTubeID.Text);
            // Dates
            DateTime? startDate = LSKYCommon.ParseDateFromUser(drpStartYear.SelectedValue, drpStartMonth.SelectedValue, txtStartDay.Text, txtStartHour.Text, txtStartMinute.Text, "00");
            DateTime? endDate = LSKYCommon.ParseDateFromUser(drpEndYear.SelectedValue, drpEndMonth.SelectedValue, txtEndDay.Text, txtEndHour.Text, txtEndMinute.Text, "00");

            // Binary values
            bool ishidden = false;
            if (chkHidden.Checked)
            {
                ishidden = true;
            }

            bool isprivate = false;
            if (chkPrivate.Checked)
            {
                isprivate = true;
            }

            bool showsidebar = false;
            if (chkSidebar.Checked)
            {
                showsidebar = true;
            }

            bool forceonline = false;
            if (chkForce.Checked)
            {
                forceonline = true;
            }

            if (endDate != null)
            {
                if (startDate != null)
                {

                    // Create a new stream in the database
                    LiveBroadcast newBroadcast = new LiveBroadcast(
                        id,
                        name,
                        location,
                        description,
                        thumbnail,
                        width,
                        height,
                        isml,
                        (DateTime)startDate,
                        (DateTime)endDate,
                        showsidebar,
                        true,
                        ishidden,
                        isprivate,
                        forceonline,
                        sidebar,
                        youtube_id
                        );

                    using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                    {         
                        if (LiveBroadcast.UpdateBroadcast(connection, newBroadcast))
                        {
                            RedirectToStreamList(id);
                        }
                        else
                        {
                            displayError("Error updating stream");
                        }
                        
                    }
                }
                else
                {
                    displayError("Error parsing start date");
                }
            }
            else
            {
                displayError("Error parsing end date");
            }
        }
    }
}