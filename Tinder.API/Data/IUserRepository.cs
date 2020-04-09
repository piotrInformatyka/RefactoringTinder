﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinder.API.Models;

namespace Tinder.API.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);

    }
}