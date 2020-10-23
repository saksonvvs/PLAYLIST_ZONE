using System;

namespace Playlist.Zone.Datatier
{
    public class Ping
    {

        public string GetStatus()
        {
            return "OK - " + DateTime.Now.ToFileTimeUtc();
        }

    }
}
