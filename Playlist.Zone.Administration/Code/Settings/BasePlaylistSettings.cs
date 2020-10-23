using Compiler.Utility.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlist.Zone.Administration.Code.Settings
{
    public class BasePlaylistSettings : IBaseSettings
    {
        // currently hardcoded
        //
        public virtual string GetConnString()
        {   
#if DEBUG
            //localhost
            return "Server=localhost;Port=3306;Database=playlist_zone;Uid=z_user;Pwd=Zergo.232028;";
#else
            //production
            return "Server=localhost;Port=3306;Database=playlist_zone;Uid=root;Pwd=viktorolka369;";
#endif


        }

    }
}
