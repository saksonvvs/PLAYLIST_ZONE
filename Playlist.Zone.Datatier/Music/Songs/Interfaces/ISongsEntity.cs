using Compiler.Interfaces.Common.Datatier;
using Playlist.Zone.Dto.Music.Song;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Playlist.Zone.Datatier.Music.Songs
{
    public interface ISongsEntity : IBaseEntity<AbstractSongDto>
    {

        Task<List<AbstractSongDto>> GetAllPlaylistSongs(int p_userID, int p_playlistID);

        
    }
}
