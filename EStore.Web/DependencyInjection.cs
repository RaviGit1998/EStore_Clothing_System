using EStore.Application;
using EStore.Infrastructure;

namespace EStore.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebDI(this IServiceCollection services)
        {
            services.AddApplicationDI()
                .AddInfrastructureDI();
            return services;
        }
    }
}
