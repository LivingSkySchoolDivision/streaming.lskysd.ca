using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYStreamingCore;

namespace LSKYStreamingManager
{
    public partial class GenerateIDs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            VideoRepository videoRepository = new VideoRepository();

            tblIDs.Rows.Clear();
            for (int x = 0; x <= 50; x++)
            {
                TableRow tblR = new TableRow();

                tblR.Cells.Add(new TableCell()
                {
                    Text = videoRepository.CreateNewVideoID()
                });

                tblIDs.Rows.Add(tblR);
            }
        }
    }
}