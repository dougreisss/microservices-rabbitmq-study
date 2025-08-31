namespace Product.WebApi.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<Model.Product>> GetAll();
        Task<Model.Product> GetById(int id);
        Task Create(Model.Product product);
        Task Update(Model.Product product);
        Task Delete(Model.Product product);
    }
}
