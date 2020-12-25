using System;
using System.Collections.Generic;
using System.Text;

namespace covrdf
{
    public static class TextHelper
    {
        public static string TruncateTo(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Substring(0, Math.Min(str.Length, maxLength));
        }
    }
}
