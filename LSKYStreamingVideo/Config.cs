using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYStreamingVideo
{
    public static class Config
    {
        // For determining who can access private videos
        private static string localNetworkChunk = "10.177.";
        
        public static bool CanAccessPrivate(string ipAddress)
        {
            return ipAddress.StartsWith(localNetworkChunk);
        }
    }
}