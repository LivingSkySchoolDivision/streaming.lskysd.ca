using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.SiteAccess
{
    public partial class SessionManager : System.Web.UI.Page
    {
        /// <summary>
        /// Returns a table row for the given session, for inserting into a table in a loop
        /// </summary>
        /// <param name="session"></param>
        /// <param name="isCurrentUser"></param>
        /// <returns></returns>
        private TableRow AddTableRow_Sessions(LoginSession session, bool isCurrentUser, bool showExpireLinks)
        {
            TableRow returnMe = new TableRow();
            returnMe.CssClass = "datatable";

            TableCell cell_username = new TableCell();
            TableCell cell_starttime = new TableCell();
            TableCell cell_endtime = new TableCell();
            TableCell cell_IP = new TableCell();

            cell_username.Text = "<abbr title=\"" + session.BrowserUserAgent + "\">" + session.Username + "</abbr>";
            cell_starttime.Text = session.SessionStarted.ToLongDateString() + " " + session.SessionStarted.ToLongTimeString();
            cell_endtime.Text = session.SessionExpires.ToLongDateString() + " " + session.SessionExpires.ToLongTimeString();
            cell_IP.Text = session.IPAddress;

            if (isCurrentUser)
            {
                cell_username.Text += " (You)";
                cell_username.Font.Bold = true;
                cell_starttime.Font.Bold = true;
                cell_endtime.Font.Bold = true;
                cell_IP.Font.Bold = true;
            }

            returnMe.Cells.Add(cell_username);
            returnMe.Cells.Add(cell_starttime);
            returnMe.Cells.Add(cell_endtime);
            returnMe.Cells.Add(cell_IP);

            if ((showExpireLinks) && (!isCurrentUser))
            {
                TableCell cell_ExpireLink = new TableCell();
                cell_ExpireLink.Text = "<a href=\"?expiresession=" + session.Thumbprint + "\">Disconnect</a>";
                cell_ExpireLink.HorizontalAlign = HorizontalAlign.Center;
                returnMe.Cells.Add(cell_ExpireLink);
            }

            return returnMe;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // Load all active sessions
            LoginSessionRepository loginRepository = new LoginSessionRepository();
            string userSessionID = Settings.getSessionIDFromCookies(Settings.logonCookieName, Request);
            LoginSession currentUser = loginRepository.Get(userSessionID, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"]);
            List<LoginSession> AllSessions = loginRepository.GetActive();

            if (!string.IsNullOrEmpty(Request.QueryString["expiresession"]))
            {
                string hashToExpire = Sanitizers.SanitizeSearchString(Request.QueryString["expiresession"]);
                if (!string.IsNullOrEmpty(hashToExpire))
                {
                    loginRepository.Delete(hashToExpire);
                }
            }
            
            // Some of the following code won't work if the currentUser object is null. Ideally this shouldn't 
            // happen because the template should catch this before this page loads, but it's better to be safe
            if (currentUser != null)
            {
                // Display them in a table
                List<LoginSession> AllSessionsSorted = AllSessions.OrderBy(c => c.Username).ToList<LoginSession>();

                foreach (LoginSession session in AllSessionsSorted)
                {
                    // Determine if this session is the current user
                    bool isCurrentUser = currentUser.Thumbprint == session.Thumbprint;

                    tsblSessions.Rows.Add(AddTableRow_Sessions(session, isCurrentUser, true));                    

                }
            }

        }
    }
}