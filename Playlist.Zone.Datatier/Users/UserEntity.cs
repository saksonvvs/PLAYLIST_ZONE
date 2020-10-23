using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Compiler.Utility.Logging;
using Compiler.Services.Logging;
using Compiler.Abstractions.Dto.System.User;
using Compiler.Abstractions.Dto.Person.User;
using Compiler.Abstractions.Datatier.Base;
using Compiler.Utility.Settings;
using Compiler.Interfaces.Common.Datatier.User;

namespace Playlist.Zone.Datatier.Users
{
    public class UserEntity : BaseEntity<AbstractUserDto, UserDto, UnknownUserDto>, IUserEntity
    {


        public UserEntity(IBaseSettings pBaseSettings) 
            : base(pBaseSettings, nameof(User))
        {
        }


        public readonly string User = string.Empty;
        


        public virtual async Task<AbstractUserDto> GetByUsername(string p_username)
        {
            AbstractUserDto retObj = new UnknownUserDto();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT U.* FROM "+ nameof(User) + @" AS U 
                                        WHERE U.Username = @p_userName AND U.IsDeleted = 0 ";
                
                var schemePolicy = await db.QueryAsync<UserDto>(sql_qry, param: new { p_userName = p_username });

                retObj = schemePolicy.FirstOrDefault();
                
                retObj = retObj == null ? new UnknownUserDto() : retObj;
            }

            return retObj;
        }
        
        public virtual async Task<AbstractUserDto> GetByEmail(string p_userEmail)
        {
            AbstractUserDto retObj = new UnknownUserDto();

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT U.* FROM " + nameof(User) + @" AS U 
                                        WHERE U.Email = @p_userEmail AND U.IsDeleted = 0 ";
                
                var schemePolicy = await db.QueryAsync<UserDto>(sql_qry, param: new { p_userEmail = p_userEmail });

                retObj = schemePolicy.FirstOrDefault();
                
                retObj = retObj == null ? new UnknownUserDto() : retObj;
                
            }

            return retObj;
        }
        
        public virtual async Task<AbstractUserDto> SignInUser(string p_username, string p_password)
        {
            AbstractUserDto retObj = new UnknownUserDto();
            

            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {
                string sql_qry = @" SELECT U.* FROM " + nameof(User) + @" AS U 
                                        WHERE U.Username = @p_userName AND U.Password = @p_userPassword AND U.isDeleted = 0 ";
                
                var schemePolicy = await db.QueryAsync<UserDto>(sql_qry, param: new { p_userName = p_username, p_userPassword = p_password });

                retObj = schemePolicy.FirstOrDefault();
                
                retObj = retObj == null ? new UnknownUserDto() : retObj;
            }


            return retObj;
        }
        
        public virtual async Task<int> EditProfile(AbstractUserDto p_user)
        {
            using (System.Data.IDbConnection db = new MySqlConnection(_baseSettings.GetConnString()))
            {   
                string insertQry = @" UPDATE "+ nameof(User) + @" SET 
                                                        Password   = p_userPassword,
                                                        FirstName  = p_firstName,
                                                        LastName   = p_lastName,
                                                        Image      = p_userImage,
                                                        IsConfirmed  = p_isConfirmed,
                                                        RegistrationDate = p_registrationDate,
                                                        LoginDate    = p_loginDate,
                                                        PasswordDate = p_passwordDate
                                                    WHERE p_Id = @p_Id ";

                var schemePolicy = await db.QueryAsync<int>(insertQry, new
                {
                    p_userPassword = p_user.Password,
                    p_firstName = p_user.FirstName,
                    p_lastName = p_user.LastName,
                    p_userImage = p_user.Image,
                    p_isConfirmed = p_user.IsConfirmed,
                    p_registrationDate = p_user.RegistrationDate,
                    p_loginDate = p_user.LoginDate,
                    p_passwordDate = p_user.PasswordDate,
                    p_Id = p_user.Id
                });

                p_user.Id = schemePolicy.Single();
            }


            return p_user.Id;
        }



        
    }
}
