using Compiler.Abstractions.Datatier.Base;
using Compiler.Abstractions.Dto.Common.Category;
using Compiler.Services.Logging;
using Compiler.Utility.Logging;
using Compiler.Utility.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Datatier.Common.Category
{
    public class CategoryEntity : BaseEntity<AbstractCategoryDto, CategoryDto, UnknownCategoryDto>, ICategoryEntity
    {

        public CategoryEntity(IBaseSettings pBaseSettings)
            : base(pBaseSettings, nameof(Category))
        {
        }

        public readonly string Category = string.Empty;
    }
}
