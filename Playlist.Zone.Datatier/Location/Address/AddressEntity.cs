using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Abstractions.Datatier.Base;
using Compiler.Abstractions.Dto.Common.Location;
using Compiler.Interfaces.Common.Datatier.Address;
using Compiler.Services.Logging;
using Compiler.Utility.Logging;
using Compiler.Utility.Settings;
using Dapper;
using MySql.Data.MySqlClient;

namespace Playlist.Zone.Datatier.Location.Address
{
    public class AddressEntity : BaseEntity<AbstractAddressDto, AddressDto, UnknownAddressDto>, IAddressEntity
    {
        
        public AddressEntity(IBaseSettings pBaseSettings) : base(pBaseSettings, nameof(Address))
        {
        }

        public readonly String Address = string.Empty;
        
    }
}
