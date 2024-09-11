using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using EStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infrastructure.Repositories
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {
        private readonly EStoreDbContext _dbContext;

        public ShippingAddressRepository(EStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ShippingAddress>> GetAllAddressesAsync()
        {
            return await _dbContext.ShippingAddresses.Include(sa => sa.User).ToListAsync();
        }

        public async Task<ShippingAddress> GetAddressByIdAsync(int id)
        {
            if(id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            return await _dbContext.ShippingAddresses.Include(sa => sa.User)
                                                   .FirstOrDefaultAsync(sa => sa.ShippingAddressId == id);
        }

        public async Task<IEnumerable<ShippingAddress>> GetAddressesByUserIdAsync(int userId)
        {
            return await _dbContext.ShippingAddresses.Include(sa => sa.User)
                                                   .Where(sa => sa.UserId == userId)
                                                   .ToListAsync();
        }

        public async Task AddAddressAsync(ShippingAddress address)
        {
            _dbContext.ShippingAddresses.Add(address);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(ShippingAddress address)
        {
            _dbContext.ShippingAddresses.Update(address);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(int id)
        {
            var address = await _dbContext.ShippingAddresses.FindAsync(id);
            if (address != null)
            {
                _dbContext.ShippingAddresses.Remove(address);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
