using Microsoft.Extensions.DependencyInjection;
using TaskApi.DAL.EF;
using TaskApi.DAL.Helpers;
using TaskApi.DAL.Interfaces;

namespace TaskApi.DAL.Infrastructure
{
    public static class IServiceCollectionExtension
    {
        public static void AddDALServices(this IServiceCollection services)
        {
            services.AddScoped<TicketDbContext>();
            services.AddScoped<IDateHelper, DateHelper>();
        }
    }
}
