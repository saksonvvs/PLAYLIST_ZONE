using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Playlist.Zone.Dto.Music.Song;
using Compiler.Services.Logging;
using Compiler.Abstractions.Datatier.Base;
using Compiler.Utility.Settings;

namespace Playlist.Zone.Datatier.Music.Songs
{
    public class SongsEntity : BaseEntity<AbstractSongDto, PlaylistSongDto, UnknownSongDto>, ISongsEntity
    {
        
        public SongsEntity(
            IBaseSettings pSettingsConfig, 
            ILogService pLogManager) 
            : base(pSettingsConfig, nameof(Playlist_Song))
        {
        }

        public readonly string Playlist_Song = string.Empty;

        
        public async Task<List<AbstractSongDto>> GetAllPlaylistSongs(int pUserId, int pPlaylistId)
        {
            List<AbstractSongDto> retObj = new List<AbstractSongDto>();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT P.* FROM "+ nameof(Playlist_Song) + " AS P WHERE P.UserId = @p_UserId AND P.PlaylistId = @p_PlaylistId AND P.IsDeleted = 0 ";

                var schemePolicy = await db.QueryAsync<PlaylistSongDto>(sql_qry, new {
                    @p_UserId = pUserId,
                    @p_PlaylistId = pPlaylistId
                });

                retObj = schemePolicy.ToList<AbstractSongDto>();
                
                retObj = retObj == null ? new List<AbstractSongDto>() : retObj;
            }

            return retObj;
        }



    }
}
