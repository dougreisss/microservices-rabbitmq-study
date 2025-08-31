using Microsoft.EntityFrameworkCore;
using Product.WebApi.Context;
using Product.WebApi.Repository.Interface;

namespace Product.WebApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlContext _dbContext;
        public ProductRepository(SqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Model.Product>> GetAll()
        {
            return await _dbContext.Product.AsNoTracking().ToListAsync();
        }
        public async Task<Model.Product> GetById(int id)
        {
            return await _dbContext.Product.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task Create(Model.Product product)
        {
            await _dbContext.Product.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Model.Product product)
        {
            _dbContext.Product.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Model.Product product)
        {
            _dbContext.Product.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
