using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Services.Helpers.Artist
{
    public static class MusicHelper
    {

        public static string ConvertViews(ulong pViews)
        {
            string result = string.Empty;

            if (pViews > 1000000)
                result = (pViews / 1000000) + "M";
            else
                result = (pViews/1000) + "K";

            return result;
        }


        public static string ConvertLikes(ulong pViews)
        {
            string result = string.Empty;

            if (pViews > 1000000)
                result = (pViews / 1000000) + "M";
            else
                result = (pViews / 1000) + "K";

            return result;
        }


    }
}
