using AdventureWorks.Database.Entities;
using AdventureWorks.Repository.Implements;
using AdventureWorks.Repository.Interfaces;
using AdventureWorks.Repository.UnitOfWork;
using AdventureWorks.Service.Implements;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<AdventureWorksContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            });
        services.AddMemoryCache();

        // Configure unit of work.
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register services
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IInventoryService, InventoryService>();

        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IReportService, ReportService>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        return services;
    }
}
