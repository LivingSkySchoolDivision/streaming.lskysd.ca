using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    public class Alert
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime DisplayFrom { get; set; }
        public DateTime DisplayTo { get; set; }
        public AlertImportance Importance { get; set; }

        public Alert() { }
                
    }
}
