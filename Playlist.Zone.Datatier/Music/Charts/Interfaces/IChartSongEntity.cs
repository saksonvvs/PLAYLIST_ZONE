using Compiler.Interfaces.Common.Datatier;
using Playlist.Zone.Dto.Music.Song;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Playlist.Zone.Datatier.Music.Charts
{
    public interface IChartSongEntity : IBaseEntity<AbstractChartSongDto>
    {
        Task<List<AbstractChartSongDto>> GetPopular();
        Task<List<AbstractChartSongDto>> GetRecent();

        Task<List<AbstractChartSongDto>> GetPlaylistSongs(int chartId);
    }
}
