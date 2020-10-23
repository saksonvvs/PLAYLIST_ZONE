using Compiler.Interfaces.Common.Datatier;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Music.Charts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Playlist.Zone.Datatier.Music.Charts
{
    public interface IChartEntity : IBaseEntity<AbstractChartDto>
    {
        Task<List<AbstractChartDto>> GetByType(int chartType);
        Task<List<AbstractChartDto>> GetPopular();
        Task<List<AbstractChartDto>> GetRecent();

        Task<bool> AddView(int p_chart_id);
        Task<bool> AddLike(int p_chart_id);
    }
}
