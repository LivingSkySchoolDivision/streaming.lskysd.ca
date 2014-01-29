using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.SiteAccess
{
    public partial class AccessLog : System.Web.UI.Page
    {
        private const int recordsToDisplay = 200;

        private TableRow addLoginAttemptRowWithType(LoginAttempt thisLoginAttempt)
        {
            System.Drawing.Color bgColor = System.Drawing.Color.LightGray;
            if (thisLoginAttempt.status.ToLower().Equals("success"))
            {
                bgColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                bgColor = System.Drawing.Color.LightSalmon;
            }


            TableRow returnMe = new TableRow();
            returnMe.CssClass = "datatable_row";

            TableCell cell_type = new TableCell();
            cell_type.BackColor = bgColor;
            TableCell cell_time = new TableCell();
            cell_time.BackColor = bgColor;
            TableCell cell_username = new TableCell();
            cell_username.BackColor = bgColor;
            TableCell cell_ip = new TableCell();
            cell_ip.BackColor = bgColor;
            TableCell cell_info = new TableCell();
            cell_info.BackColor = bgColor;

            TableCell cell_UserAgent = new TableCell();
            cell_UserAgent.BackColor = bgColor;



            cell_type.Text = thisLoginAttempt.status;
            cell_time.Text = thisLoginAttempt.eventTime.ToShortDateString() + " " + thisLoginAttempt.eventTime.ToLongTimeString();
            cell_username.Text = thisLoginAttempt.enteredUserName;
            cell_ip.Text = thisLoginAttempt.ipAddress;
            cell_info.Text = thisLoginAttempt.info;
            cell_UserAgent.Text = thisLoginAttempt.userAgent;

            returnMe.Cells.Add(cell_type);
            returnMe.Cells.Add(cell_time);
            returnMe.Cells.Add(cell_username);
            returnMe.Cells.Add(cell_ip);
            returnMe.Cells.Add(cell_info);
            returnMe.Cells.Add(cell_UserAgent);

            return returnMe;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<LoginAttempt> loginAttempts = new List<LoginAttempt>();

            using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
            {
                loginAttempts = LoginAttempt.getRecentLoginEvents(connection, DateTime.Now.AddMonths(-1), DateTime.Now, recordsToDisplay);
            }

            foreach (LoginAttempt la in loginAttempts)
            {
                tblLogins_All.Rows.Add(addLoginAttemptRowWithType(la));
            }
        }
    }
}