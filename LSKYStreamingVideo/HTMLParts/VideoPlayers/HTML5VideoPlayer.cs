using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using LSKYStreamingCore;

namespace LSKYStreamingVideo.CommonHTMLParts
{
    public static class HTML5VideoPlayer
    {
        public static string GetHTML(Video video)
        {
            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<video autoplay class=\"html5_player\" width=\"" + video.Width + "\" height=\"" + video.Height + "\" controls poster=\"lsky_stream_poster.png\" >");
            if (!string.IsNullOrEmpty(video.FileURL_H264))
            {
                returnMe.Append("<source src=\"" + Config.VideoPath + video.FileURL_H264 + "\" type=\"video/mp4\" />");
            }
            if (!string.IsNullOrEmpty(video.FileURL_THEORA))
            {
                returnMe.Append("<source src=\"" + Config.VideoPath + video.FileURL_THEORA + "\" type=\"video/ogg\" />");
            }
            if (!string.IsNullOrEmpty(video.FileURL_VP8))
            {
                returnMe.Append("<source src=\"" + Config.VideoPath + video.FileURL_VP8 + "\" type=\"video/webm\" />");
            }
            returnMe.Append("<em>Sorry, your browser doesn't support HTML5 video.</em>");
            returnMe.Append("</video>");
            returnMe.Append("<div style=\"font-size: 8pt; text-align: right; width: " + video.Width + "px;\">Problems viewing this video? <a href=\"/help/\">Click here for our help page</a></div>");
            return returnMe.ToString();
        }
    }
}