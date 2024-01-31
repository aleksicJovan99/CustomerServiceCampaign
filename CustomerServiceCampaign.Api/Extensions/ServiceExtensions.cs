using System.Text;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

    // Configures JSON Web Token (JWT) authentication
    public static void ConfigureJWT(this IServiceCollection services, 
    IConfiguration configuration) 
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = configuration.GetSection("AppSettings:Token").Value!;

        services.AddAuthentication(o => {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => 
        {
            options.TokenValidationParameters = new TokenValidationParameters 
            {
                ValidateIssuer = false, 
                ValidateAudience = true, 
                ValidateLifetime = true, 
                ValidateIssuerSigningKey = true,

                //ValidIssuer = jwtSettings.GetSection("validIssuer").Value, 
                ValidAudience = jwtSettings.GetSection("validAudience").Value, 
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) 
            };
        });
    }
    
}
