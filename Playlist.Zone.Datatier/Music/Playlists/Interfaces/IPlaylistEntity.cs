using Compiler.Abstractions.Dto.Common.Tag;
using Compiler.Interfaces.Common.Datatier;
using Playlist.Zone.Dto.Music.Playlist;
using Playlist.Zone.Dto.Music.Song;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Playlist.Zone.Datatier.Music.Playlists
{
    public interface IPlaylistEntity : IBaseEntity<AbstractPlaylistDto>
    {
        Task<List<AbstractPlaylistDto>> GetAllUserPlaylists(int p_userID);

        
        Task<List<AbstractPlaylistDto>> GetPopular();
        Task<List<AbstractPlaylistDto>> GetRecent();
        Task<List<AbstractPlaylistDto>> GetMostViewed();


        Task<bool> SetDefaultPlaylist(int p_UserId, int p_PlaylistId);
        Task<bool> AddView(int p_user_id, int p_playlist_id);
        Task<bool> AddLike(int p_user_id, int p_playlist_id);

        Task<bool> AddTag(AbstractTagDto pTag);

        Task<List<AbstractPlaylistDto>> GetByTagId(int pTagId);



        Task<IEnumerable<string>> GetPlaylistIcons(int pPlaylistId);
    }
}
