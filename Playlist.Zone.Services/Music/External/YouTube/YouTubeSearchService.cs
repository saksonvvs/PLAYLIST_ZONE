using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Services;
using Playlist.Zone.Dto.Music.Song;
using Playlist.Zone.Dto.Music.SearchResult;
using Playlist.Zone.Services.Helpers.Artist;

namespace Playlist.Zone.Services.Music.External
{
    public class YouTubeSearchService : ISearchMusicService
    {
        private int MaxResults = 10;

        private string _ApiKey { get; set; }
        private string _ApplicationName { get; set; }



        public YouTubeSearchService()
        {
            _ApiKey = "AIzaSyDgaLcs8TOSRtioVCJZ7svXx-OmUa5uOUc";
            _ApplicationName = "Playlist.Zone";
        }


        public YouTubeSearchService(string pApiKey, string pApplicationName)
        {
            _ApiKey = pApiKey;
            _ApplicationName = pApplicationName;
        }



        public async Task<string> SearchFirstByKeyword(string p_searchKeyword)
        {
            string returnVideoID = "";

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _ApiKey,
                ApplicationName = _ApplicationName
            });


            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = p_searchKeyword;      
            searchListRequest.MaxResults = MaxResults;

            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<AbstractSongDto> videosList = new List<AbstractSongDto>();
            
            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(searchResult.Id.VideoId);

                        AbstractSongDto foundSong = new PlaylistSongDto();
                        foundSong.YouTubeId = searchResult.Id.VideoId;

                        videosList.Add(foundSong);
                        break;
                }
            }

            if(videos.Count > 0)
            {
                returnVideoID = videos[0];
            }

            return returnVideoID;
        }






        /*
        public async Task<AbstractSearchResultDto> SearchListByKeyword(string p_searchKeyword)
         {
            AbstractSearchResultDto videosList = new Playlist.Zone.Dto.Music.SearchResult.SearchResultDto();

            try
            {
               var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _ApiKey,
                    ApplicationName = _ApplicationName
                });

                var searchListRequest = youtubeService.Search.List("snippet");   //("snippet");
                searchListRequest.Q = p_searchKeyword;
                searchListRequest.MaxResults = 10;

                var searchListResponse = await searchListRequest.ExecuteAsync();

                videosList.NextPageToken = searchListResponse.NextPageToken;

                foreach (var searchResult in searchListResponse.Items)
                {
                    switch (searchResult.Id.Kind)
                    {
                        case "youtube#video":
                            
                            AbstractSongDto foundSong = new PlaylistSongDto();
                            foundSong.YouTubeId = searchResult.Id.VideoId;

                            //will need to strip title to song artist and name
                            foundSong.Artist = searchResult.Snippet.Title;


                            //------------------------------------------------------------
                            var videoRequest = youtubeService.Videos.List("snippet,statistics,contentDetails");
                            videoRequest.Id = searchResult.Id.VideoId;
                            videoRequest.MaxResults = 1;
                            var videoItemRequestResponse = await videoRequest.ExecuteAsync();

                            // Get the videoID of the first video in the list
                            var video = videoItemRequestResponse.Items[0];
                            //foundSong.Duration = video.ContentDetails.Duration;
                            foundSong.Duration = video.ContentDetails.Duration.Replace("PT", "").Replace("M", ":").Replace("S", "");
                            foundSong.ViewsNum = video.Statistics.ViewCount == null ? 0 : (ulong)video.Statistics.ViewCount;
                            foundSong.LikesNum = video.Statistics.LikeCount == null ? 0 : (ulong)video.Statistics.ViewCount;
                            //------------------------------------------------------------


                            videosList.SongsList.Add(foundSong);
                            break;
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Search Exception", ex);
            }


            return videosList;
        }*/




        public async Task<AbstractSearchResultDto> SearchListByKeyword( string pSearchKeyword, string pPageToken = "")
        {
            AbstractSearchResultDto videosList = new Playlist.Zone.Dto.Music.SearchResult.SearchResultDto();

            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _ApiKey,
                    ApplicationName = _ApplicationName
                });


                var searchListRequest = youtubeService.Search.List("id");
                searchListRequest.Q = pSearchKeyword;


                if (pPageToken != string.Empty)
                    searchListRequest.PageToken = pPageToken;


                searchListRequest.MaxResults = MaxResults;


                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = await searchListRequest.ExecuteAsync();


                videosList.NextPageToken = searchListResponse.NextPageToken;


                // Add each result to the appropriate list, and then display the lists of
                // matching videos, channels, and playlists.
                foreach (var searchResult in searchListResponse.Items)
                {
                    switch (searchResult.Id.Kind)
                    {
                        case "youtube#video":
                            AbstractSongDto foundSong = new PlaylistSongDto();
                            

                            var videoRequest = youtubeService.Videos.List("snippet,statistics,contentDetails");
                            videoRequest.Id = searchResult.Id.VideoId;
                            videoRequest.MaxResults = 1;
                            var videoItemRequestResponse = await videoRequest.ExecuteAsync();


                            // Get the videoID of the first video in the list
                            var video = videoItemRequestResponse.Items[0];

                            //will need to strip title to song artist and name
                            foundSong.YouTubeId = searchResult.Id.VideoId;
                            foundSong.Artist  = video.Snippet.Title;
                            foundSong.Duration = video.ContentDetails.Duration.Replace("PT", "").Replace("M", ":").Replace("S", "");
                            foundSong.ViewsNum = video.Statistics.ViewCount == null ? "" : MusicHelper.ConvertViews((ulong)video.Statistics.ViewCount);
                            foundSong.LikesNum = video.Statistics.LikeCount == null ? "" : MusicHelper.ConvertLikes((ulong)video.Statistics.LikeCount);
                            


                            videosList.SongsList.Add(foundSong);
                            break;
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Search Exception", ex);
            }


            return videosList;
        }





        /*
        // need to finish this
        public async Task<List<AbstractSongDto>> GetByYoutubeId(string p_youtubeId)
        {
            List<AbstractSongDto> videosList = new List<AbstractSongDto>();

            try
            {
                string returnVideoID = "";

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _ApiKey,
                    ApplicationName = _ApplicationName
                });

                var searchListRequest = youtubeService.Videos.List("snippet"); //.Search. .List("snippet");
                searchListRequest.Id = p_youtubeId;
                searchListRequest.MaxResults = 10;

                // Call the search.list method to retrieve results matching the specified query term.
                var searchListResponse = await searchListRequest.ExecuteAsync();

                List<string> videos = new List<string>();


                // Add each result to the appropriate list, and then display the lists of
                // matching videos, channels, and playlists.
                foreach (var searchResult in searchListResponse.Items)
                {
                    AbstractSongDto foundSong = new PlaylistSongDto();
                    foundSong.YouTubeId = searchResult.Id;

                    char[] spliOption = { '-' };
                    string[] songInfoArr = searchResult.Snippet.Title.Split(spliOption);
                    string songArtist = "";
                    string songName = "";

                    if(songInfoArr.Length == 2)
                    {
                        songArtist = songInfoArr[0];
                        songName = songInfoArr[1];
                    }
                    else
                    {
                        songArtist = searchResult.Snippet.Title;
                    }

                    //will need to strip title to song artist and name
                    foundSong.Artist = songArtist;
                    foundSong.Name = songName;

                    videosList.Add(foundSong);
                }


                if (videos.Count > 0)
                {
                    returnVideoID = videos[0];
                }

            }
            catch (Exception ex)
            {
                //TODO: log exception
            }


            return videosList;
        }
       


        public async Task<AbstractSongDto> SearchSong(string pSongId)
        {
            
        }*/


        public async Task<AbstractSongDto> GetByYoutubeId(string pYoutubeId)
        {
            if (string.IsNullOrEmpty(pYoutubeId))
                throw new ArgumentOutOfRangeException();


            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _ApiKey,
                ApplicationName = _ApplicationName
            });


            AbstractSongDto foundSong = new PlaylistSongDto();
            foundSong.YouTubeId = pYoutubeId;


            //------------------------------------------------------------
            var videoRequest = youtubeService.Videos.List("snippet,statistics,contentDetails");
            videoRequest.Id = pYoutubeId;
            videoRequest.MaxResults = 1;
            var videoItemRequestResponse = await videoRequest.ExecuteAsync();

            // Get the videoID of the first video in the list
            var video = videoItemRequestResponse.Items[0];

            //will need to strip title to song artist and name
            foundSong.Artist = video.Snippet.Title;
            foundSong.Duration = video.ContentDetails.Duration.Replace("PT", "").Replace("M", ":").Replace("S", "");
            foundSong.ViewsNum = video.Statistics.ViewCount == null ? "" : MusicHelper.ConvertViews((ulong)video.Statistics.ViewCount);
            foundSong.LikesNum = video.Statistics.LikeCount == null ? "" : MusicHelper.ConvertLikes((ulong)video.Statistics.LikeCount);
            //------------------------------------------------------------

            return foundSong;
        }


    }
}
