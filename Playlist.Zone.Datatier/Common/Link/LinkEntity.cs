using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Abstractions.Datatier.Base;
using Compiler.Abstractions.Dto.Common.Link;
using Compiler.Services.Logging;
using Compiler.Utility.Logging;
using Compiler.Utility.Settings;
using Dapper;
using MySql.Data.MySqlClient;
using Playlist.Zone.Dto.Common.Link;

namespace Compiler.Datatier.Common.Link
{
    public class LinkEntity : BaseEntity<AbstractLinkDto, LinkDto, UnknownLinkDto>, ILinkEntity
    {
        

        public LinkEntity(IBaseSettings pBaseSettings) 
            : base(pBaseSettings, nameof(Link))
        {
        }

        public readonly string Link = string.Empty;




        public async Task<List<AbstractLinkDto>> GetByCategory(int pCategoryId)
        {
            List<AbstractLinkDto> retObject = new List<AbstractLinkDto>();

            
            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT CL.* FROM "+ nameof(Link) + " AS CL WHERE CL.CategoryId = @p_CategoryId ";

                var schemePolicy = await db.QueryAsync<LinkDto>(sql_qry,
                    new
                    {
                        @p_CategoryId = pCategoryId
                    });


                retObject = schemePolicy.ToList<AbstractLinkDto>();

                
                retObject = retObject == null ? new List<AbstractLinkDto>() : retObject;
            }


            return retObject;
        }



        public async Task<List<AbstractLinkDto>> GetBySection(int pSectionId)
        {
            List<AbstractLinkDto> retObject = new List<AbstractLinkDto>();

            
            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT CL.* FROM "+ nameof(Link) +" AS CL WHERE CL.SectionId = @p_SectionId ";

                var schemePolicy = await db.QueryAsync<LinkDto>(sql_qry,
                    new
                    {
                        @p_SectionId = pSectionId
                    });

                retObject = schemePolicy.ToList<AbstractLinkDto>();

                
                retObject = retObject == null ? new List<AbstractLinkDto>() : retObject;
            }


            return retObject;
        }


        

    }
}
