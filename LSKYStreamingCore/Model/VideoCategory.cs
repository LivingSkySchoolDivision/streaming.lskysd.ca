using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LSKYStreamingCore
{
    public class VideoCategory
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ParentCategoryID { get; set; }
        public bool IsHidden { get; set; }
        public bool IsPrivate { get; set; }
        public List<VideoCategory> Children { get; set; }
        public VideoCategory ParentCategory { get; set; }

        public bool HasChildren
        {
            get
            {
                return Children.Count > 0;
            }
        }
        public bool HasParent
        {
            get
            {
                return string.IsNullOrEmpty(ParentCategoryID);
            }
        }

        public VideoCategory()
        {
            this.Children = new List<VideoCategory>();
            this.ParentCategory = null;
        }

        public string FullName
        {
            get
            {
                // Use recursion to get all parent full names too, regardless of how many layers deep this category is
                if (this.HasParent)
                {
                    return this.ParentCategory.FullName + "/" + this.Name;
                } else
                {
                    return this.Name;
                }
            }
        }
        
    }
}
