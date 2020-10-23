using System;
using System.Collections.Generic;
using System.Text;
using Compiler.Common.Music;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Dto.Music.Song;

namespace Playlist.Zone.Services.Music.Charts.ChartParser.YoutubeParser
{
    public class YoutubeParser : IChartParser
    {

        public YoutubeParser()
        {
        }


        public bool AnalyzeRawChartData(string p_chartData)
        {
            return true;
        }



        //chart-details
        public AbstractChartDto ParseChart(string p_chartData)
        {
            AbstractChartDto chart = new ChartDto();
            chart.Type = ChartType.YouTube;
            chart.Name = "Top Songs 100 " + DateTime.Now.ToString("MM-dd-yyyy");
            chart.Uid = "Top-Songs-100-" + DateTime.Now.ToString("MM-dd-yyyy");
            chart.Date = DateTime.Now;


            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(p_chartData);


            IEnumerable<HtmlAgilityPack.HtmlNode> currNodeItems = doc.DocumentNode.SelectNodes("//*[contains(@class,'chart-list-item')]");


            string node_html = "";
            int charPosCounter = 1;
            foreach (var item in currNodeItems)
            {
                string dataRank   = string.Empty;
                string dataArtist = string.Empty;
                string dataTitle = string.Empty;

                if (item.Attributes["data-rank"] != null && item.Attributes["data-artist"] != null && item.Attributes["data-title"] != null)
                {
                    dataRank = item.Attributes["data-rank"].Value.ToString();
                    dataArtist = item.Attributes["data-artist"].Value.ToString();
                    dataTitle = item.Attributes["data-title"].Value.ToString();

                    
                    ChartSongDto _song = new ChartSongDto();
                    _song.Name = dataTitle.Replace("\\n", "").Trim();
                    _song.Artist = dataArtist.Replace("\\n", "").Trim();
                    _song.Position = charPosCounter;


                    chart.SongsList.Add(_song);
                    chart.SongNum += 1;

                    charPosCounter += 1;
                }
                
            }

            return chart;
        }
        
        
    }
}
