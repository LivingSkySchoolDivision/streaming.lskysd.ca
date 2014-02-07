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
        private TableRow addBroadcastTableRow(LiveBroadcast thisBroadcast)
        {
            TableRow returnMe = new TableRow();

            TableCell Cell_Edit = new TableCell();
            Cell_Edit.Text = "<a href=\"EditStream.aspx?i=" + thisBroadcast.ID + "\">Edit</a>";
            returnMe.Cells.Add(Cell_Edit);

            TableCell Cell_Name = new TableCell();
            Cell_Name.Text = thisBroadcast.Name;
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

                using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                {
                    AllLiveBroadcasts = LiveBroadcast.LoadAll(connection);
                }
                                
                foreach (LiveBroadcast stream in AllLiveBroadcasts)
                {
                    tblStreams.Rows.Add(addBroadcastTableRow(stream));
                }

            }

        }
    }
}