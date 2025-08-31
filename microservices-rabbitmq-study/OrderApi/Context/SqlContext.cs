using Microsoft.EntityFrameworkCore;

namespace Order.WebApi.Context
{
    public class SqlContext : DbContext
    {
        public SqlContext()
        {
            
        }
        public SqlContext(DbContextOptions<SqlContext> options) : base (options)
        {
            
        }

        public DbSet<Model.Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Model.Order>().HasData
            (
               new Model.Order
               {
                   Id = 1,
                   ProductId = 1,
                   ClientId = 1,
                   Count = 2
               }
            );
        }
    }
}
