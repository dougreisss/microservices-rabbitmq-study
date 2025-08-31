namespace Order.WebApi.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Model.Order>> GetAll();
        Task<Model.Order> GetById(int id);
        Task Create(Model.Order order);
        Task Update(Model.Order order);
        Task Delete(Model.Order order);
    }
}
