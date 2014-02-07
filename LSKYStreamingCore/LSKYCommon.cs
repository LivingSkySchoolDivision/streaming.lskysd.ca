using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace LSKYStreamingCore
{
    public static class LSKYCommon
    {
        private static Random random = new Random(DateTime.Now.Millisecond); // Not cryptographically random, but random enough for what I need it for

        // Database connection strings
        public static string dbConnectionString_ReadOnly = ConfigurationManager.ConnectionStrings["StreamingDatabaseReadOnly"].ConnectionString;

        /// <summary>
        /// Returns a new ID string for use in the database
        /// </summary>
        /// <returns></returns>
        
        const string BaseUrlChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public static string getNewID(int number_of_characters)
        {
            int maxNumber = BaseUrlChars.Length;
            List<int> numList = new List<int>();

            for (int x = 0; x < number_of_characters; x++)
            {
                numList.Add(random.Next(maxNumber));
            }

            return numList.Aggregate(string.Empty, (current, num) => current + BaseUrlChars.Substring(num, 1));        
        }

        /// <summary>
        /// Returns an MD5 hash of the specified string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getMD5(string input)
        {
            string returnMe = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public static string boolToTrueFalse(bool thisBool)
        {
            if (thisBool)
            {
                return "True";
            }
            else
            {
                return "False";
            }
        }

        public static string boolToYesOrNoHTML(bool thisBool)
        {
            if (thisBool)
            {
                return "<span style=\"color: #007700;\">Yes</span>";
            }
            else
            {
                return "<span style=\"color: #770000;\">No</span>";
            }
        }

        public static string boolToYesOrNo(bool thisBool)
        {
            if (thisBool)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        public static int boolToOneOrZero(bool thisBool)
        {
            if (thisBool)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static string removeSpaces(string working)
        {
            try
            {
                return Regex.Replace(working, @"[^\w]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return string.Empty;
            }
        }

        public static int ParseDatabaseInt(string thisDatabaseValue)
        {
            int parsedValue = 0;

            if (int.TryParse(thisDatabaseValue, out parsedValue))
            {
                return parsedValue;
            }
            else
            {
                return 0;
            }
        }

        public static DateTime ParseDatabaseDateTime(string thisDatabaseValue)
        {
            if (string.IsNullOrEmpty(thisDatabaseValue))
            {
                return DateTime.MinValue;
            }
            else
            {
                DateTime parsedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(thisDatabaseValue, out parsedDateTime))
                {
                    return parsedDateTime;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            
        }

        public static bool ParseDatabaseBool(string thisDatabaseValue, bool valueIfFailed)
        {
            if (String.IsNullOrEmpty(thisDatabaseValue))
            {
                return false;
            }
            else
            {
                bool parsedValue = valueIfFailed;

                if (bool.TryParse(thisDatabaseValue, out parsedValue))
                {
                    return parsedValue;
                }
                else
                {
                    return valueIfFailed;
                }                
            }
        }

        public static string SanitizeQueryStringID(string dirtyString)
        {
            int max_size = 10;

            StringBuilder returnMe = new StringBuilder();

            string working = string.Empty;
            if (dirtyString.Length <= max_size)
            {
                working = dirtyString;
            }
            else
            {
                working = dirtyString.Substring(0, max_size);
            }

            foreach (char c in working)
            {
                if (BaseUrlChars.Contains(c))
                {
                    returnMe.Append(c);
                }
            }

            return returnMe.ToString();

        }

        const string AllowedSearchCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz !@&-=_+:;.,";
        public static string SanitizeSearchString(string dirtyString)
        {
            int max_size = 250;

            StringBuilder returnMe = new StringBuilder();

            string working = string.Empty;
            if (dirtyString.Length <= max_size)
            {
                working = dirtyString;
            }
            else
            {
                working = dirtyString.Substring(0, max_size);
            }

            foreach (char c in working)
            {
                if (AllowedSearchCharacters.Contains(c))
                {
                    returnMe.Append(c);
                }
            }

            return returnMe.ToString();

        }
        
        const string AllowedGeneralCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ~!@#$%^&*()_+-=/?|";
        public static string SanitizeGeneralInputString(string dirtyString)
        {
            int max_size = 50000;

            StringBuilder returnMe = new StringBuilder();

            string working = string.Empty;
            if (dirtyString.Length <= max_size)
            {
                working = dirtyString;
            }
            else
            {
                working = dirtyString.Substring(0, max_size);
            }

            foreach (char c in working)
            {
                if (AllowedGeneralCharacters.Contains(c))
                {
                    returnMe.Append(c);
                }
            }

            return returnMe.ToString();

        }

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
