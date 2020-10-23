using Playlist.Zone.Dto.Music.Charts;

namespace Playlist.Zone.Services.Music.Charts.ChartParser
{
    public interface IChartParser
    {

        bool AnalyzeRawChartData(string p_chartData);
        AbstractChartDto ParseChart(string p_chartData);

    }
}
