using Microsoft.EntityFrameworkCore;

namespace Client.WebApi.Context
{
    public class SqlContext : DbContext
    {
        public SqlContext()
        {
            
        }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
            
        }

        public DbSet<Model.Client> Client { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Model.Client>().HasData
            (
                new Model.Client
                {
                    Id = 1,
                    Name = "Douglas Reis",
                    DocumentNumber = "05630557076",
                    Email = "dougreisrrs@gmail.com",
                    DateOfBirth = new DateOnly(2003, 05, 13)
                },
                new Model.Client
                {
                    Id = 2,
                    Name = "Wagner Reis",
                    DocumentNumber = "40543790010",
                    Email = "wagner@gmail.com",
                    DateOfBirth = new DateOnly(2007, 01, 21)
                }
            );
        }
    }
}
