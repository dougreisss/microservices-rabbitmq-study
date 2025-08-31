namespace Order.WebApi.Services.Interface
{
    public interface IOrderSenderService
    {
        Task OrderSender(Model.Order order);
    }
}
