
using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using EStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private EStoreDbContext _context;
        public ProductRepository(EStoreDbContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductsByIdAsync(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

       
    }
}
