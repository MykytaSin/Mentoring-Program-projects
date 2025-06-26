using EventApi.Helpers;
using EventApi.Interfaces;
using EventApi.Services;

namespace EventApi
{
    public static class DIConfigurator
    {
        public static void ConfigureDI(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMapHelper, MapHelper>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IVenueService, VenueService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICacheHelper, CacheHelper>();
            builder.Services.AddScoped<IEmailProvider, SendGridEmailProvider>();

            builder.Services.Configure<ServiceBusSettings>(
                builder.Configuration.GetSection("ServiceBus"));

            builder.Services.AddScoped<IServiceBusMessageReceiver, ServiceBusMessageReceiver>();

            builder.Services.AddSingleton<IServiceBusMessageSender>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration["ServiceBus:ConnectionString"]
                    ?? throw new InvalidOperationException("ServiceBus:ConnectionString not found");
                var queueName = configuration["ServiceBus:QueueName"]
                    ?? throw new InvalidOperationException("ServiceBus:QueueName not found");
                return new ServiceBusMessageSender(connectionString, queueName);
            });


        }
    }
}
