namespace Product.WebApi.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<Model.Product>> GetAll();
        Task<Model.Product> GetById(int id);
        Task Create(Model.Product client);
        Task Update(Model.Product client);
        Task Delete(Model.Product client);
    }
}
