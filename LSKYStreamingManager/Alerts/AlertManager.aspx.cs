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

            if (alert.Importance == Alert.AlertImportance.High)
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
            // Check if we're supposed to delete an alert
            if (!string.IsNullOrEmpty(Request.QueryString["DELETEALERT"]))
            {
                int parsedAlertID = Helpers.ParseDatabaseInt(Request.QueryString["DELETEALERT"].ToString().Trim());

                if (parsedAlertID > 0)
                {
                    using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                    {
                        Alert.DeleteAlert(connection, parsedAlertID);
                    }
                }

                Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path));   
            }


            // Populate date fields            
            if (!IsPostBack)
            {
                ResetInputFields();
            }

            // Load all alerts from the database
            List<Alert> AllAlerts = new List<Alert>();

            using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
            {
                AllAlerts = Alert.LoadAllAlerts(connection);
            }
                        
            foreach (Alert alert in AllAlerts)
            {
                tblAlerts.Rows.Add(addAlertRow(alert));
            }

        }

        protected void btnSaveNewAlert_Click(object sender, EventArgs e)
        {
            // Parse the dates
            int From_Year = Helpers.ParseDatabaseInt(drpYear_From.Text);
            int From_Month = Helpers.ParseDatabaseInt(drpMonth_From.Text);
            int From_Day = Helpers.ParseDatabaseInt(txtDay_From.Text);
            int From_Hour = Helpers.ParseDatabaseInt(txtHour_From.Text); ;
            int From_Minute = Helpers.ParseDatabaseInt(txtMinute_From.Text);
                        
            DateTime FromDate = new DateTime(From_Year, From_Month, From_Day, From_Hour, From_Minute, 0);

            int To_Year = Helpers.ParseDatabaseInt(drpYear_To.Text);
            int To_Month = Helpers.ParseDatabaseInt(drpMonth_To.Text);
            int To_Day = Helpers.ParseDatabaseInt(txtDay_To.Text);
            int To_Hour = Helpers.ParseDatabaseInt(txtHour_To.Text); ;
            int To_Minute = Helpers.ParseDatabaseInt(txtMinute_To.Text);            

            DateTime ToDate = new DateTime(To_Year, To_Month, To_Day, To_Hour, To_Minute, 0);

            int importance_int = Helpers.ParseDatabaseInt(drpImportance.Text);
            string alertContent = txtAlertContent.Text;

            Alert.AlertImportance importance = Alert.AlertImportance.Normal;
            if (importance_int > 0) 
            {
                importance = Alert.AlertImportance.High;
            }

            if (!string.IsNullOrEmpty(alertContent))
            {
                Alert newAlert = new Alert(0, alertContent, FromDate, ToDate, importance);
                using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                {
                    Alert.InsertNewAlert(connection, newAlert);
                    Response.Redirect(Request.RawUrl);
                }
            }

        }
    }
}