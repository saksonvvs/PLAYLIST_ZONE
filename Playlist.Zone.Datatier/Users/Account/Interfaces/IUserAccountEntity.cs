using Compiler.Abstractions.Dto.System.User.Account;
using Compiler.Interfaces.Common.Datatier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Datatier.Users.Account
{
    public interface IUserAccountEntity : IBaseEntity<AbstractUserAccountDto>
    {
        Task<AbstractUserAccountDto> GetUserAccount(int pUserId);
        
    }
}
