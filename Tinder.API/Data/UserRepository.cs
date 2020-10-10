﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinder.API.Helper;
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
        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);
            if(!string.IsNullOrEmpty(userParams.Gender))
                users = users.Where(u => u.Gender == userParams.Gender);

            if(userParams.UserLikes)
            {
                var userLikes = await GetUserLikes(userParams.UserId, userParams.UserLikes);
                users = users.Where(u => userLikes.Contains(u.Id));
            }
            if(userParams.UserIsLiked)
            {
                var userIsLiked = await GetUserLikes(userParams.UserId, userParams.UserLikes);
                users = users.Where(u => userIsLiked.Contains(u.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 100)
            {
                var minDate = DateTime.Today.AddYears(-userParams.MaxAge);
                var maxDate = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.Birthday >= minDate && u.Birthday <= maxDate);
            }

            if(userParams.ZodiacSign != "Wszystkie")
            {
                users = users.Where(u => u.ZodiacSign == userParams.ZodiacSign);
            }
            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.CreatedAt);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateList(users, userParams.PageNumber, userParams.PageSize);
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
        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.UserLikesId == userId && u.UserIsLikedId == recipientId);
        }
        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
                                            .Include(u => u.Recipient).ThenInclude(p => p.Photos).AsQueryable();
            switch(messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false && u.RecipientDeleted == false);
                    break;
            }
            messages = messages.OrderBy(d => d.DateSent);

            return await PagedList<Message>.CreateList(messages, messageParams.PageNumber, messageParams.PageSize);

        }
        public async  Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId)
        {
            var messages = await _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
                                             .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                                             .Where(m => (m.RecipientId == userId && m.SenderId == recipientId && m.RecipientDeleted == false)||
                                                   (m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false))
                                             .OrderByDescending(m => m.DateSent).ToListAsync();
            return messages;
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool userLikes)
        {
            var user = await _context.Users.Include(x => x.UserLikes).Include(x => x.UserIsLiked).FirstOrDefaultAsync(u => u.Id == id);
            if(userLikes)
            {
                return user.UserLikes.Where(u => u.UserIsLikedId == id).Select(u => u.UserLikesId);
            }
            else
                return user.UserIsLiked.Where(u => u.UserLikesId == id).Select(u => u.UserIsLikedId);
        }
    }
}
