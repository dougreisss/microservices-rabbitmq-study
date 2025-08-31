using PaymentConsumer.Services;
using PaymentConsumer.Services.Interfaces;
using PaymentWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddScoped<IPaymentService, PaymentService>();

var host = builder.Build();
host.Run();
