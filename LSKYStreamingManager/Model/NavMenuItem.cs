using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSKYStreamingManager
{
    public class NavMenuItem : IComparable
    {
        public string url { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int id { get; set; }
        public string category { get; set; }

        public NavMenuItem(int id, string url, string name, string description, string category)
        {
            this.id = id;
            this.description = description;
            this.name = name;
            this.url = url;
            this.category = category;

            if (string.IsNullOrEmpty(category))
            {
                this.category = "General";
            }
        }

        public override string ToString()
        {
            return this.name;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            NavMenuItem obj2 = obj as NavMenuItem;

            if (obj2 != null)
            {
                return this.name.CompareTo(obj2.name);
            }
            else
            {
                throw new ArgumentException("Object is not a NavMenuItem");
            }
        }
        
    }
}