using System;
using System.Collections.Generic;
using System.Text;
using Compiler.Common.Music;
using Playlist.Zone.Services.Music.Charts.ChartParser;
using Playlist.Zone.Dto.Music.Charts;
using Playlist.Zone.Dto.Music.Song;

namespace Playlist.Zone.Music.Charts.ChartParser.BillboardParser
{
    public class BillboardParser : IChartParser
    {

        public BillboardParser()
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
            chart.Type = ChartType.Billboard;
            chart.Name = "Billboard Hot 100 " + DateTime.Now.ToString("MM-dd-yyyy");
            chart.Uid = "Billboard-Hot-100-" + DateTime.Now.ToString("MM-dd-yyyy");
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

                /*node_html = item.InnerHtml;

                HtmlAgilityPack.HtmlDocument itemNode = new HtmlAgilityPack.HtmlDocument();
                itemNode.LoadHtml(node_html);

                HtmlAgilityPack.HtmlNode currNodeSongName = itemNode.DocumentNode.SelectSingleNode("//*[contains(@class,'chart-row__song')]");
                HtmlAgilityPack.HtmlNode currNodeArtistName = itemNode.DocumentNode.SelectSingleNode("//*[contains(@class,'chart-row__artist')]");


                ChartSongDto _song = new ChartSongDto();
                _song.Name = currNodeSongName.InnerText.Replace("\\n", "").Trim();
                _song.Artist = currNodeArtistName.InnerText.Replace("\\n", "").Trim();
                _song.Position = charPosCounter;


                chart.SongsList.Add(_song);
                chart.SongNum += 1;

                charPosCounter += 1;*/


            }

            return chart;
        }



        /*
        public AbstractChartDto ParseChart(string p_chartData)
        {
            AbstractChartDto chart = new ChartDto();
            chart.Type = ChartType.Billboard;
            chart.Name      = "Billboard Hot 100 " + DateTime.Now.ToString("MM-dd-yyyy");
            chart.Uid = "Billboard-Hot-100-" + DateTime.Now.ToString("MM-dd-yyyy");
            chart.Date = DateTime.Now;


            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(p_chartData);


            IEnumerable<HtmlAgilityPack.HtmlNode> currNodeItems = doc.DocumentNode.SelectNodes("//*[contains(@class,'chart-row__container')]");

            string node_html = "";
            int charPosCounter = 1;
            foreach (var item in currNodeItems)
            {
                node_html = item.InnerHtml;

                HtmlAgilityPack.HtmlDocument itemNode = new HtmlAgilityPack.HtmlDocument();
                itemNode.LoadHtml(node_html);

                HtmlAgilityPack.HtmlNode currNodeSongName   = itemNode.DocumentNode.SelectSingleNode("//*[contains(@class,'chart-row__song')]");
                HtmlAgilityPack.HtmlNode currNodeArtistName = itemNode.DocumentNode.SelectSingleNode("//*[contains(@class,'chart-row__artist')]");


                ChartSongDto _song = new ChartSongDto();
                _song.Name   = currNodeSongName.InnerText.Replace("\\n", "").Trim();
                _song.Artist = currNodeArtistName.InnerText.Replace("\\n", "").Trim();
                _song.Position = charPosCounter;
                    

                chart.SongsList.Add(_song);
                chart.SongNum += 1;

                charPosCounter += 1;
            }

            return chart;
        }*/
    }
}
