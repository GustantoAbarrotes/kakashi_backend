using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Connection;
using static RepositoryLayer.Utils.Consts;

namespace RepositoryLayer
{
    public static class RepositoryDI
    {
        /// <summary>
        /// Here we register to the container DI, only restricted classes
        /// </summary>
        public static void Inject(IServiceCollection services) 
        {
            services.AddScoped<IDBManager, DBManager>();
        }
    }
}
