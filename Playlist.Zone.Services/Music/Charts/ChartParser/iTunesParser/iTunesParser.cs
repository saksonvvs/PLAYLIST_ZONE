using System;
using System.Collections.Generic;
using Compiler.Common.Music;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Dto.Music.Song;

namespace Playlist.Zone.Services.Music.Charts.ChartParser.iTunesParser
{
    public class iTunesParser : IChartParser
    {

        public iTunesParser()
        {
        }


        public bool AnalyzeRawChartData(string p_chartData)
        {
            return true;
        }


        public AbstractChartDto ParseChart(string p_chartData)
        {
            AbstractChartDto chart = new ChartDto();
            chart.Type = ChartType.iTunes;
            chart.Name = "iTunes Hot 100 " + DateTime.Now.ToString("MM-dd-yyyy");
            chart.Uid = "iTunes-Hot-100-" + DateTime.Now.ToString("MM-dd-yyyy");
            chart.Date = DateTime.Now;


            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(p_chartData);

            //<div class="section-conten-">
            //HtmlAgilityPack.HtmlNode fNode = doc.DocumentNode.SelectSingleNode("");

            IEnumerable<HtmlAgilityPack.HtmlNode> currNodeItems = doc.DocumentNode.SelectNodes("//section[@class='section chart-grid']//li");

            string node_html = "";
            int charPosCounter = 1;
            foreach (var item in currNodeItems)
            {
                node_html = item.InnerHtml;

                HtmlAgilityPack.HtmlDocument itemNode = new HtmlAgilityPack.HtmlDocument();
                itemNode.LoadHtml(node_html);

                HtmlAgilityPack.HtmlNode currNodeSongName   = itemNode.DocumentNode.SelectSingleNode("//h3//a");
                HtmlAgilityPack.HtmlNode currNodeArtistName = itemNode.DocumentNode.SelectSingleNode("//h4//a");


                AbstractChartSongDto _song = new ChartSongDto();
                _song.Name   = currNodeSongName.InnerText.Replace("\\n", "").Trim();
                _song.Artist = currNodeArtistName.InnerText.Replace("\\n", "").Trim();
                _song.Position = charPosCounter;
                    

                chart.SongsList.Add(_song);
                chart.SongNum += 1;

                charPosCounter += 1;
            }

            return chart;
        }
    }
}
