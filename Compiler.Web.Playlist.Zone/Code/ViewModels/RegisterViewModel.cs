using Compiler.Abstractions.Dto.Person.User;
using Compiler.Abstractions.Dto.System.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Compiler.Web.Playlist.Zone.Code.ViewModels
{
    public class RegisterViewModel
    {
        public AbstractUserDto User { get; set; }


        public RegisterViewModel()
        {
            User = new UnknownUserDto();
        }

    }
}
