using Client.WebApi.Model;

namespace Client.WebApi.Repository.Interface
{
    public interface IClientRepository
    {
        Task<List<Model.Client>> GetAll();
        Task<Model.Client> GetById(int id);
        Task Create(Model.Client client);
        Task Update(Model.Client client);
        Task Delete(Model.Client client);    
    }
}
