using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    public static class LSKYCommonHTMLParts
    {
        public static string SmallVideoListItem(Video video, bool showThumbnail)
        {
            StringBuilder returnMe = new StringBuilder();

            string thumbnailURL = "none.png";
            string playerURL = "player/?i=" + video.ID;

            if (!string.IsNullOrEmpty(video.ThumbnailURL))
            {
                thumbnailURL = video.ThumbnailURL;
            }

            returnMe.Append("<table border=0 cellpadding=0 cellspacing=0 style=\"width: 100%\">");
            returnMe.Append("<tr>");

            if (showThumbnail)
            {
                returnMe.Append("<td valign=\"top\" width=\"128\">");
                returnMe.Append("<a href=\"" + playerURL + "\">");
                returnMe.Append("<div style=\"width: 128px; text-align: right; height: 128px; background-color: white; background-image: url(/thumbnails/small/" + thumbnailURL + ");background-size: 128px 128px; background-repeat: no-repeat;\"></div>");
                returnMe.Append("</a>");
                returnMe.Append("</td>");
            }
            returnMe.Append("<td valign=\"top\"><div class=\"video_list_info_container\">");
            returnMe.Append("<a style=\"text-decoration: none;\" href=\"" + playerURL + "\"><div class=\"video_list_name\">" + video.Name + "</div></a>");
            returnMe.Append("<div class=\"video_list_info\"><b>Duration:</b> " + video.GetDurationInEnglish() + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Submitted by:</b> " + video.Author + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Recorded at:</b> " + video.Location + "</div>");

            if (video.ShouldDisplayAirDate)
            {
                returnMe.Append("<div class=\"video_list_info\"><b>Original broadcast:</b> " + video.DateAired.ToLongDateString() + "</div>");
            }

            if (!string.IsNullOrEmpty(video.DownloadURL))
            {
                returnMe.Append("<div class=\"video_list_info\">Download available</div>");
            }
            returnMe.Append("<br/><div class=\"video_list_description\">" + video.DescriptionSmall + "</div>");

            returnMe.Append("</div></td>");


            returnMe.Append("</tr>");
            returnMe.Append("</table><br/>");

            return returnMe.ToString();

        }


    }
}
