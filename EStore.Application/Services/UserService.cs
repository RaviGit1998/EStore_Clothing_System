using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.Services
{
    public class UserService : IUserRepository
    {
        public Task<User> RegisterUser(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(c => c.Email == user.Email);
            if (existingUser != null)
            {
                return null;
            }
        }
    }
}
