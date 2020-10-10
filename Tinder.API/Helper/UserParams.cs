using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tinder.API.Helper
{
    public class UserParams
    {
        private const int MaxPageSize = 48;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 24;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public int UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
        public string ZodiacSign { get; set; } = "Wszystkie";
        public string OrderBy { get; set; }
        public bool UserLikes { get; set; } = false;
        public bool UserIsLiked { get; set; } = false;

    }
}
