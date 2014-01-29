using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.Login
{
    public partial class index : System.Web.UI.Page
    {
        /// <summary>
        /// Creates the cookie on user's computer
        /// </summary>
        /// <param name="sessionID"></param>
        private void createCookie(string sessionID)
        {
            HttpCookie newCookie = new HttpCookie(LSKYStreamingManagerCommon.logonCookieName);
            newCookie.Value = sessionID;
            newCookie.Expires = DateTime.Now.AddHours(8);
            newCookie.Domain = LSKYStreamingManagerCommon.getServerName(Request);
            newCookie.Secure = true;
            Response.Cookies.Add(newCookie);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check to see if a user is already logged in and display an appropriate message
            LoginSession currentUser = null;
            string userSessionID = LSKYStreamingManagerCommon.getSessionIDFromCookies(LSKYStreamingManagerCommon.logonCookieName, Request);

            // Load the current user to get a listof allowed schools
            if (!string.IsNullOrEmpty(userSessionID))
            {
                using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                {
                    currentUser = LoginSession.loadThisSession(connection, userSessionID, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"]);
                }
            }

            if (currentUser != null)
            {
                tblAlreadyLoggedIn.Visible = true;
                tblLoginform.Visible = false;
                lblUsername.Text = currentUser.username;
            }
        }

        /// <summary>
        /// Displays an error message on the Login form
        /// </summary>
        /// <param name="errorMessage"></param>
        protected void displayError(string errorMessage)
        {
            tblErrorMessage.Visible = true;
            lblErrorMessage.Text = errorMessage;
        }

        /// <summary>
        /// Checks to make sure that the username and password combination given (even if valid) are complex enough
        /// </summary>
        /// <param name="thisPassword"></param>
        /// <returns></returns>
        private bool isPasswordStrongEnough(string thisPassword)
        {
            bool returnme = false;

            char[] upperCase = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] lowerCase = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] specialchar = { ' ', '.', ',', '!', '@', '#', '$', '%', '^', '&', '\\', '/', '*', '(', ')', '-', '+', '|', '?', '`', '~' };

            List<string> commonPasswords = new List<string>();
            #region Add some common passwords to the list
            // These are from various sources for the most commonly used passwords from various breaches in the last few years
            // This list could definitely be bigger, but I only have so  much patience. Maybe at some point I will have it look these up from a database table
            commonPasswords.Add("123");
            commonPasswords.Add("1234");
            commonPasswords.Add("12345");
            commonPasswords.Add("123456");
            commonPasswords.Add("1234567");
            commonPasswords.Add("12345678");
            commonPasswords.Add("123456789");
            commonPasswords.Add("password");
            commonPasswords.Add("abc123");
            commonPasswords.Add("qwerty");
            commonPasswords.Add("monkey");
            commonPasswords.Add("letmein");
            commonPasswords.Add("111111");
            commonPasswords.Add("iloveyou");
            commonPasswords.Add("trustno1");
            commonPasswords.Add("123123");
            commonPasswords.Add("password1");
            commonPasswords.Add("000000");
            commonPasswords.Add("123123123");
            commonPasswords.Add("abc");
            commonPasswords.Add(" ");
            commonPasswords.Add("  ");
            commonPasswords.Add("   ");
            commonPasswords.Add("    ");
            #endregion

            if (thisPassword.Length > 4)
            {
                if (!commonPasswords.Contains(thisPassword))
                {
                    return true;
                }
            }

            return returnme;
        }

        /// <summary>
        /// Simply redirects the user to the index page (URL specified in the LSKYCommon_District static class)
        /// </summary>
        public void redirectToIndex()
        {
            //Response.Write("Redirect to: " + LSKYStreamingManagerCommon.indexURL);
            Response.Redirect(LSKYStreamingManagerCommon.indexURL);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Do a sanity check on the username and password 
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            
            if (
                (username.Length > 3) &&
                (password.Length > 3)
                )
            {
                // Validate username and password
                if (LSKYStreamingManagerCommon.validateADCredentials("lskysd", username, password))
                {
                    // Check if the password is complex enough

                    if (isPasswordStrongEnough(password))
                    {
                        // Attempt to create a session for the user
                        string newSessionID = LoginSession.createSession(username, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"]);

                        if (newSessionID != string.Empty)
                        {
                            // Create a cookie with the user's shiny new session ID
                            createCookie(newSessionID);

                            // Redirect to the front page
                            Logging.logLoginAttempt(username, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"], "SUCCESS", "Successful login");
                            redirectToIndex();
                        }
                        else
                        {
                            displayError("<b style=\"color: red\">Access denied:</b> Your credentials worked, but your account is not authorized for access to this site.<br><br> To request access to this site, please create a ticket in our <a href=\"https://helpdesk.lskysd.ca\">Help Desk system</a>.");
                            Logging.logLoginAttempt(username, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"], "FAILURE", "Not authorized for access");
                        }
                    }
                    else
                    {
                        displayError("<b style=\"color: red\">Access denied:</b> Your password is not complex enough. Please change your password to something more complex and try again.");
                        Logging.logLoginAttempt(username, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"], "FAILURE", "Password not complex enough");
                    }
                }
                else
                {
                    displayError("<b style=\"color: red\">Access denied:</b> Invalid username or password entered");
                    Logging.logLoginAttempt(username, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"], "FAILURE", "Invalid username or password");
                }
            }
            else
            {
                displayError("<b style=\"color: red\">Access denied:</b> Invalid username or password entered");
                // Don't bother logging this
            }
        }
    }    
}