using System;
using System.Collections.Generic;
using System.Text;
using Compiler.Common.Music;
using Newtonsoft.Json.Linq;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Dto.Music.Song;

namespace Playlist.Zone.Services.Music.Charts.ChartParser.ShazamParser
{
    public class ShazamParser : IChartParser
    {

        public ShazamParser()
        {
        }


        public bool AnalyzeRawChartData(string p_chartData)
        {
            return true;
        }


        public AbstractChartDto ParseChart(string p_chartData)
        {
            AbstractChartDto chart = new ChartDto();
            chart.Type = ChartType.Shazam;
            chart.Name = "Shazam 100 " + DateTime.Now.ToString("MM-dd-yyyy");
            chart.Uid  = "Shazam-100-" + DateTime.Now.ToString("MM-dd-yyyy");
            chart.Date     = DateTime.Now;


            JObject ojson = JObject.Parse(p_chartData);

            int charPosCounter = 1;
            foreach (var chartItem in ojson["chart"])
            {
                AbstractChartSongDto _song = new ChartSongDto();
                _song.Name = chartItem["heading"]["title"].ToString();
                _song.Artist = chartItem["heading"]["subtitle"].ToString();
                _song.Position = charPosCounter;


                chart.SongsList.Add(_song);
                chart.SongNum += 1;


                charPosCounter += 1;
            }

            


            return chart;
        }
    }
}
