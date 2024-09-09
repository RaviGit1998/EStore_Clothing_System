using EStore.Application.Interfaces;
using EStore.Domain.Entities;
using EStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private EStoreContext _context;
        public ProductRepository(EStoreContext context) 
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
