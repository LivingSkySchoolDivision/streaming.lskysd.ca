using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore.ExtensionMethods
{
    public static class IntExtensionMethods
    {

        public static bool IsOdd(this int value)
        {
            return value %2 != 0;
        }
    }
}
