namespace Order.WebApi.Model
{
    public class OrderSenderConfig
    {
        public string? Hostname { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Queue { get; set; }
    }
}
