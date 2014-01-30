using LSKYStreamingCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYStreamingManager.Streams
{
    public partial class ISMLViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // populate dropdown box: players
                drpPlayers.Items.Add(new ListItem("Silverlight Player", "silverlight"));
                drpPlayers.Items.Add(new ListItem("HTML5 Player", "html5"));

                // populate dropdown box: ISML files
                DirectoryInfo di = new DirectoryInfo(Server.MapPath("/isml"));

                FileInfo[] files_in_isml_dir = di.GetFiles("*.isml");

                foreach (FileInfo file in files_in_isml_dir)
                {
                    drpISMLFiles.Items.Add(new ListItem(file.Name, file.Name));
                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            // Parse selected player
            if (!string.IsNullOrEmpty(drpISMLFiles.SelectedValue))
            {
                string fileName = LSKYCommon.SanitizeSearchString(drpISMLFiles.SelectedValue);

                LSKYStreamingCore.LSKYCommonHTMLParts.Player selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.HTML5;

                if (!string.IsNullOrEmpty(drpPlayers.SelectedValue))
                {
                    if (drpPlayers.SelectedValue.ToLower() == "html5")
                    {
                        selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.HTML5;
                    }

                    if (drpPlayers.SelectedValue.ToLower() == "silverlight")
                    {
                        selectedPlayer = LSKYStreamingCore.LSKYCommonHTMLParts.Player.Silverlight;
                    }

                    LiveBroadcast testStream = new LiveBroadcast("TEST","Test Stream: " + fileName, "Test", string.Empty, string.Empty, string.Empty, 720,480,fileName, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1), false, false, false, false, true, string.Empty);

                    litPlayer.Text = LSKYCommonHTMLParts.BuildLiveStreamPlayerHTML(testStream, selectedPlayer, false);

                }
            }


            // Parse selected ISML file
        }
    }
}