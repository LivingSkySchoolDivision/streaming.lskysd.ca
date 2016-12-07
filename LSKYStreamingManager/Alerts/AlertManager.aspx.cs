using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.Alerts
{
    public partial class AlertManager : System.Web.UI.Page
    {
        private TableRow addAlertRow(Alert alert)
        {
            TableRow returnMe = new TableRow();
            TableCell cell_name = new TableCell();
            TableCell cell_dateFrom = new TableCell();
            TableCell cell_dateTo = new TableCell();
            TableCell cell_importance = new TableCell();
            TableCell cell_deletebutton = new TableCell();

            cell_name.Text = alert.Content;
            cell_dateFrom.Text = alert.DisplayFrom.ToShortDateString() + " " + alert.DisplayFrom.ToShortTimeString();
            cell_dateTo.Text = alert.DisplayTo.ToShortDateString() + " " + alert.DisplayTo.ToShortTimeString();

            if (alert.Importance == AlertImportance.High)
            {
                cell_importance.Text = "High";
            }
            else
            {
                cell_importance.Text = "Normal";
            }

            cell_deletebutton.Text = "<a href=\"?DELETEALERT=" + alert.ID + "\">DELETE</a>";

            cell_deletebutton.HorizontalAlign = HorizontalAlign.Center;
            cell_importance.HorizontalAlign = HorizontalAlign.Center;

            returnMe.Cells.Add(cell_name);
            returnMe.Cells.Add(cell_dateFrom);
            returnMe.Cells.Add(cell_dateTo);
            returnMe.Cells.Add(cell_importance);
            returnMe.Cells.Add(cell_deletebutton);

            return returnMe;
        }

        private void ResetInputFields()
        {
            txtAlertContent.Text = "";
            txtHour_From.Text = DateTime.Now.Hour.ToString();
            txtHour_To.Text = DateTime.Now.AddHours(1).Hour.ToString();
            txtMinute_From.Text = "00";
            txtMinute_To.Text = "00";
            txtDay_From.Text = DateTime.Now.Day.ToString();
            txtDay_To.Text = DateTime.Now.Day.ToString();

            // Months
            String[] Months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            drpMonth_To.Items.Clear();
            drpMonth_From.Items.Clear();
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
                drpMonth_From.Items.Add(newItem);

                newItem = new ListItem();
                newItem.Text = Months[month - 1];
                newItem.Value = month.ToString();
                if (month == DateTime.Now.Month)
                {
                    newItem.Selected = true;
                }
                drpMonth_To.Items.Add(newItem);
            }

            // Years
            drpYear_To.Items.Clear();
            drpYear_From.Items.Clear();
            for (int year = (DateTime.Now.Year - 5); year < (DateTime.Now.Year + 5); year++)
            {
                ListItem newItem = null;
                newItem = new ListItem();
                newItem.Text = year.ToString();
                newItem.Value = year.ToString();
                if (year == DateTime.Now.Year)
                {
                    newItem.Selected = true;
                }
                drpYear_From.Items.Add(newItem);


                newItem = new ListItem();
                newItem.Text = year.ToString();
                newItem.Value = year.ToString();
                if (year == DateTime.Now.Year)
                {
                    newItem.Selected = true;
                }
                drpYear_To.Items.Add(newItem);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AlertRepository alertRepo = new AlertRepository();

            // Check if we're supposed to delete an alert
            if (!string.IsNullOrEmpty(Request.QueryString["DELETEALERT"]))
            {
                int parsedAlertID = Parsers.ParseInt(Request.QueryString["DELETEALERT"].ToString().Trim());

                if (parsedAlertID > 0)
                {
                    alertRepo.Delete(parsedAlertID);
                }

                Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path));   
            }


            // Populate date fields            
            if (!IsPostBack)
            {
                ResetInputFields();
            }

            // Load all alerts from the database
            List<Alert> AllAlerts = alertRepo.GetAll();
                        
            foreach (Alert alert in AllAlerts)
            {
                tblAlerts.Rows.Add(addAlertRow(alert));
            }

        }

        protected void btnSaveNewAlert_Click(object sender, EventArgs e)
        {
            // Parse the dates
            int From_Year = Parsers.ParseInt(drpYear_From.Text);
            int From_Month = Parsers.ParseInt(drpMonth_From.Text);
            int From_Day = Parsers.ParseInt(txtDay_From.Text);
            int From_Hour = Parsers.ParseInt(txtHour_From.Text); ;
            int From_Minute = Parsers.ParseInt(txtMinute_From.Text);
                        
            DateTime FromDate = new DateTime(From_Year, From_Month, From_Day, From_Hour, From_Minute, 0);
            
            int To_Year = Parsers.ParseInt(drpYear_To.Text);
            int To_Month = Parsers.ParseInt(drpMonth_To.Text);
            int To_Day = Parsers.ParseInt(txtDay_To.Text);
            int To_Hour = Parsers.ParseInt(txtHour_To.Text); ;
            int To_Minute = Parsers.ParseInt(txtMinute_To.Text);            

            DateTime ToDate = new DateTime(To_Year, To_Month, To_Day, To_Hour, To_Minute, 0);

            int importance_int = Parsers.ParseInt(drpImportance.Text);
            string alertContent = txtAlertContent.Text;

            AlertImportance importance = AlertImportance.Normal;
            if (importance_int > 0) 
            {
                importance = AlertImportance.High;
            }

            if (!string.IsNullOrEmpty(alertContent))
            {
                Alert newAlert = new Alert() {
                    Content = alertContent,
                    DisplayFrom = FromDate,
                    DisplayTo = ToDate,
                    Importance = importance
                };

                AlertRepository alertRepo = new AlertRepository();
                alertRepo.Insert(newAlert);
                Response.Redirect(Request.RawUrl);
            }

        }
    }
}