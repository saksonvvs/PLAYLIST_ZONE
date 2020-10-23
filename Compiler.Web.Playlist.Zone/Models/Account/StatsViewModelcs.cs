using Playlist.Zone.Dto.Music.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compiler.Web.Playlist.Zone.Models.Account
{
    public class StatsViewModel
    {
        public IList<AbstractPlaylistDto> playlists { get; set; }

        public StatsViewModel()
        {
            playlists = new List<AbstractPlaylistDto>();
        }




    }
}
