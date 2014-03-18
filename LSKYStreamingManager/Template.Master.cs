using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager
{
    public partial class Template : System.Web.UI.MasterPage
    {
        public LoginSession loggedInUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            // If "Logout" or "Logoff" are in the querystring, log the current session off
            if ((Request.QueryString.AllKeys.Contains("logoff")) || (Request.QueryString.AllKeys.Contains("logout")))
            {
                if (!string.IsNullOrEmpty(LSKYStreamingManagerCommon.getSessionIDFromCookies(LSKYStreamingManagerCommon.logonCookieName, Request)))
                {
                    using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                    {
                        LoginSession.expireThisSession(connection, LSKYStreamingManagerCommon.getSessionIDFromCookies(LSKYStreamingManagerCommon.logonCookieName, Request));
                    }

                    tblLoggedInUserBanner.Visible = false;
                    redirectToLogin();
                }
            }


            lblServerTime.Text = DateTime.Now.ToLongDateString() + " - " + DateTime.Now.ToLongTimeString();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // Check the IP to make sure traffic originates from within our network
            if (
                !(
                    (Request.ServerVariables["REMOTE_ADDR"].Contains("127.0.0.1")) ||
                    (Request.ServerVariables["REMOTE_ADDR"].Contains("::1"))
                    )
                )
            {
                if (!Request.ServerVariables["REMOTE_ADDR"].StartsWith(LSKYStreamingManagerCommon.localNetworkChunk))
                {
                    Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + LSKYStreamingManagerCommon.outsideErrorMessage);
                    Response.End();
                }
            }

            // API keys are not valid for these sites anymore, so we don't need to look for one here

            // Check for an authentication cookie and see if it is valid
            string userSessionID = LSKYStreamingManagerCommon.getSessionIDFromCookies(LSKYStreamingManagerCommon.logonCookieName, Request);

            if (!string.IsNullOrEmpty(userSessionID))
            {
                using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                {
                    loggedInUser = LoginSession.loadThisSession(connection, userSessionID, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"]);
                }
            }

            // If the cookie exists, and the ID contained in it corresponds to a valid session, "loggedInUser" will not be null.
            if (loggedInUser == null)
            {
                string CurrentURL = Request.Url.AbsoluteUri;
                string LoginURL = Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + LSKYStreamingManagerCommon.loginURL;
                // If the application is running in the root, we dont need to include the application path
                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    LoginURL = Request.Url.GetLeftPart(UriPartial.Authority) + LSKYStreamingManagerCommon.loginURL;
                }
                if (!
                    (CurrentURL.ToLower().Equals(LoginURL.ToLower()))
                    )
                {
                    redirectToLogin();
                }
            }
            else
            {
                tblLoggedInUserBanner.Visible = true;
                lblLoggedInUser_Username.Text = loggedInUser.username;
                lblLoggedInUser_SessionEnds.Text = loggedInUser.ends.ToShortDateString() + " " + loggedInUser.ends.ToShortTimeString();

            }

        }

        private void invalidateLocalCookie()
        {
            if (Request.Cookies.AllKeys.Contains(LSKYStreamingManagerCommon.logonCookieName))
            {
                HttpCookie newCookie = new HttpCookie(LSKYStreamingManagerCommon.logonCookieName);
                newCookie.Value = "NOTHING TO SEE HERE";
                newCookie.Expires = DateTime.Now.AddDays(-1D);
                newCookie.Domain = Request.Url.Host;
                newCookie.Secure = true;
                Response.Cookies.Add(newCookie);
            }
        }


        /// <summary>
        /// Stops the processing of the current page, and redirects to the login page (URL is specified in a string at the top of this file)
        /// </summary>
        private void redirectToLogin()
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + LSKYStreamingManagerCommon.loginURL);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.End();
        }
    }
}