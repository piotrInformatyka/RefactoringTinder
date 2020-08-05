using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinder.API.Models;

namespace Tinder.API.Data
{
    public class UserRepository : GenericRepository, IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext contex) : base(contex)
        {
            _context = contex;
        }

        public async Task<User> GetUser(int id)
        { 
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == id);
            return user;
        } 
        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }
        public async Task<Photo> GetPhoto (int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }
        public async Task<Photo> GetMainPhoto(int userId)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.IsMain && p.UserId == userId);
            return photo;
        }
    }
}
