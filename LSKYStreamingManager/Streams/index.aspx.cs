using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.Streams
{
    public partial class index : System.Web.UI.Page
    {
        private TableRow addBroadcastTableRow(LiveBroadcast thisBroadcast, bool highlight)
        {
            TableRow returnMe = new TableRow();

            bool isLive = thisBroadcast.IsLive();
            bool isComplete = thisBroadcast.IsComplete();

            if (isLive)
            {
                returnMe.CssClass += " stream_list_live";
            }

            if (isComplete)
            {
                returnMe.CssClass += " stream_list_complete";
            }

            if (highlight)
            {
                returnMe.CssClass += " highlight_row";
            }



            TableCell Cell_View = new TableCell();
            if (isLive)
            {
                Cell_View.Text = "<a href=\"http://streaming.lskysd.ca/live/?i=" + thisBroadcast.ID + "\" target=\"_New\">View</a>";
            }
            else
            {
                Cell_View.Text = "";
            }
            returnMe.Cells.Add(Cell_View);

            TableCell Cell_Edit = new TableCell();
            Cell_Edit.Text = "<a href=\"EditStream.aspx?i=" + thisBroadcast.ID + "\">Edit</a>";
            returnMe.Cells.Add(Cell_Edit);

            TableCell Cell_Name = new TableCell();
            Cell_Name.Text = thisBroadcast.Name;
            if (isLive)
            {
                Cell_Name.Text += " <div style=\"display: inline; font-size: 8pt; font-weight: bold; color: rgba(0,128,0,1); text-decoration: none;\">(live now)</div>";
            } 
            
            if (isComplete)
            {
                Cell_Name.Text += " <div style=\"display: inline; font-size: 8pt; font-weight: bold; color: rgba(128,0,0,1); text-decoration: none;\">(completed)</div>";
            }
            returnMe.Cells.Add(Cell_Name);

            TableCell Cell_Location = new TableCell();
            Cell_Location.Text = thisBroadcast.Location;
            returnMe.Cells.Add(Cell_Location);

            TableCell Cell_Start = new TableCell();
            Cell_Start.Text = thisBroadcast.StartTime.ToShortDateString() + " " + thisBroadcast.StartTime.ToShortTimeString();
            returnMe.Cells.Add(Cell_Start);
            
            TableCell Cell_Hidden = new TableCell();
            Cell_Hidden.Text = LSKYCommon.boolToYesOrNoHTML(thisBroadcast.IsHidden);
            returnMe.Cells.Add(Cell_Hidden);

            TableCell Cell_Private = new TableCell();
            Cell_Private.Text = LSKYCommon.boolToYesOrNoHTML(thisBroadcast.IsPrivate);
            returnMe.Cells.Add(Cell_Private);

            TableCell Cell_AlwaysOnline = new TableCell();
            Cell_AlwaysOnline.Text = LSKYCommon.boolToYesOrNoHTML(thisBroadcast.ForcedLive);
            returnMe.Cells.Add(Cell_AlwaysOnline);

            return returnMe;            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<LiveBroadcast> AllLiveBroadcasts = new List<LiveBroadcast>();

                // See if we should highlight one (that would have been just added
                string HighLightID = string.Empty;

                if (Request.QueryString["highlight"] != null)
                {
                    HighLightID = Request.QueryString["highlight"].ToString().Trim();
                }
                
                // Load the most recent 200 streams
                using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                {
                    AllLiveBroadcasts = LiveBroadcast.LoadAll(connection, 200);
                }
                                
                foreach (LiveBroadcast stream in AllLiveBroadcasts)
                {
                    bool highlight = false;
                    if ((HighLightID != string.Empty) && (HighLightID == stream.ID))
                    {
                        highlight = true;
                    }

                    tblStreams.Rows.Add(addBroadcastTableRow(stream, highlight));
                }

            }

        }
    }
}