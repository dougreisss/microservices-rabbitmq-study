using Microsoft.EntityFrameworkCore;
using Order.WebApi.Context;
using Order.WebApi.Repository;
using Order.WebApi.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// dbContext

builder.Services.AddDbContext<SqlContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("OrderDb")
    ));

// repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
