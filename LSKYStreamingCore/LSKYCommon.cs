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
        public static string dbConnectionString_ReadWrite = ConfigurationManager.ConnectionStrings["StreamingDatabaseReadWrite"].ConnectionString;

        /// <summary>
        /// Returns a new ID string for use in the database
        /// </summary>
        /// <returns></returns>
        
        const string BaseUrlChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public static string getNewID(int characters)
        {
            int maxNumber = BaseUrlChars.Length;
            List<int> numList = new List<int>();

            for (int x = 0; x < characters; x++)
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
        
        
    }
}
