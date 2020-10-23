using Compiler.Abstractions.Dto;
using Compiler.Abstractions.Dto.Common.Location;
using Compiler.Abstractions.Dto.Person.User;
using Compiler.Abstractions.Dto.System.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Playlist.Zone.Dto.User.Account
{
    //
    //!!!! not completed - need to bring here all entity keys
    //
    public abstract class AbstractUserAccountDto : AbstractParameterDto
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }


        public AbstractUserDto User { get; set; }
        public AbstractAddressDto Address { get; set; }

        public AbstractUserAccountDto()
        {
            User = new UnknownUserDto();
            Address = new UnknownAddressDto();
        }
    }
}
