using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore.ExtensionMethods
{
    public static class ListExtensionMethods
    {/// <summary>
     /// Converts a list of integers to a comma seperated string, for displaying somewhere to the user
     /// </summary>
     /// <param name="list"></param>
     /// <returns></returns>
        public static string ToCommaSeparatedString(this List<int> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (int item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }

        public static string ToSpaceSeparatedString(this List<int> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (int item in list)
            {
                returnMe.Append(item);
                returnMe.Append(" ");
            }

            if (returnMe.Length > 1)
            {
                returnMe.Remove(returnMe.Length - 1, 1);
            }

            return returnMe.ToString();
        }

        public static string ToCommaSeparatedString(this List<string> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (string item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }
        public static string ToSpaceSeparatedString(this List<string> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (string item in list)
            {
                returnMe.Append(item);
                returnMe.Append(" ");
            }

            if (returnMe.Length > 1)
            {
                returnMe.Remove(returnMe.Length - 1, 1);
            }

            return returnMe.ToString();
        }

        public static string ToSemicolenSeparatedString(this List<string> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (string item in list)
            {
                returnMe.Append(item);
                returnMe.Append(";");
            }

            if (returnMe.Length > 1)
            {
                returnMe.Remove(returnMe.Length - 1, 1);
            }

            return returnMe.ToString();
        }

        public static string ToSemicolenSeparatedString(this List<int> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (int item in list)
            {
                returnMe.Append(item);
                returnMe.Append(";");
            }

            if (returnMe.Length > 1)
            {
                returnMe.Remove(returnMe.Length - 1, 1);
            }

            return returnMe.ToString();
        }

        public static void AddRangeUnique<T>(this List<T> thisList, List<T> collection)
        {
            foreach (T potentialNewItem in collection.Where(potentialNewItem => !thisList.Contains(potentialNewItem)))
            {
                thisList.Add(potentialNewItem);
            }
        }

        public static void AddUnique<T>(this List<T> thisList, T obj)
        {
            if (!thisList.Contains(obj))
            {
                thisList.Add(obj);
            }
        }

        public static string ToCommaSeparatedString<T>(this List<T> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (T item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }


    }
}
