using Compiler.Abstractions.Datatier.Base;
using Compiler.Abstractions.Dto.Common.Tag;
using Compiler.Interfaces.Common.Datatier.Tag;
using Compiler.Services.Logging;
using Compiler.Utility.Logging;
using Compiler.Utility.Settings;
using Dapper;
using MySql.Data.MySqlClient;
using Playlist.Zone.Dto.Common.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Datatier.Common.Tag
{
    public class TagEntity : BaseEntity<AbstractTagDto, TagDto, UnknownTagDto>, ITagEntity
    {

        public TagEntity(IBaseSettings pBaseSettings)
            : base(pBaseSettings, nameof(Tag))
        {
        }

        public readonly string Tag = string.Empty;
        public readonly string Tag_Owner_Rel = string.Empty;


        public async Task<List<AbstractTagDto>> GetRandomTags()
        {
            List<AbstractTagDto> retObjList = new List<AbstractTagDto>();


            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT T.* FROM " + nameof(Tag) + @" AS T 
                                        ORDER BY RAND()
                                            LIMIT 20 ";


                var schemePolicy = await db.QueryAsync<TagDto>(sql_qry);

                retObjList = schemePolicy.ToList<AbstractTagDto>();

                retObjList = retObjList == null ? new List<AbstractTagDto>() : retObjList;

            }

            return retObjList;
        }



        public async Task<AbstractTagDto> GetByName(string pName)
        {
            AbstractTagDto retObj = new UnknownTagDto();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT T.* FROM " + nameof(Tag) + @" AS T 
                                        WHERE Name = @p_Name ";
                
                var schemePolicy = await db.QueryAsync<TagDto>(sql_qry, new { @p_Name = pName });

                retObj = schemePolicy.FirstOrDefault<AbstractTagDto>();

                retObj = retObj == null ? new TagDto() : retObj;
            }
            
            return retObj;
        }



        public async Task<List<AbstractTagDto>> GetByOwnerId(int pOwnerId)
        {
            List<AbstractTagDto> retObject = new List<AbstractTagDto>();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string plstTagsQry = @" SELECT * FROM tag_owner_rel  PLST_REL 
                                                    LEFT JOIN tag AS TG ON PLST_REL.TagId = TG.Id
                                                        WHERE PLST_REL.OwnerId = @p_OwnerId ";

                var schemePolicy3 = await db.QueryAsync<TagDto>(plstTagsQry, new { p_OwnerId = pOwnerId });
                retObject = schemePolicy3.ToList<AbstractTagDto>();
                if (retObject == null) retObject = new List<AbstractTagDto>();
            }

            return retObject;
        }


    }
}
