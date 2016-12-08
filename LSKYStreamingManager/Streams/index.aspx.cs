using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYStreamingCore.ExtensionMethods;

namespace LSKYStreamingManager.Streams
{
    public partial class index : System.Web.UI.Page
    {
        private TableRow addBroadcastTableRow(LiveBroadcast thisBroadcast, bool highlight)
        {
            TableRow returnMe = new TableRow();
            
            if (thisBroadcast.IsLive)
            {
                returnMe.CssClass += " stream_list_live";
            }

            if (thisBroadcast.IsEnded)
            {
                returnMe.CssClass += " stream_list_complete";
            }

            if (highlight)
            {
                returnMe.CssClass += " highlight_row";
            }



            TableCell Cell_View = new TableCell();
            Cell_View.Text = "<a href=\"http://streaming.lskysd.ca/live/?i=" + thisBroadcast.ID + "\" target=\"_New\">View</a>";
            returnMe.Cells.Add(Cell_View);

            TableCell Cell_Edit = new TableCell();
            Cell_Edit.Text = "<a href=\"EditStream.aspx?i=" + thisBroadcast.ID + "\">Edit</a>";
            returnMe.Cells.Add(Cell_Edit);

            TableCell Cell_Name = new TableCell();
            Cell_Name.Text = thisBroadcast.Name;
            if (thisBroadcast.IsLive)
            {
                Cell_Name.Text += " <div style=\"display: inline; font-size: 8pt; font-weight: bold; color: rgba(0,128,0,1); text-decoration: none;\">(live now)</div>";
            } 
            
            if (thisBroadcast.IsEnded)
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
            Cell_Hidden.Text = thisBroadcast.IsHidden.ToYesOrNoHTML();
            returnMe.Cells.Add(Cell_Hidden);

            TableCell Cell_Private = new TableCell();
            Cell_Private.Text = thisBroadcast.IsPrivate.ToYesOrNoHTML();
            returnMe.Cells.Add(Cell_Private);

            TableCell Cell_AlwaysOnline = new TableCell();
            Cell_AlwaysOnline.Text = thisBroadcast.ForcedLive.ToYesOrNoHTML();
            returnMe.Cells.Add(Cell_AlwaysOnline);

            return returnMe;            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LiveBroadcastRepository liveBroadcastRepository = new LiveBroadcastRepository();
                List<LiveBroadcast> AllLiveBroadcasts = liveBroadcastRepository.GetAll();

                // See if we should highlight one (that would have been just added)
                string HighLightID = string.Empty;

                if (Request.QueryString["highlight"] != null)
                {
                    HighLightID = Request.QueryString["highlight"].ToString().Trim();
                }
                
                foreach (LiveBroadcast stream in AllLiveBroadcasts.OrderByDescending(l => l.IsLive).ThenByDescending(l => l.StartTime))
                {
                    bool highlight = (HighLightID != string.Empty) && (HighLightID == stream.ID);

                    tblStreams.Rows.Add(addBroadcastTableRow(stream, highlight));
                }

            }

        }
    }
}