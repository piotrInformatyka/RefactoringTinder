using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tinder.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required(ErrorMessage ="Nazwa uzytkownika jest wymagana")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Nazwa uzytkownika jest wymagana")]
        [StringLength(12, MinimumLength =6,ErrorMessage ="hasło musi się składać z 6-12znaków")]
        public string Password { get; set; }
    }
}
