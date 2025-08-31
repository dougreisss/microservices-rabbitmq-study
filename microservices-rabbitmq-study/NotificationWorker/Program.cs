using NotificationConsumer.Services;
using NotificationConsumer.Services.Interfaces;
using NotificationWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<NotificationConsumerWorker>();

builder.Services.AddScoped<INotificationService, NotificationService>();

var host = builder.Build();
host.Run();
