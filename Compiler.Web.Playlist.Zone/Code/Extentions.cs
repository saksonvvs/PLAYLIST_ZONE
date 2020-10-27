using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compiler.Web.Playlist.Zone.Code
{
    public static class Extentions
    {

        public static string ToCustomString(this string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException();



            return value;
        }

    }
}
