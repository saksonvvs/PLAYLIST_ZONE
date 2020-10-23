using Compiler.Abstractions.Dto.Common.Link;
using Compiler.Interfaces.Common.Datatier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Datatier.Common.Link
{
    public interface ILinkEntity : IBaseEntity<AbstractLinkDto>
    {
        Task<List<AbstractLinkDto>> GetByCategory(int pCategory);
        Task<List<AbstractLinkDto>> GetBySection(int pSectionId);
    }
}
