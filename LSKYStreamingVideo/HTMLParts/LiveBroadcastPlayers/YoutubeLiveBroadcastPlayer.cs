using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using LSKYStreamingCore;

namespace LSKYStreamingVideo.CommonHTMLParts
{
    public static class YoutubeLiveBroadcastPlayer
    {
        public  static string GetHTML(LiveBroadcast stream)
        {
            StringBuilder returnMe = new StringBuilder();
            returnMe.Append("<iframe src=\"https://www.youtube.com/embed/" + stream.YouTubeID + "?autoplay=1\"" + " frameborder=\"0\" style=\"border: 0px solid black; width: " + stream.Width + "px; height: " + stream.Height + "px;\" allowfullscreen>");
            returnMe.Append("<object data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" width=\"" + stream.Width + "\" height=\"" + stream.Height + "\">");
            returnMe.Append("</iframe>");
            return returnMe.ToString();
        }
    }
}