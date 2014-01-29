﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager
{
    public partial class index : System.Web.UI.Page
    {
        private TableRow addNavCategory(string category)
        {
            TableRow newRow = new TableRow();

            TableCell categoryCell = new TableCell();
            categoryCell.Text = "<br/><br/><div class=\"navigation_category\">" + category + "</div>";

            categoryCell.ColumnSpan = 2;

            newRow.Cells.Add(categoryCell);

            return newRow;
        }

        private TableRow addNavItem(NavMenuItem item)
        {
            TableRow newRow = new TableRow();

            TableCell nameCell = new TableCell();
            nameCell.Text = "<a href=\"" + item.url + "\" class=\"nav_link_normal\">" + item.name + "</a>";
            nameCell.CssClass = "navigation_table_name";
            newRow.Cells.Add(nameCell);

            TableCell descriptionCell = new TableCell();
            descriptionCell.Text = item.description;
            descriptionCell.CssClass = "navigation_table_description";
            newRow.Cells.Add(descriptionCell);

            return newRow;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            List<NavMenuItem> MainMenu = new List<NavMenuItem>();
            //MainMenu.Add(new NavMenuItem(0, "#", "Title", "Description", "Category"));
            MainMenu.Add(new NavMenuItem(0, "#", "Add a new video", "Adds a new video to the site", "Videos"));
            MainMenu.Add(new NavMenuItem(0, "#", "Modify a video", "Edit an existing video record", "Videos"));
            MainMenu.Add(new NavMenuItem(0, "#", "Add a new category", "Adds a category to put videos in", "Videos"));
            MainMenu.Add(new NavMenuItem(0, "#", "Schedule a broadcast", "Create a new live stream record", "Live Broadcasts"));
            MainMenu.Add(new NavMenuItem(0, "#", "Modify a broadcast", "Edit an existing live stream record", "Live Broadcasts"));
            MainMenu.Add(new NavMenuItem(0, "#", "ISML Viewer", "Test an ISML file to see if the stream is working", "Live Broadcasts"));
            MainMenu.Add(new NavMenuItem(0, "SiteAccess/SessionManager.aspx", "Current login sessions", "View current login sessions on this website", "Site Administration"));
            MainMenu.Add(new NavMenuItem(0, "SiteAccess/AccessLog.aspx", "Access log", "View login success and failure logs", "Site Administration"));


            // Get the current user
            LoginSession currentUser = null;
            using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
            {
                string userSessionID = LSKYStreamingManagerCommon.getSessionIDFromCookies(LSKYStreamingManagerCommon.logonCookieName, Request);
                currentUser = LoginSession.loadThisSession(connection, userSessionID, Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"]);
            }


            // Get a list of all categories
            List<string> MenuCategories = new List<string>();
            foreach (NavMenuItem item in MainMenu)
            {
                if (!MenuCategories.Contains(item.category))
                {
                    MenuCategories.Add(item.category);
                }
            }
            MenuCategories.Sort();

            foreach (string category in MenuCategories)
            {
                tblNavigation.Rows.Add(addNavCategory(category));
                foreach (NavMenuItem item in MainMenu)
                {
                    if (item.category == category)
                    {
                        tblNavigation.Rows.Add(addNavItem(item));
                    }
                }
            }



        }
    }
}