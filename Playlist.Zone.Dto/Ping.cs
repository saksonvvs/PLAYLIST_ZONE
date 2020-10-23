using System;

namespace Playlist.Zone.Dto
{
    public class Ping
    {

        public string GetStatus()
        {
            return "OK - " + DateTime.Now.ToFileTimeUtc();
        }

    }
}
