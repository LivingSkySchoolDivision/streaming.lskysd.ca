using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.Videos
{
    public partial class index : System.Web.UI.Page
    {
        private TableRow addVideoTableRow(Video video)
        {
            TableRow returnMe = new TableRow();

            TableCell Cell_View = new TableCell();
            Cell_View.Text = "<a href=\"http://streaming.lskysd.ca/player/?i=" + video.ID + "\" target=\"_New\">View</a>";            
            returnMe.Cells.Add(Cell_View);

            TableCell Cell_Edit = new TableCell();
            Cell_Edit.Text = "<a href=\"EditVideo.aspx?i=" + video.ID + "\">Edit</a>";
            returnMe.Cells.Add(Cell_Edit);

            TableCell Cell_Name = new TableCell();
            Cell_Name.Text = video.Name;
            returnMe.Cells.Add(Cell_Name);

            TableCell Cell_Category = new TableCell();
            Cell_Category.Text = video.GetFullCategory();
            returnMe.Cells.Add(Cell_Category);

            TableCell Cell_Dimensions = new TableCell();
            Cell_Dimensions.Text = video.Width + "x" + video.Height;
            returnMe.Cells.Add(Cell_Dimensions);

            TableCell Cell_Formats = new TableCell();
            Cell_Formats.Text = "()";
            returnMe.Cells.Add(Cell_Formats);

            TableCell Cell_Hidden = new TableCell();
            Cell_Hidden.Text = LSKYCommon.boolToYesOrNoHTML(video.IsHidden);
            returnMe.Cells.Add(Cell_Hidden);

            TableCell Cell_Private = new TableCell();
            Cell_Private.Text = LSKYCommon.boolToYesOrNoHTML(video.IsPrivate);
            returnMe.Cells.Add(Cell_Private);

            TableCell Cell_Expires = new TableCell();
            if (video.Expires)
            {
                Cell_Expires.Text = video.DateExpires.ToShortDateString();
            }
            else
            {
                Cell_Expires.Text = "<i>No</i>";
            }
            returnMe.Cells.Add(Cell_Expires);

            return returnMe;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<Video> AllVideos = new List<Video>();

                // See if we should highlight one (that would have been just added
                string HighLightID = string.Empty;

                if (Request.QueryString["highlight"] != null)
                {
                    HighLightID = Request.QueryString["highlight"].ToString().Trim();
                }

                // Load the most recent 200 streams
                using (SqlConnection connection = new SqlConnection(LSKYStreamingManagerCommon.dbConnectionString_ReadWrite))
                {

                    AllVideos = Video.LoadAll(connection).OrderByDescending(c => c.DateAdded).ToList<Video>();
                }

                foreach (Video video in AllVideos)
                {
                    tblVideos.Rows.Add(addVideoTableRow(video));
                }

            }

        }
    }
}