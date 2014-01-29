﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    public static class LSKYCommonHTMLParts
    {
        public enum Player
        {
            Silverlight,
            HTML5
        }

        /// <summary>
        /// A full video list item, with most meta information
        /// </summary>
        /// <param name="video"></param>
        /// <param name="showThumbnail"></param>
        /// <returns></returns>
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

        /// <summary>
        /// A view list item consisting of just a thumbnail and a video title
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
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

        public static string BuildErrorMessage(string message)
        {
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div class=\"large_infobox\" style=\"\">" + message + "</div> ");
            return returnMe.ToString();
        }

        public static string BuildSilverlightVideoPlayer(Video video)
        {
            if (!string.IsNullOrEmpty(video.FileURL_ISM))
            {
                int width = video.Width;
                int height = video.Height;

                string playerXapFile = "LSKYSmoothStreamPlayer_PreRecorded.xap";

                StringBuilder returnMe = new StringBuilder();
                returnMe.Append("<div style=\"border: 0px solid black; width: " + width + "px; height: " + height + "px;\">");
                returnMe.Append("<object data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" width=\"" + width + "\" height=\"" + height + "\">");
                returnMe.Append("<param name=\"source\" value=\"" + playerXapFile + "\"/>");
                returnMe.Append("<param name=\"onError\" value=\"onSilverlightError\" />");
                returnMe.Append("<param name=\"background\" value=\"white\" />");
                returnMe.Append("<param name=\"minRuntimeVersion\" value=\"4.0.50826.0\" />");
                returnMe.Append("<param name=\"autoUpgrade\" value=\"true\" />");
                returnMe.Append("<param name=\"initParams\" value=\"streamuri=/video_files/" + video.FileURL_ISM + "/Manifest,width=" + width + ",height=" + height + "\" />");
                returnMe.Append("<a href=\"http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0\" style=\"text-decoration:none\">");
                returnMe.Append("<img src=\"http://go.microsoft.com/fwlink/?LinkId=161376\" alt=\"Get Microsoft Silverlight\" style=\"border-style:none\"/>");
                returnMe.Append("</a>");
                returnMe.Append("</object>");
                returnMe.Append("</div>");
                returnMe.Append("<div style=\"width: " + video.Width + "px; margin-left: auto; margin-right: auto; font-size: 8pt; color: #444444; text-align: right;\">");

                if (video.IsHTML5Available())
                {
                    returnMe.Append("Problems viewing the stream? <a href=\"?i=" + video.ID + "&html5=true\">click here to switch to HTML5 player</a>, or <a href=\"/help/\">click here for our help page</a> ");
                }
                else
                {
                    returnMe.Append("Problems viewing the stream? <a href=\"/help/\">Click here for our help page</a> ");
                }

                returnMe.Append("</div>");
                return returnMe.ToString();
            }
            else
            {
                return BuildErrorMessage("Video not supported by this player.");
            }
        }

        public static string BuildHTML5VideoPlayerHTML(Video video)
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<video autoplay class=\"html5_player\" width=\"" + video.Width + "\" height=\"" + video.Height + "\" controls poster=\"lsky_stream_poster.png\" >");
            if (!string.IsNullOrEmpty(video.FileURL_H264))
            {
                returnMe.Append("<source src=\"/video_files/" + video.FileURL_H264 + "\" type=\"video/mp4\" />");
            }
            if (!string.IsNullOrEmpty(video.FileURL_THEORA))
            {
                returnMe.Append("<source src=\"/video_files/" + video.FileURL_THEORA + "\" type=\"video/ogg\" />");
            }
            if (!string.IsNullOrEmpty(video.FileURL_VP8))
            {
                returnMe.Append("<source src=\"/video_files/" + video.FileURL_VP8 + "\" type=\"video/webm\" />");
            }
            returnMe.Append("<em>Sorry, your browser doesn't support HTML5 video.</em>");
            returnMe.Append("</video>");

            returnMe.Append("<div style=\"width: " + video.Width + "px; margin-left: auto; margin-right: auto; font-size: 8pt; color: #444444; text-align: right;\">");
            if (video.IsSilverlightAvailable())
            {
                returnMe.Append("Problems viewing the stream? <a style=\"font-size:8pt;\" href=\"?i=" + video.ID + "&silverlight=true\">click here to switch to the Silverlight player</a>, or <a href=\"/help/\">click here for our help page</a> ");
            }
            else
            {
                returnMe.Append("Problems viewing the stream? <a href=\"/help/\">Click here for our help page</a> ");
            }
            returnMe.Append("</div>");

            return returnMe.ToString();
        }

        public static string BuildHTML5LiveStreamPlayer(Stream stream)
        {
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div style=\"width: " + stream.Width + "px; margin-left: auto; margin-right: auto;\">");
            returnMe.Append("<video class=\"html5_player\" width=\"" + stream.Width + "\" ");
            returnMe.Append("height=\"" + stream.Height + "\" ");
            returnMe.Append("src=\"/isml/" + stream.ISM_URL + "/manifest(format=m3u8-aapl).m3u8\" ");
            returnMe.Append("poster=\"lsky_stream_poster.png\" ");
            returnMe.Append("autoplay=\"true\" ");
            returnMe.Append("style=\"background-color: white;\" ");
            returnMe.Append("controls=\"true\" >Your browser does not appear to support this streaming video format</video>");
            returnMe.Append("</div>");
            returnMe.Append("<div style=\"width: " + stream.Width + "px; margin-left: auto; margin-right: auto; font-size: 8pt; color: #444444; text-align: right;\">");
            returnMe.Append("Problems viewing the stream? <a style=\"font-size:8pt;\" href=\"?i=" + stream.ID + "&silverlight=true\">click here to switch to the Silverlight player</a>, or <a href=\"/help/\">click here for our help page</a> ");
            returnMe.Append("</div>");
            return returnMe.ToString();

        }

        public static string BuildSilverlightLiveStreamPlayer(Stream stream)
        {
            int width = stream.Width;
            int height = stream.Height;

            string playerXapFile = "LSKYSmoothStreamPlayer_Live.xap";

            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<div style=\"border: 0px solid black; width: " + width + "px; height: " + height + "px;\">");
            returnMe.Append("<object data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" width=\"" + width + "\" height=\"" + height + "\">");
            returnMe.Append("<param name=\"source\" value=\"" + playerXapFile + "\"/>");
            returnMe.Append("<param name=\"onError\" value=\"onSilverlightError\" />");
            returnMe.Append("<param name=\"background\" value=\"white\" />");
            returnMe.Append("<param name=\"minRuntimeVersion\" value=\"4.0.50826.0\" />");
            returnMe.Append("<param name=\"autoUpgrade\" value=\"true\" />");
            returnMe.Append("<param name=\"initParams\" value=\"streamuri=/isml/" + stream.ISM_URL + "/Manifest,width=" + width + ",height=" + height + "\" />");
            returnMe.Append("<a href=\"http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0\" style=\"text-decoration:none\">");
            returnMe.Append("<img src=\"http://go.microsoft.com/fwlink/?LinkId=161376\" alt=\"Get Microsoft Silverlight\" style=\"border-style:none\"/>");
            returnMe.Append("</a>");
            returnMe.Append("</object>");
            returnMe.Append("</div>");
            returnMe.Append("<div style=\"width: " + stream.Width + "px; margin-left: auto; margin-right: auto; font-size: 8pt; color: #444444; text-align: right;\">");
            returnMe.Append("Problems viewing the stream? <a href=\"?i=" + stream.ID + "&html5=true\">click here to switch to HTML5 player</a>, or <a href=\"/help/\">click here for our help page</a> ");
            returnMe.Append("</div>");
            return returnMe.ToString();
        }

    }
}