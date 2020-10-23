using System;
using System.Collections.Generic;
using System.Text;
using Compiler.Interfaces.Common.Datatier.User;
using Compiler.Services.Logging;
using Compiler.Utility.Logging;
using Compiler.Utility.Settings;
using Playlist.Zone.Datatier.Users;

namespace Compiler.Datatier.Users
{
    public class UserManagementAdmin : UserEntity, IUserEntity
    {
        
        public UserManagementAdmin(IBaseSettings p_baseSettings) : base(p_baseSettings)
        {
        }
        
        //
        //!!! will need to implement this method
        //

    }
}
