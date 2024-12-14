using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer;
using RepositoryLayer.Repositories.Supplier;
using ServiceLayer.Supplier;

namespace ServiceLayer
{
    public static class ContainerDI
    {
        public static void Inject(IServiceCollection services, IConfiguration config)
        {
            RepositoryDI.Inject(services);
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<SupplierService>();
        }
    }
}
