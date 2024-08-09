using IdentityManager.Data;
using IdentityManager.Models.IdntityEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityManager.StartupExtensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection ServiceConfiguration(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("connstr"));
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
            });
            return services;
        }
    }
}
