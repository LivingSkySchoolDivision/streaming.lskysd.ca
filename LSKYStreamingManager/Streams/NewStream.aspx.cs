﻿using LSKYStreamingCore;
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

                for (int x = DateTime.Now.Year - 2; x < DateTime.Now.Year + 5; x++)
                {
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
                }

                // Populate thumbnail dropdown
                DirectoryInfo ThumbnailDirectory = new DirectoryInfo(Server.MapPath("/thumbnails/small"));
                foreach (FileInfo file in ThumbnailDirectory.GetFiles())
                {
                    if (
                        (file.Extension.ToLower() == ".png") ||
                        (file.Extension.ToLower() == ".jpg") ||
                        (file.Extension.ToLower() == ".gif")
                        )
                    {
                        drpThumbnail.Items.Add(new ListItem(file.Name, file.Name));
                    }
                }

                // Populate isml dropdown
                DirectoryInfo ISMLDirectory = new DirectoryInfo(Server.MapPath("/isml"));
                foreach (FileInfo file in ISMLDirectory.GetFiles("*.isml"))
                {
                    drpISML.Items.Add(new ListItem(file.Name, file.Name));                    
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

            imgThumbnail.ImageUrl = "/thumbnails/small/" + drpThumbnail.SelectedValue;

        }

        private DateTime? ParseDateFromUser(string year, string month, string day, string hour, string minute, string second)
        {
            int ParsedYear = LSKYCommon.ParseDatabaseInt(year);
            int ParsedMonth = LSKYCommon.ParseDatabaseInt(month);
            int ParsedDay = LSKYCommon.ParseDatabaseInt(day);
            int ParsedHour = LSKYCommon.ParseDatabaseInt(hour);
            int ParsedMinute = LSKYCommon.ParseDatabaseInt(minute);
            int ParsedSecond = LSKYCommon.ParseDatabaseInt(second);

            // Validate

            // Days in month            

            if ((ParsedYear > 0) && (ParsedYear < 9999))
            {
                if ((ParsedMonth >= 1) && (ParsedMonth <=12)) 
                {
                    int DaysThisMonth = DateTime.DaysInMonth(ParsedYear,ParsedMonth);                    
                    if ((ParsedDay > 0) && (ParsedDay <= DaysThisMonth)) 
                    {
                        if ((ParsedHour >= 0) && (ParsedHour <= 24))
                        {
                            if ((ParsedMinute >= 0) && (ParsedMinute <= 60)) 
                            {
                                if ((ParsedSecond >= 0) && (ParsedSecond <= 60))
                                {
                                    return new DateTime(ParsedYear, ParsedMonth, ParsedDay, ParsedHour, ParsedMinute, ParsedSecond);
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate inputted data

            // Text values
            string name = LSKYCommon.SanitizeGeneralInputString(txtTitle.Text);
            string location = LSKYCommon.SanitizeGeneralInputString(txtStreamLocation.Text);
            string description = LSKYCommon.SanitizeGeneralInputString(txtDescription.Text);
            string thumbnail = LSKYCommon.SanitizeGeneralInputString(drpThumbnail.SelectedValue);
            int width = LSKYCommon.ParseDatabaseInt(txtWidth.Text);
            int height = LSKYCommon.ParseDatabaseInt(txtHeight.Text);
            string isml = LSKYCommon.SanitizeGeneralInputString(drpISML.SelectedValue);
            string sidebar = LSKYCommon.SanitizeGeneralInputString(txtSidebar.Text);

            // Dates
            DateTime? startDate = ParseDateFromUser(drpStartYear.SelectedValue, drpStartMonth.SelectedValue, txtStartDay.Text, txtStartHour.Text, txtStartMinute.Text, "00");
            DateTime? endDate = ParseDateFromUser(drpEndYear.SelectedValue, drpEndMonth.SelectedValue, txtEndDay.Text, txtEndHour.Text, txtEndMinute.Text, "00");
            
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
                    using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                    {
                        // Create a unique ID for the stream
                        string newStreamID = string.Empty;
                        do
                        {
                            newStreamID = LSKYCommon.getNewID(6);
                        } while (LiveBroadcast.DoesIDExist(connection, newStreamID));

                        // Create a new stream in the database
                        LiveBroadcast newBroadcast = new LiveBroadcast(
                            newStreamID,
                            name, 
                            location,
                            description,
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
                            sidebar
                            );

                        LiveBroadcast.InsertNewBroadcast(connection, newBroadcast);
                        RedirectToStreamList(newStreamID);
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