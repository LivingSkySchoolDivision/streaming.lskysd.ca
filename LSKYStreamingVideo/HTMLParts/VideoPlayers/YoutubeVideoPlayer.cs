using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using LSKYStreamingCore;

namespace LSKYStreamingVideo.CommonHTMLParts
{
    public static class YoutubeVideoPlayer
    {
        public static string GetHTML(Video video)
        {
            StringBuilder returnMe = new StringBuilder();
            
            string srcQueryString = "?";
            if (video.YoutubeStartTimeInSeconds > 0)
            {
                srcQueryString += "start=" + video.YoutubeStartTimeInSeconds + "&";
            }
            srcQueryString += "autoplay=1";

            returnMe.Append("<iframe src=\"https://www.youtube.com/embed/" + video.YoutubeURL + srcQueryString + "\"" + " frameborder=\"0\" style=\"border: 0px solid black; width: " + video.Width + "px; height: " + video.Height + "px;\" allowfullscreen>");
            returnMe.Append("<object data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" width=\"" + video.Width + "\" height=\"" + video.Height + "\">");
            returnMe.Append("</iframe>");
            return returnMe.ToString();
        }
    }
}