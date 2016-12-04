using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Strips any non-alpha or numeric characters, leaving only letters and numbers.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string inputString)
        {
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder returnMe = new StringBuilder();

            foreach (char c in inputString)
            {
                if (allowedChars.Contains(c))
                {
                    returnMe.Append(c);
                }
            }

            return returnMe.ToString();
        }
        
        public static string NoLongerThan(this string value, int width)
        {
            return value.Length > width ? value.Substring(0, width) : value;
        }
    }
}
