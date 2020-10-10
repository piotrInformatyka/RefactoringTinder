using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tinder.API.Models
{
    public class Like
    {
        public int UserLikesId { get; set; }
        public int UserIsLikedId { get; set; }

        public User UserLikes { get; set; }
        public User UserIsLiked { get; set; }
    }
}
