using DAL.Interfaces;
using DAL.UnitOfWork;
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
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IVenueService, VenueService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
        }
    }
}
