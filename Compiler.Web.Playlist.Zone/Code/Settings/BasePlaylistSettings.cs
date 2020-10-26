using Compiler.Utility.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compiler.Web.Playlist.Zone.Code.Settings
{
    public class BasePlaylistSettings : IBaseSettings
    {
        private readonly IConfiguration _config;

        public BasePlaylistSettings(IConfiguration pConfig)
        {
            _config = pConfig;
        }


        // currently hardcoded
        //
        public virtual string GetConnString()
        {

            return _config.GetSection("Connections").GetSection("ConString").Value;

#if DEBUG
            //localhost
            return "Server=localhost;Port=3306;Database=playlist_zone;Uid=z_user;Pwd=Zergo.232028;";
#else
            //production
            //return "Server=localhost;Port=3306;Database=playlist_zone;Uid=root;Pwd=viktorolka369;";
#endif


        }

    }
}
