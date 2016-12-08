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

            returnMe.Cells.Add(new TableCell() { Text = "<a href=\"http://streaming.lskysd.ca/player/?i=" + video.ID + "\" target=\"_New\">View</a>" });
            returnMe.Cells.Add(new TableCell() { Text = "<a href=\"EditVideo.aspx?i=" + video.ID + "\">Edit</a>" });
            returnMe.Cells.Add(new TableCell() { Text = video.Name });
            returnMe.Cells.Add(new TableCell() { Text = video.CategoryName });
            returnMe.Cells.Add(new TableCell() { Text = video.Dimensions });
            returnMe.Cells.Add(new TableCell() { Text = video.IsHidden.ToYesOrNoHTML() });
            returnMe.Cells.Add(new TableCell() { Text = video.IsPrivate.ToYesOrNoHTML() });
            returnMe.Cells.Add(new TableCell() { Text = video.DateAdded.ToString() });

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