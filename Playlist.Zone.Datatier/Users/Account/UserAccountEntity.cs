using Compiler.Abstractions.Datatier.Base;
using Compiler.Abstractions.Dto.Common.Location;
using Compiler.Abstractions.Dto.System.User.Account;
using Compiler.Interfaces.Common.Datatier.Address;
using Compiler.Interfaces.Common.Datatier.User;
using Compiler.Services.Logging;
using Compiler.Utility.Logging;
using Compiler.Utility.Settings;
using Dapper;
using MySql.Data.MySqlClient;
using Playlist.Zone.User.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Datatier.Users.Account
{
    public class UserAccountEntity : BaseEntity<AbstractUserAccountDto, UserAccountDto, UnknownUserAccountDto>, IUserAccountEntity
    {
        
        protected readonly IUserEntity _userManager;
        protected readonly IAddressEntity _addressManager;


        public UserAccountEntity(
            IBaseSettings pBaseSettings, 
            IUserEntity pUserManager,
            IAddressEntity pAddressManager) : base(pBaseSettings, nameof(User_Account))
        {
            _userManager = pUserManager;
            _addressManager = pAddressManager;
        }

        public readonly string User_Account = string.Empty;





        public async Task<AbstractUserAccountDto> GetUserAccount(int pUserId)
        {
            if (pUserId <= 0)
                throw new ArgumentOutOfRangeException();

            return await GetById(pUserId);
        }



        //!!!! need to double check this function
        //
        public override async Task<AbstractUserAccountDto> GetById(int pUserId)
        {
            AbstractUserAccountDto retObj = new UnknownUserAccountDto();

            retObj = await base.GetById(pUserId);

            
            if (retObj != null && retObj.Id > 0)
            {
                retObj.User = await _userManager.GetById(pUserId);
                retObj.Address = await _addressManager.GetById(retObj.AddressId);
            }
            else
            {
                retObj = new UserAccountDto();
                retObj.User = await _userManager.GetById(pUserId);
                retObj.Address = new UnknownAddressDto();
            }


            return retObj;
        }



        public override async Task<int> Add(AbstractUserAccountDto pUserAccount)
        {

            if(pUserAccount.User == null || pUserAccount.User.Id <= 0)
                throw new ArgumentException("User parameter is null or empty");



            if (pUserAccount.User.Id > 0)
                await _userManager.Update(pUserAccount.User);

            if (pUserAccount.Address.Id <= 0)
                pUserAccount.Address.Id = await _addressManager.Add(pUserAccount.Address);
            else
                await _addressManager.Update(pUserAccount.Address);


            await base.Add(pUserAccount);

            
            return pUserAccount.Id;
        }




        public override async Task<bool> Update(AbstractUserAccountDto pUserAccount)
        {
            if (pUserAccount.User == null || pUserAccount.User.Id <= 0)
                throw new ArgumentException("User parameter is null or empty");



            if (pUserAccount.User.Id > 0)
                await _userManager.Update(pUserAccount.User);


            if (pUserAccount.Address.Id <= 0)
                pUserAccount.Address.Id = await _addressManager.Add(pUserAccount.Address);
            else
                await _addressManager.Update(pUserAccount.Address);


            await base.Update(pUserAccount);
            

            return true;
        }

        
    }
}
