using Compiler.Utility.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlist.Zone.Administration.Code.Settings
{
    public class BasePlaylistSettings : IBaseSettings
    {

        private readonly IConfiguration _config;

        public BasePlaylistSettings(IConfiguration pConfig)
        {
            _config = pConfig;
        }


        public virtual string GetConnString()
        {
            return _config.GetSection("Connections").GetSection("ConString").Value;
        }


    }
}
