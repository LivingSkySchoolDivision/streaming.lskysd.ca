using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager
{
    public partial class GenerateIDs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < 25; x++)
            {
                litIDs.Text += LSKYCommon.getNewID(6) + "<BR>";
            }

        }
    }
}