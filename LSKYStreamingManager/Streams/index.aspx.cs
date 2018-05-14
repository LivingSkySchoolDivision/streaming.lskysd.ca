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
            
            returnMe.Cells.Add(new TableCell() { Text = "<a href=\"http://streaming.lskysd.ca/live/?i=" + thisBroadcast.ID + "\" target=\"_New\">View</a>" });

            returnMe.Cells.Add(new TableCell() { Text = "<a href=\"EditStream.aspx?i=" + thisBroadcast.ID + "\">Edit</a>" });
            
            string StreamName = thisBroadcast.Name;

            if (thisBroadcast.IsLive)
            {
                StreamName += " <div style=\"display: inline; font-size: 8pt; font-weight: bold; color: rgba(0,128,0,1); text-decoration: none;\">(live now)</div>";
            } 
            
            if (thisBroadcast.IsEnded)
            {
                StreamName += " <div style=\"display: inline; font-size: 8pt; font-weight: bold; color: rgba(128,0,0,1); text-decoration: none;\">(completed)</div>";
            }

            returnMe.Cells.Add(new TableCell() { Text = StreamName });

            returnMe.Cells.Add(new TableCell() { Text = thisBroadcast.Location });
            returnMe.Cells.Add(new TableCell() { Text = thisBroadcast.StartTime.ToShortDateString() + " " + thisBroadcast.StartTime.ToShortTimeString() });
            returnMe.Cells.Add(new TableCell() { Text = thisBroadcast.IsHidden.ToYesOrNoHTML() });
            returnMe.Cells.Add(new TableCell() { Text = thisBroadcast.IsPrivate.ToYesOrNoHTML() });
            returnMe.Cells.Add(new TableCell() { Text = thisBroadcast.ForcedLive.ToYesOrNoHTML() });
            returnMe.Cells.Add(new TableCell() { Text = thisBroadcast.YouTubeID });

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