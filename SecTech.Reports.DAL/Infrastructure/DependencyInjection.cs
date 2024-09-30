using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecTech.DAL.Repositories;
using SecTech.Reports.Domain.Entity;
using SecTech.Reports.Domain.Interfaces.Repository;

namespace SecTech.Reports.DAL.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration options)
        {
            var connectionString = options.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connectionString); });
            services.InitRepositories();
        }

        private static void InitRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Attendance>, BaseRepository<Attendance>>();
            

        }

    }
}
