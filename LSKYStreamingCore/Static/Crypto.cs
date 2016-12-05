using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    class Crypto
    {
        public static Random random = new Random(DateTime.Now.Millisecond); // Not cryptographically random, but random enough for what I need it for
        /// <summary>
        /// Returns an MD5 hash of the specified string
        /// </summary>
        /// <param name="input"></param>
        ///     <returns></returns>
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

        const string BaseUrlChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public static string GenerateID(int number_of_characters)
        {
            int maxNumber = BaseUrlChars.Length;
            List<int> numList = new List<int>();

            for (int x = 0; x < number_of_characters; x++)
            {
                numList.Add(Crypto.random.Next(maxNumber));
            }

            return numList.Aggregate(string.Empty, (current, num) => current + BaseUrlChars.Substring(num, 1));
        }


    }
}
