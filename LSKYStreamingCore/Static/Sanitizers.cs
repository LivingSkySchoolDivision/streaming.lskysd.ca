using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    public static class Sanitizers
    {
        const string BaseUrlChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
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

        const string AllowedGeneralCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ~!@#$%^&*()_+-=/?|.,'\"";
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
    }
}
