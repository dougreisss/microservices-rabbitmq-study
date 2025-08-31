using Client.WebApi.Context;
using Client.WebApi.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Client.WebApi.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly SqlContext _dbContext;
        public ClientRepository(SqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Model.Client>> GetAll()
        {
            return await _dbContext.Client.AsNoTracking().ToListAsync();
        }
        public async Task<Model.Client> GetById(int id)
        {
            return await _dbContext.Client.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task Create(Model.Client client)
        {
            await _dbContext.Client.AddAsync(client);
            await _dbContext.SaveChangesAsync();    
        }

        public async Task Update(Model.Client client)
        {
            _dbContext.Client.Update(client);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Model.Client client)
        {
            _dbContext.Client.Remove(client);
            await _dbContext.SaveChangesAsync();
        }
    }
}
