using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LSKYStreamingCore;

namespace LSKYStreamingVideo.CommonHTMLParts
{
    public static class VideoPlayer
    {
        public static string GetHTML(Video video)
        {
            if (!string.IsNullOrEmpty(video.YoutubeURL))
            {
                return YoutubeVideoPlayer.GetHTML(video);
            }
            else
            {
                return HTML5VideoPlayer.GetHTML(video);
            }
        }
    }
}