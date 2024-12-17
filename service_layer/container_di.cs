using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer;
using RepositoryLayer.Repositories.Product;
using RepositoryLayer.Repositories.Supplier;
using ServiceLayer.Product;
using ServiceLayer.Supplier;
using ServiceLayer.Version;

namespace ServiceLayer
{
    public static class ContainerDI
    {
        public static void Inject(IServiceCollection services, IConfiguration config)
        {
            //class internal layer repositorie
            RepositoryDI.Inject(services);
            //repositories
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            //services
            services.AddScoped<SupplierService>();
            services.AddScoped<ProductService>();
            services.AddScoped<VersionService>();
        }
    }
}
