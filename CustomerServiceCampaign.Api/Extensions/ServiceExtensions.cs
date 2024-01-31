using Entities;
using Microsoft.AspNetCore.Identity;
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
    

    public static void ConfigureIdentity(this IServiceCollection services) 
    {
        var builder = services.AddIdentityCore<User>(o => {
            o.Password.RequireDigit = true; 
            o.Password.RequireLowercase = false; 
            o.Password.RequireUppercase = false; 
            o.Password.RequireNonAlphanumeric = false; 
            o.Password.RequiredLength = 6; 
            o.User.RequireUniqueEmail = true;
        });
        
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
        builder.AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
    }    
}
