using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tinder.API.Helper
{
    public static class Extensions
    {
        public static int CalculateAge(this DateTime dateTime)
        {
            var age = (DateTime.Today.Year - dateTime.Year);
            if (dateTime.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }
    }
}
