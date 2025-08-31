using Microsoft.EntityFrameworkCore;
using Order.WebApi.Context;
using Order.WebApi.Repository;
using Order.WebApi.Repository.Interfaces;
using Order.WebApi.Services.Interface;
using Order.WebApi.Services;
using Order.WebApi.Model;

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

// services 
builder.Services.AddScoped<IOrderSenderService, OrderSenderService>();

// rabbitmq config

builder.Services.Configure<OrderSenderConfig>(builder.Configuration.GetSection("OrderSenderConfig"));

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
