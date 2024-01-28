using Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceCampaign.Api;
public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    
    public static void ConfigureSqlContext(this IServiceCollection services,
    IConfiguration configuration) => 
        services.AddDbContext<RepositoryContext>(options =>
            options.UseMySql(configuration.GetConnectionString("sqlConnection"),
            new MySqlServerVersion(new Version(8, 0, 27)), b =>
            b.MigrationsAssembly("CustomerServiceCampaign.Api"))
        );
    
        
}
