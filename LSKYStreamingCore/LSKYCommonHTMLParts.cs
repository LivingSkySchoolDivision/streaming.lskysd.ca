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
            string playerURL = "/player/?i=" + video.ID;

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
                returnMe.Append("<div style=\"width: 200px;\">");
                returnMe.Append("<img src=\"/thumbnails/small/" + thumbnailURL + "\" class=\"video_thumbnail_list_item_container_image\">");
                returnMe.Append("</a></div>");
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


        public static string VideoThumbnailListItem(Video video)
        {
            StringBuilder returnMe = new StringBuilder();

            string thumbnailURL = "none.png";
            string playerURL = "/player/?i=" + video.ID;

            if (!string.IsNullOrEmpty(video.ThumbnailURL))
            {
                thumbnailURL = video.ThumbnailURL;
            }

            returnMe.Append("<div class=\"video_thumbnail_list_item_container\">");
            returnMe.Append("<a href=\"" + playerURL + "\">");
            returnMe.Append("<img src=\"/thumbnails/small/" + thumbnailURL + "\" class=\"video_thumbnail_list_item_container_image\">");
            returnMe.Append("<div class=\"video_thumbnail_list_item_container_link\" style=\"text-decoration: none;\">" + video.Name + "</div>");
            returnMe.Append("</a>");
            if (!string.IsNullOrEmpty(video.Author))
            {
                returnMe.Append("<div class=\"video_thumbnail_list_item_container_info\">by " + video.Author + "</div>");
            }
            returnMe.Append("</div>");

            /*
            returnMe.Append("<td valign=\"top\"><div class=\"video_list_info_container\">");
            returnMe.Append("<a style=\"text-decoration: none;\" href=\"" + playerURL + "\"><div class=\"video_list_name\">" + video.Name + "</div></a>");
            returnMe.Append("<div class=\"video_list_info\"><b>Duration:</b> " + video.GetDurationInEnglish() + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Submitted by:</b> " + video.Author + "</div>");
            returnMe.Append("<div class=\"video_list_info\"><b>Recorded at:</b> " + video.Location + "</div>");
            */

            return returnMe.ToString();

        }


    }
}
