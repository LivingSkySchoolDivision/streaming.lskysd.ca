using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYStreamingCore.ExtensionMethods;

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
            Cell_Category.Text = video.CategoryName;
            returnMe.Cells.Add(Cell_Category);

            TableCell Cell_Dimensions = new TableCell();
            Cell_Dimensions.Text = video.Width + "x" + video.Height;
            returnMe.Cells.Add(Cell_Dimensions);

            TableCell Cell_Formats = new TableCell();
            Cell_Formats.Text = "()";
            returnMe.Cells.Add(Cell_Formats);

            TableCell Cell_Hidden = new TableCell();
            Cell_Hidden.Text = video.IsHidden.ToYesOrNoHTML();
            returnMe.Cells.Add(Cell_Hidden);

            TableCell Cell_Private = new TableCell();
            Cell_Private.Text = video.IsPrivate.ToYesOrNoHTML();
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
                VideoRepository videoRepository = new VideoRepository();
                List<Video> AllVideos = videoRepository.GetAll();

                foreach (Video video in AllVideos)
                {
                    tblVideos.Rows.Add(addVideoTableRow(video));
                }

            }

        }
    }
}