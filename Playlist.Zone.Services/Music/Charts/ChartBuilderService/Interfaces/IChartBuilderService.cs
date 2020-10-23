using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Services.Music.Charts.ChartParser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Music.Charts.ChartBuilderService
{
    public interface IChartBuilderService
    {
        System.Threading.Tasks.Task<AbstractChartDto> BuildChart(string p_chartUrl, List<KeyValuePair<string, string>> p_chartHeaders, IChartParser p_chartParser);

        System.Threading.Tasks.Task<AbstractChartDto> BuildChart(string p_chartUrl, string p_post_data, List<KeyValuePair<string, string>> p_chartHeaders, IChartParser p_chartParser);
        
    }
}
