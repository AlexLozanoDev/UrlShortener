using Application.Common.Interfaces;
using Infrastructure.Cache;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var redisHost = configuration["RedisSettings:Host"];
            var redisUser = configuration["RedisSettings:User"];
            var redisPassword = configuration["RedisSettings:Password"];

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseNpgsql(connectionString));

            services.AddStackExchangeRedisCache(options =>
            {
                var configOptions = new ConfigurationOptions
                {
                    EndPoints = { redisHost },
                    User = redisUser,
                    Password = redisPassword,
                    Ssl = true,
                    AbortOnConnectFail = false,
                    ConnectTimeout = 5000
                };

                options.ConfigurationOptions = configOptions;
            });

            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<ILinkRepository, LinkRepository>();
            services.AddScoped<IStatsRepository, StatsRepository>();

            return services;

        }
    }
}
