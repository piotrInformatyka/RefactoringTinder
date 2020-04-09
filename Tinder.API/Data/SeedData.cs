using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinder.API.Models;

namespace Tinder.API.Data
{
    public class SeedData
    {
        private readonly DataContext _context;
        public SeedData(DataContext context)
        {
            _context = context;
        }
        public void SeedUsers()
        {
            var userData = File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            var i = 1;
            foreach(var user in users)
            {
                i++;
                byte[] passwordHash, passwordSalt;
                CreatePasswordHashSalt("password",out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLowerInvariant();
                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }

        private void CreatePasswordHashSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
