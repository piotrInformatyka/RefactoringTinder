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
        [StringLength(12, MinimumLength =6,ErrorMessage ="hasło musi się składać z 4-10znaków")]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string ZodiacSign { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}
