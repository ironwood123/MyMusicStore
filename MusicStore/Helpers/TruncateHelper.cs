using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helpers
{
    public static class TruncateHelper
    {
        public static string Truncate(string input, int length)
        {
            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return (input.Substring(0, length) + "...");
            }
        }
    }
}