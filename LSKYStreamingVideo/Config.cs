using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LSKYStreamingVideo
{
    public static class Config
    {
        // For determining who can access private videos
        private static string localNetworkChunk
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["LocalNetworkIPChunk"].ToString();
            }
        }

        // Thumbnail Path
        public static string ThumbnailPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ThumbnailPath"].ToString();
            }
        }

        // Video path
        public static string VideoPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["VideoPath"].ToString();
            }
        }

        public static bool CanAccessPrivate(string ipAddress)
        {
            return ipAddress.StartsWith(localNetworkChunk);
        }

    }
}