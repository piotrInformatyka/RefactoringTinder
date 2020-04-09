using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinder.API.Models;

namespace Tinder.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }

        //Podstawowe informacje
        public string Gender { get; set; }
        public int Age { get; set; }
        public string ZodiacSign { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        //Zakładka info
        public string Growth { get; set; }
        public string EyeColor { get; set; }
        public string HairColor { get; set; }
        public string MartialStatus { get; set; }
        public string Education { get; set; }
        public string Profession { get; set; }
        public string Children { get; set; }
        public string Languages { get; set; }

        //O mnie
        public string Motto { get; set; }
        public string Description { get; set; }
        public string Personality { get; set; }
        public string LookingFor { get; set; }

        //zaintersowania
        public string Interests { get; set; }
        public string FreeTime { get; set; }
        public string Sport { get; set; }
        public string Movies { get; set; }
        public string Music { get; set; }
        //zakładka preferencje
        public string ILike { get; set; }
        public string IDoNotLike { get; set; }
        public string MakesMeLaugh { get; set; }
        public string ItFeelsBestIn { get; set; }
        public string FriendsWouldDescribeMe { get; set; }
        //zdjęcia
        public ICollection<PhotoForDetailedDto> Photos { get; set; }
        public string PhotoUrl { get; set; }
    }
}
