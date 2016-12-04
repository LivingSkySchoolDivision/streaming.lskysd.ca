using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace LSKYStreamingCore
{
    public static class Helpers
    {
        public static Random random = new Random(DateTime.Now.Millisecond); // Not cryptographically random, but random enough for what I need it for
               
        /// <summary>
        /// Returns a new ID string for use in the database
        /// </summary>
        /// <returns></returns>
        
        

        

        

        
        
        

        public static string GenerateBrowserCompatibilityChart(Video video)
        {
            bool CompatibleWithIE = false;
            bool CompatibleWithFireFox = false;
            bool CompatibleWithChrome = false;
            bool CompatibleWithOperaPC = false;
            bool CompatibleWithOperaAndroid = false;
            bool CompatibleWithAndroidBrowser = false;
            bool CompatibleWithSafari = false;
            bool CompatibleWithSafariIOS = false;

            if (!string.IsNullOrEmpty(video.FileURL_ISM))
            {
                CompatibleWithIE = true;
            }

            if (!string.IsNullOrEmpty(video.FileURL_H264))
            {
                CompatibleWithIE = true;
                CompatibleWithFireFox = true;
                CompatibleWithChrome = true;
                CompatibleWithOperaAndroid = true;
                CompatibleWithSafari = true;
                CompatibleWithSafariIOS = true;
                CompatibleWithAndroidBrowser = true;
            }

            if (!string.IsNullOrEmpty(video.FileURL_THEORA))
            {
                CompatibleWithAndroidBrowser = true;
                CompatibleWithChrome = true;
                CompatibleWithFireFox = true;
                CompatibleWithOperaPC = true;                
            }

            if (!string.IsNullOrEmpty(video.FileURL_VP8))
            {
                CompatibleWithAndroidBrowser = true;
                CompatibleWithChrome = true;
                CompatibleWithFireFox = true;
                CompatibleWithOperaPC = true;
                CompatibleWithOperaAndroid = true;                 
            }

            StringBuilder returnMe = new StringBuilder();

            returnMe.Append("<table border=0 cellspacing=0 cellpadding=0 style=\"width: 200px;\">");

            returnMe.Append("<tr>");
            returnMe.Append("<td>Android Browser</td>");
            if (CompatibleWithAndroidBrowser)
            {
                returnMe.Append("<td><div style=\"color: #009900;\">Yes</div></td>");
            }
            else
            {
                returnMe.Append("<td><div style=\"color: #990000;\">No</div></td>");
            }
            returnMe.Append("</tr>");

            returnMe.Append("<tr>");
            returnMe.Append("<td>Google Chrome</td>");
            if (CompatibleWithChrome)
            {
                returnMe.Append("<td><div style=\"color: #009900;\">Yes</div></td>");
            }
            else
            {
                returnMe.Append("<td><div style=\"color: #990000;\">No</div></td>");
            }
            returnMe.Append("</tr>");

            returnMe.Append("<tr>");
            returnMe.Append("<td>Internet Explorer</td>");
            if (CompatibleWithIE)
            {
                returnMe.Append("<td><div style=\"color: #009900;\">Yes</div></td>");
            }
            else
            {
                returnMe.Append("<td><div style=\"color: #990000;\">No</div></td>");
            }
            returnMe.Append("</tr>");

            returnMe.Append("<tr>");
            returnMe.Append("<td>Mozilla Firefox</td>");
            if (CompatibleWithFireFox)
            {
                returnMe.Append("<td><div style=\"color: #009900;\">Yes</div></td>");
            }
            else
            {
                returnMe.Append("<td><div style=\"color: #990000;\">No</div></td>");
            }
            returnMe.Append("</tr>");

            returnMe.Append("<tr>");
            returnMe.Append("<td>Opera (PC)</td>");
            if (CompatibleWithOperaPC)
            {
                returnMe.Append("<td><div style=\"color: #009900;\">Yes</div></td>");
            }
            else
            {
                returnMe.Append("<td><div style=\"color: #990000;\">No</div></td>");
            }
            returnMe.Append("</tr>");

            returnMe.Append("<tr>");
            returnMe.Append("<td>Opera (Android)</td>");
            if (CompatibleWithOperaAndroid)
            {
                returnMe.Append("<td><div style=\"color: #009900;\">Yes</div></td>");
            }
            else
            {
                returnMe.Append("<td><div style=\"color: #990000;\">No</div></td>");
            }
            returnMe.Append("</tr>");

            returnMe.Append("<tr>");
            returnMe.Append("<td>Safari (MacOS)</td>");
            if (CompatibleWithSafari)
            {
                returnMe.Append("<td><div style=\"color: #009900;\">Yes</div></td>");
            }
            else
            {
                returnMe.Append("<td><div style=\"color: #990000;\">No</div></td>");
            }
            returnMe.Append("</tr>");

            returnMe.Append("<tr>");
            returnMe.Append("<td>Safari (iOS)</td>");
            if (CompatibleWithSafariIOS)
            {
                returnMe.Append("<td><div style=\"color: #009900;\">Yes</div></td>");
            }
            else
            {
                returnMe.Append("<td><div style=\"color: #990000;\">No</div></td>");
            }
            returnMe.Append("</tr>");                
            
            returnMe.Append("</table>");

            return returnMe.ToString();
        }

        

        

    }
}
