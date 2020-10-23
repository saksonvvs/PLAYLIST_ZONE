using Compiler.Abstractions.Datatier.Base;
using Compiler.Abstractions.Dto.Common.KeyValue;
using Compiler.Services.Logging;
using Compiler.Utility.Logging;
using Compiler.Utility.Settings;
using Playlist.Zone.Dto.Common.KeyValue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Datatier.Common.KeyValue
{
    public class KeyValueEntity : BaseEntity<AbstractKeyValueDto, KeyValueDto, UnknownKeyValueDto>, IKeyValueEntity
    {
        public KeyValueEntity(IBaseSettings pBaseSettings)
            : base(pBaseSettings, nameof(Key_Value_Pair))
        {
        }

        public readonly string Key_Value_Pair = string.Empty;
    }
}
