﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tinder.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
        //Podstawowe informacje
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
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
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Like> UserLikes { get; set; } //użytkownik lubi
        public ICollection<Like> UserIsLiked { get; set; } //użytkownik jest lubiany przez nich
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesRecived { get; set; }
    }
}
