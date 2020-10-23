using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Compiler.Web.Playlist.Zone.Code
{
    public class SessionManagement
    {



        public static int GetSessionUserID(ClaimsPrincipal p_user)
        {
            int ret_user_id = 0;

            //----------------------------------------------------------
            var identity = (ClaimsIdentity)p_user.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            foreach (var item in claims)
            {
                if (item.Type == ClaimTypes.NameIdentifier)
                {
                    Int32.TryParse(item.Value, out ret_user_id);
                    break;
                }
            }
            //----------------------------------------------------------

            return ret_user_id;
        }





        public static string GetSessionUserImage(ClaimsPrincipal p_user)
        {
            string ret_user_image = "noimage.png";


            //----------------------------------------------------------
            var identity = (ClaimsIdentity)p_user.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            foreach (var item in claims)
            {
                if (item.Type == ClaimTypes.Spn)
                {
                    ret_user_image = item.Value;
                    break;
                }
            }
            //----------------------------------------------------------


            return ret_user_image;
        }




        public static bool GetSessionIsAdmin(ClaimsPrincipal p_user)
        {
            bool isAdmin = p_user.HasClaim(ClaimTypes.Role, "Admin");

            return isAdmin;
        }




    }
}
