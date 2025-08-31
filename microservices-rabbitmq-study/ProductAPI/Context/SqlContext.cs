using Microsoft.EntityFrameworkCore;

namespace Product.WebApi.Context
{
    public class SqlContext : DbContext
    {
        public SqlContext()
        {
            
        }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
            
        }

        public DbSet<Model.Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Model.Product>().HasData
            (
               new Model.Product 
               {
                   Id = 1, 
                   Name = "Mouse",
                   Category = "Computing",
                   Description = "Mouse RGB",
                   Price = 200 
               }
            );
        }
    }
}
