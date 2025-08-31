using Microsoft.EntityFrameworkCore;
using Order.WebApi.Context;
using Order.WebApi.Repository.Interfaces;

namespace Order.WebApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SqlContext _dbContext;
        public OrderRepository(SqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Model.Order>> GetAll()
        {
            return await _dbContext.Order.AsNoTracking().ToListAsync();
        }
        public async Task<Model.Order> GetById(int id)
        {
            return await _dbContext.Order.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task Create(Model.Order order)
        {
            await _dbContext.Order.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Model.Order order)
        {
            _dbContext.Order.Update(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Model.Order order)
        {
            _dbContext.Order.Remove(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}
