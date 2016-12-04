using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingVideo
{
    public partial class Template : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblServerTime.Text = DateTime.Now.ToLongDateString() + " - " + DateTime.Now.ToLongTimeString() + " (" + Request.ServerVariables["REMOTE_ADDR"] + ")";

            // Load any alerts from the database
            List<Alert> ActiveAlerts = new List<Alert>();
            using (SqlConnection connection = new SqlConnection(Helpers.dbConnectionString_ReadOnly))
            {
                ActiveAlerts = Alert.LoadActiveAlerts(connection);
            }
            
            if (ActiveAlerts.Count > 0)
            {
                StringBuilder AlertBarContent = new StringBuilder();

                foreach (Alert alert in ActiveAlerts)
                {
                    if (alert.Importance == Alert.AlertImportance.High)
                    {
                        AlertBarContent.Append("<div class=\"alertbar_high\">" + alert.Content + "</div>");
                    }
                    else
                    {
                        AlertBarContent.Append("<div class=\"alertbar_normal\">" + alert.Content + "</div>");
                    }
                }

                litAlertContainer.Text = AlertBarContent.ToString();
                
            }

        }
    }
}