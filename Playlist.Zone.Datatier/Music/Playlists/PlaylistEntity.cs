using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Playlist.Zone.Datatier.Music.Songs;
using Playlist.Zone.Dto.Music.Playlist;
using Playlist.Zone.Dto.Music.Song;
using Compiler.Services.Logging;
using Compiler.Abstractions.Datatier.Base;
using Compiler.Interfaces.Common.Datatier.Tag;
using Compiler.Utility.Settings;
using Compiler.Abstractions.Dto.Common.Tag;

namespace Playlist.Zone.Datatier.Music.Playlists
{
    public class PlaylistEntity : BaseEntity<AbstractPlaylistDto, PlaylistDto, UnknownPlaylistDto>, IPlaylistEntity
    {
        
        private readonly ITagEntity _tagEntity;
        private readonly ISongsEntity _songEntity;

        private readonly int PAGE_LIMIT = 20;

        public PlaylistEntity(
            IBaseSettings pSettingsConfig, 
            ITagEntity pTagEntity,
            ISongsEntity pSongEntity) 
            : base(pSettingsConfig, nameof(Playlist))
        {
            _tagEntity = pTagEntity;
            _songEntity = pSongEntity;
        }


        public readonly string Playlist = string.Empty;
        public readonly string Tag_Owner_Rel = string.Empty;



              
        public override async Task<AbstractPlaylistDto> GetById(int pId)
        {
            AbstractPlaylistDto result = new UnknownPlaylistDto();
            

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string chartQry = @" SELECT * FROM "+ nameof(Playlist) +" WHERE Id = @p_Id ";
                var schemePolicy = await db.QueryAsync<PlaylistDto>(chartQry, new { p_Id = pId });
                result = schemePolicy.Single<AbstractPlaylistDto>();
                result = result == null ? new UnknownPlaylistDto() : result;
            }

            if (result.Id <= 0) return result;
                
            result.SongsList = await _songEntity.GetAllPlaylistSongs(result.UserId, result.Id);
            result.TagsList  = await _tagEntity.GetByOwnerId(result.Id);

            return result;
        }




        public async Task<List<AbstractPlaylistDto>> GetAllUserPlaylists(int p_userID)
        {
            List<AbstractPlaylistDto> result = new List<AbstractPlaylistDto>();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT P.* FROM "+ nameof(Playlist) + @" AS P 
                                        WHERE P.isDeleted = 0 AND P.userID = @p_userID ";

                var schemePolicy = await db.QueryAsync<PlaylistDto>(sql_qry, param: new { p_userID = p_userID });
                result = schemePolicy.ToList<AbstractPlaylistDto>();
                result = result == null ? new List<AbstractPlaylistDto>() : result;
            }

            return result;
        }

        


        public async Task<List<AbstractPlaylistDto>> GetPopular()
        {
            List<AbstractPlaylistDto> result = new List<AbstractPlaylistDto>();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT P.* FROM " + nameof(Playlist) + @" AS P 
                                        WHERE P.isDeleted = 0 
                                             ORDER BY ViewNum DESC LIMIT " + PAGE_LIMIT;


                var schemePolicy = await db.QueryAsync<PlaylistDto>(sql_qry);
                result = schemePolicy.ToList<AbstractPlaylistDto>();
                result = result == null ? new List<AbstractPlaylistDto>() : result;
            }

            return result;
        }



        public async Task<List<AbstractPlaylistDto>> GetMostViewed()
        {
            List<AbstractPlaylistDto> result = new List<AbstractPlaylistDto>();


            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT P.* FROM "+ nameof(Playlist) + @" AS P 
                                        WHERE P.isDeleted = 0 
                                             ORDER BY ViewNum DESC LIMIT " + PAGE_LIMIT;

                var schemePolicy = await db.QueryAsync<PlaylistDto>(sql_qry);
                result = schemePolicy.ToList<AbstractPlaylistDto>();
                result = result == null ? new List<AbstractPlaylistDto>() : result;
            }

            return result;
        }



        public async Task<List<AbstractPlaylistDto>> GetRecent()
        {
            throw new NotImplementedException();
        }




        public async Task<bool> SetDefaultPlaylist(int pUserId, int pPlaylistId)
        {
            if (pUserId <= 0) throw new ArgumentOutOfRangeException();
            if (pPlaylistId <= 0) throw new ArgumentOutOfRangeException();



            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry_reset = @" UPDATE "+ nameof(Playlist) + @" AS PS
                                        SET IsDefault = 0
                                        WHERE PS.UserId = @p_userID ";
                await db.ExecuteAsync(sql_qry_reset, param: new { p_userID = pUserId });



                string sql_qry = @" UPDATE "+ nameof(Playlist) + @" AS PS
                                        SET IsDefault = 1
                                        WHERE PS.UserId = @p_userID AND PS.Id = @p_playlistID ";
                await db.ExecuteAsync(sql_qry, param: new { p_userID = pUserId, p_playlistID = pPlaylistId });
            }

            return true;
        }




        public async Task<bool> AddView(int pUserId, int pPlaylistId)
        {
            if (pUserId <= 0) throw new ArgumentOutOfRangeException();
            if (pPlaylistId <= 0) throw new ArgumentOutOfRangeException();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {   
                string sql_qry = @" UPDATE "+ nameof(Playlist) + @" AS PS
                                        SET ViewNum = ViewNum + 1
                                        WHERE PS.UserId = @p_userID AND PS.Id = @p_playlistID ";
                await db.ExecuteAsync(sql_qry, param: new { p_userID = pUserId, p_playlistID = pPlaylistId });
            }

            return true;
        }


        public async Task<bool> AddLike(int pUserId, int pPlaylistId)
        {
            if (pUserId <= 0) throw new ArgumentOutOfRangeException();
            if (pPlaylistId <= 0) throw new ArgumentOutOfRangeException();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" UPDATE " + nameof(Playlist) + @" AS PS
                                        SET LikeNum = LikeNum + 1
                                        WHERE PS.UserId = @p_userID AND PS.Id = @p_playlistID ";
                await db.ExecuteAsync(sql_qry, param: new { p_userID = pUserId, p_playlistID = pPlaylistId });
            }

            return true;
        }

        

        public async Task<bool> AddTag(AbstractTagDto pTag)
        {
            if (pTag == null) throw new ArgumentNullException(nameof(pTag) + " is null");

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = " INSERT INTO tag_owner_rel (TagId, OwnerId) VALUES (@p_TagId, @p_OwnerId) ";

                await db.ExecuteAsync(sql_qry,
                    param: new
                    {
                        @p_TagId = pTag.Id,
                        p_OwnerId = pTag.OwnerId
                    });
            }

            return true;
        }
        


        public async Task<List<AbstractPlaylistDto>> GetByTagId(int pTagId)
        {
            if (pTagId <= 0) throw new ArgumentOutOfRangeException();


            List<AbstractPlaylistDto> retObjList = new List<AbstractPlaylistDto>();


            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" select PLST.* from " + nameof(Tag_Owner_Rel) + @" AS TG 
                                        LEFT JOIN playlist AS PLST ON TG.OwnerId = PLST.Id
                                            WHERE TG.TagId = @p_TagId ";


                var schemePolicy = await db.QueryAsync<PlaylistDto>(sql_qry, param: new { @p_TagId = pTagId });
                retObjList = schemePolicy.ToList<AbstractPlaylistDto>();
                retObjList = retObjList == null ? new List<AbstractPlaylistDto>() : retObjList;
            }

            return retObjList;
        }




        public async Task<IEnumerable<string>> GetPlaylistIcons(int pPlaylistId)
        {
            if (pPlaylistId <= 0) throw new ArgumentOutOfRangeException();


            IEnumerable<string> result = new List<string>();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT PS.YouTubeId FROM playlist_song AS PS 
                                        WHERE PS.isDeleted = 0 AND PS.playlistID = @p_playlistID LIMIT " + PAGE_LIMIT;

                var schemePolicy = await db.QueryAsync<string>(sql_qry, param: new { p_playlistID = pPlaylistId });
                result = schemePolicy.ToList<string>();
                result = result == null ? new List<string>() : result;
            }

            return result;
        }



    }
}
